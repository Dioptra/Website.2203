const ports = [5000, 19142, 20007, 21127, 22169];
const lsofPath = "/usr/sbin/lsof";
const dotnetPath = "/usr/local/share/dotnet/dotnet";
const pathValue = "/usr/local/share/dotnet:/opt/homebrew/bin:/usr/local/bin:/usr/bin:/bin:/usr/sbin:/sbin";

const encoder = new TextEncoder();
const decoder = new TextDecoder();
const urlPattern = /Now listening on:\s+(https?:\/\/\S+)/;

let browserOpened = false;

const normaliseBrowserUrl = (url: string) => url.replace("://localhost", "://127.0.0.1");

const openInDefaultBrowser = async (url: string) => {
  const browserUrl = normaliseBrowserUrl(url);
  let command: string[] | null = null;

  switch (Deno.build.os) {
    case "darwin":
      command = ["open", browserUrl];
      break;
    case "linux":
      command = ["xdg-open", browserUrl];
      break;
    case "windows":
      command = ["cmd", "/c", "start", "", browserUrl];
      break;
  }

  if (command === null) {
    return;
  }

  try {
    const process = new Deno.Command(command[0], {
      args: command.slice(1),
      stdin: "null",
      stdout: "null",
      stderr: "null",
    }).spawn();

    await process.status;
  } catch {
    // Keep Aspire running even if the browser launch fails.
  }
};

const forwardStream = async (stream: ReadableStream<Uint8Array> | null, writer: WritableStreamDefaultWriter<Uint8Array>) => {
  if (stream === null) {
    return;
  }

  let buffered = "";

  for await (const chunk of stream) {
    await writer.write(chunk);

    buffered += decoder.decode(chunk, { stream: true });

    if (!browserOpened) {
      const match = buffered.match(urlPattern);
      if (match !== null) {
        browserOpened = true;
        await openInDefaultBrowser(match[1]);
      }
    }

    if (buffered.length > 4096) {
      buffered = buffered.slice(-2048);
    }
  }

  buffered += decoder.decode();

  if (!browserOpened) {
    const match = buffered.match(urlPattern);
    if (match !== null) {
      browserOpened = true;
      await openInDefaultBrowser(match[1]);
    }
  }
};

for (const port of ports) {
  const lookup = new Deno.Command(lsofPath, {
    args: ["-ti", `:${port}`],
    stdout: "piped",
    stderr: "null",
  });

  const output = await lookup.output();
  const pids = new TextDecoder().decode(output.stdout).trim().split(/\s+/).filter(Boolean);

  for (const pid of pids) {
    try {
      Deno.kill(Number(pid), "SIGKILL");
    } catch {
      // Ignore races where the process exits before we can kill it.
    }
  }
}

const run = new Deno.Command(dotnetPath, {
  args: ["run"],
  cwd: "AspireAppHost",
  env: { PATH: pathValue },
  stdin: "inherit",
  stdout: "piped",
  stderr: "piped",
});

const child = run.spawn();
const stdoutWriter = Deno.stdout.writable.getWriter();
const stderrWriter = Deno.stderr.writable.getWriter();

try {
  await Promise.all([
    forwardStream(child.stdout, stdoutWriter),
    forwardStream(child.stderr, stderrWriter),
  ]);
} finally {
  stdoutWriter.releaseLock();
  stderrWriter.releaseLock();
}

const status = await child.status;
Deno.exit(status.code);
