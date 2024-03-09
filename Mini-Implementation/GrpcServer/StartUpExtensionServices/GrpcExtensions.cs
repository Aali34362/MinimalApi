using System.Reflection;

namespace GrpcServer.StartUpExtensionServices;

public static class GrpcExtensions
{
    public static void MapGrpcServicesFromAssembly(this IApplicationBuilder app, Assembly assembly)
    {
        //var grpcServiceType = typeof(IGrpcService<>);

        //foreach (var type in assembly.GetTypes())
        //{
        //    // Check if the type implements the desired gRPC service interface
        //    var implementedInterface = type.GetInterfaces().FirstOrDefault(i =>
        //        i.IsGenericType && i.GetGenericTypeDefinition() == grpcServiceType);

        //    if (implementedInterface != null)
        //    {
        //        // Construct the concrete type of the service
        //        var concreteType = grpcServiceType.MakeGenericType(implementedInterface.GetGenericArguments());

        //        // Use reflection to invoke MapGrpcService<T> with the current service type
        //        var genericMethod = typeof(GrpcEndpointRouteBuilderExtensions)
        //            .GetMethod(nameof(GrpcEndpointRouteBuilderExtensions.MapGrpcService), new[] { typeof(IEndpointRouteBuilder) })
        //            .MakeGenericMethod(concreteType);

        //        genericMethod.Invoke(null, new object[] { app });
        //    }
        //}
    }
}
