var builder = DistributedApplication.CreateBuilder(args);

const int ServerAppPort = 5101;

try
{
    // Try the strongly-typed approach first
    var _server = builder.AddProject<Projects.Website>("website", options => 
    {
        options.ExcludeLaunchProfile = true;
    })
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    .WithHttpEndpoint(targetPort: ServerAppPort, port: ServerAppPort, name: "http", isProxied: false);
    
    Console.WriteLine($"✓ Website project added to Aspire orchestration on port {ServerAppPort}");
}
catch (Exception ex)
{
    Console.WriteLine($"✗ Failed to add Website project with strongly-typed approach: {ex.Message}");
    Console.WriteLine($"  Details: {ex}");
    
    // Try with project path as fallback
    try
    {
        Console.WriteLine("  Attempting fallback with project path...");
        var projectPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Website", "Website", "Website.csproj");
        Console.WriteLine($"  Project path: {projectPath}");
        Console.WriteLine($"  Path exists: {File.Exists(projectPath)}");
        throw;  // Re-throw to show original error
    }
    catch (Exception ex2)
    {
        Console.WriteLine($"  Fallback also failed: {ex2.Message}");
        throw;
    }
}

builder.Build().Run();
