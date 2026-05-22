const ports = [5000, 19142, 20007, 21127, 22169];
const lsofPath = "/usr/sbin/lsof";
const dotnetPath = "/usr/local/share/dotnet/dotnet";
const pathValue = "/usr/local/share/dotnet:/opt/homebrew/bin:/usr/local/bin:/usr/bin:/bin:/usr/sbin:/sbin";

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
  stdout: "inherit",
  stderr: "inherit",
});

const child = run.spawn();
const status = await child.status;
Deno.exit(status.code);
