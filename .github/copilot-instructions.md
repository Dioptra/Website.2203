# Dioptra Website — AI Development Guide

> Blazor dual-mode (Server + WebAssembly) marketing and contact website for Dioptra Limited

---

## Quick Reference

| Item               | Value                                                    |
| :----------------- | :------------------------------------------------------- |
| **Framework**      | .NET 10.0 (`net10.0`)                                    |
| **Build**          | `dotnet build Website.2203.slnx`                         |
| **Run (Aspire)**   | `cd AspireAppHost && dotnet run`                         |
| **Frontend build** | `cd Website.Client && npm install && npx webpack`        |
| **Logging**        | Serilog + `ILogger<T>` injection (not `Console.WriteLine`) |

---

## ⛔ Critical Rules

### Solution-Wide Build Properties

`Directory.Build.props` enforces these settings across all projects:

```xml
<TargetFramework>net10.0</TargetFramework>
<ImplicitUsings>enable</ImplicitUsings>
<Nullable>enable</Nullable>
```

**Never override these per-project.** Build artifacts go to `.artifacts/`.

### Project Architecture

| Project                  | Purpose                                          | Tech                        |
| :----------------------- | :----------------------------------------------- | :-------------------------- |
| **Website.Server**       | Main web server hosting both render modes        | ASP.NET Core 10, Serilog    |
| **Website.Client**       | Shared Blazor components, TypeScript/SCSS assets | Razor, webpack, TypeScript  |
| **Website.WebAssembly**  | Blazor WebAssembly client shell                  | Blazor WASM                 |
| **AspireAppHost**        | .NET Aspire orchestration (dev only)             | .NET Aspire                 |

---

## AI Agent Guidelines

- **No sycophancy** — Direct technical feedback; identify issues; skip the praise
- **Don't bowdlerise** — Be frank; swearing's fine if the idea is wrong
- **Incremental changes** — One small change → verify → next change
- **Preserve working code** — Functionality first; never break working code for "better" architecture

---

## Development Workflows

| Task              | Command                                      |
| :---------------- | :------------------------------------------- |
| Build solution    | `dotnet build Website.2203.slnx`             |}
| Run via Aspire    | `cd AspireAppHost && dotnet run`             |
| Deno build        | `deno task build`                            |
| Deno rebuild      | `deno task rebuild`                          |
| Deno aspire       | `deno task aspire`                           |

---

## C# Standards

### Logging

Use Serilog via `ILogger<T>` injection, not `Console.WriteLine`:

```csharp
[Inject] private ILogger<MyComponent> Logger { get; set; }

Logger.LogInformation("Page loaded");
Logger.LogError(ex, "Failed to send notification");
```

### Naming Conventions

Per `.editorconfig`:

- Interfaces: `I` prefix (e.g., `IMessage`, `INotification`)
- Types: PascalCase
- Properties, methods, events: PascalCase
- File-scoped namespaces preferred
- Explicit accessibility modifiers for non-interface members

### General

- `ImplicitUsings=enable` — global usings are available
- `Nullable=enable` — use nullable reference type annotations
- Expression-bodied members where appropriate
- File-scoped namespaces
- Null-coalescing (`??`) and null-propagation (`?.`) operators preferred

---

## TypeScript Standards

- **Entry point**: `Website.Client/WebAssets/Scripts/Main.ts`
- **Build**: webpack via `npm run build` or `npx webpack`
- **Output**: `wwwroot/js/`
- `const`/`let` over `var`
- Avoid `any` unless necessary
- async/await for async operations

---

## Markdown Standards

- Wrap prose to 180 characters where practical. Do not wrap tables, URLs, or code blocks.
- **Every** fenced code block must have a language tag — bare ` ``` ` is never acceptable
- Table alignment: `:---` left, `---:` right (numbers), `:---:` centre
- Prefer Mermaid diagrams over ASCII art

---

## Key Files

| File/Directory                    | Purpose                                          |
| :-------------------------------- | :----------------------------------------------- |
| `Directory.Build.props`           | Solution-wide MSBuild properties                 |
| `deno.json`                       | Deno task runner (build, rebuild, aspire)        |
| `AspireAppHost/Program.cs`        | .NET Aspire orchestration entry point            |
| `Website.Server/Program.cs`       | ASP.NET Core server entry point                  |
| `Website.Client/`                 | Shared Razor components and web assets           |
| `.vscode/tasks.json`              | VS Code build tasks                              |
| `.vscode/launch.json`             | VS Code launch configuration (Aspire)            |
