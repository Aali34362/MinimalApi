﻿//<auto-generated/>
using MinimalApi.EndPoints;
namespace MinimalApi.EndPoints;

public static class EndpointsExtension
{
    public static IEndpointRouteBuilder MapAllEndpoints(this IEndpointRouteBuilder endpoints)
    {

           new OrganizationEndPoint().MapEndPoints(endpoints);
           new UserEndPoint().MapEndPoints(endpoints);
        return endpoints;
    }
}
