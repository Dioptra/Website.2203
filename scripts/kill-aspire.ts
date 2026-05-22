const ports = [5000, 19142, 20007, 21127, 22169];
const lsofPath = "/usr/sbin/lsof";
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

console.log("aspire killed");
