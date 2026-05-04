var builder = DistributedApplication.CreateBuilder(args);

const int ServerAppPort = 5010;

var _server = builder.AddProject<Projects.Website_Server>("website", options => options.ExcludeLaunchProfile = true)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    .WithHttpEndpoint(targetPort: ServerAppPort, port: ServerAppPort, name: "http", isProxied: false);

builder.Build().Run();
