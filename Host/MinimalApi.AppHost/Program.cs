using CommunityToolkit.Aspire.Hosting.Dapr;

var builder = DistributedApplication.CreateBuilder(args);

////var postgres = builder.AddPostgres("postgres")
////    .WithDataVolume()
////    .AddDatabase("url-shortener");

////var redis = builder.AddRedis("redis");
////builder.AddProject<Projects.UrlShortening_Api>("urlshortening-api")
////    .WithReference(postgres)
////    .WithReference(redis)
////    .WaitFor(postgres)
////    .WaitFor(redis);

////builder.AddProject<Projects.Stocks_Realtime_Api>("stocks-realtime-api");

////builder.AddProject<Projects.SignalRIntro_Api>("signalrintro-api");

////builder.AddProject<Projects.KafkaImplementation>("kafkaimplementation");

////builder.AddProject<Projects.WebHook_Publisher_Api>("webhook-publisher-api");

////builder.AddProject<Projects.WebHook_Consumer_Api>("webhook-consumer-api");

////builder.AddProject<Projects.WebHook_App>("webhook-app");

////builder.AddProject<Projects.Exotic_WebHook_Api>("exotic-webhook-api");

builder.AddProject<Projects.Dapr_eShop_Orders>("dapr-eshop-orders")
    .WithDaprSidecar(new DaprSidecarOptions { AppId = "orders-api" });

builder.AddProject<Projects.Dapr_eShop_Checkout>("dapr-eshop-checkout")
    .WithDaprSidecar(new DaprSidecarOptions { AppId = "checkout-api" });

builder.Build().Run();
