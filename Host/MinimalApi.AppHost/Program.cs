var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .AddDatabase("url-shortener");

var redis = builder.AddRedis("redis");
builder.AddProject<Projects.UrlShortening_Api>("urlshortening-api")
    .WithReference(postgres)
    .WithReference(redis)
    .WaitFor(postgres)
    .WaitFor(redis);

builder.AddProject<Projects.Stocks_Realtime_Api>("stocks-realtime-api");

builder.AddProject<Projects.SignalRIntro_Api>("signalrintro-api");

builder.AddProject<Projects.KafkaImplementation>("kafkaimplementation");

builder.Build().Run();
