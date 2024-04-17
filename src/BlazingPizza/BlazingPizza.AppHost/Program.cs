var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddSqlServer("blazingpizza-db")
    .AddDatabase("DB", "BlazingPizza");

var server = builder.AddProject<Projects.BlazingPizza_Server>("blazingpizza-server")
    .WithReference(db);

var app = builder.AddProject<Projects.BlazingPizza_App>("blazingpizza-app")
    .WithReference(server);

builder.Build().Run();
