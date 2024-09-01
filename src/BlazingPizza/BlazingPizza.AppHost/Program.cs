var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddSqlServer("blazingpizza-db")
    .WithDataVolume()
    .PublishAsAzureSqlDatabase()
    .AddDatabase("DB", "BlazingPizza");

var server = builder.AddProject<Projects.BlazingPizza_Server>("blazingpizza-server")
    .WithReference(db);

var app = builder.AddProject<Projects.BlazingPizza_App>("blazingpizza-app")
    .WithExternalHttpEndpoints()
    .WithReference(server);

builder.Build().Run();
