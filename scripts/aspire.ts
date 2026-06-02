const ports = [5000, 5101, 19142, 20007, 21127, 22169];
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
    // Keep service running even if the browser launch fails.
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

// Kill any existing processes on the ports we need
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

// Build the solution first to ensure all assets are generated
console.log("Building solution...");
const buildCmd = new Deno.Command(dotnetPath, {
  args: ["build", "Website.2203.slnx"],
  env: { PATH: pathValue },
  stdout: "inherit",
  stderr: "inherit",
});

const buildStatus = await buildCmd.output();
if (buildStatus.code !== 0) {
  console.error("Build failed!");
  Deno.exit(1);
}

// Start the Website project directly instead of through Aspire orchestration
// This is simpler for local development and avoids OTLP infrastructure requirements
const websitePort = 5101;
console.log(`\nStarting Website on port ${websitePort}...\n`);
const run = new Deno.Command(dotnetPath, {
  args: ["run", "--no-build"],
  cwd: "Website/Website",
  env: { 
    PATH: pathValue,
    ASPNETCORE_ENVIRONMENT: "Development",
    ASPNETCORE_URLS: `http://+:${websitePort}`,
    Kestrel__Endpoints__Http__Url: `http://+:${websitePort}`,
  },
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
