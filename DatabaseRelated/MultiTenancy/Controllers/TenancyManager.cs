﻿namespace MultiTenancy.Controllers;

public interface ITenant
{
    int Id { get; }
    string Title { get; }
}

public interface ITenantSetter
{
    int Id { set; }
    string Title { set; }
}

public class Tenant : ITenant, ITenantSetter
{
    public int Id { get; set; }
    public required string Title { get; set; }
}

public interface ITenancyManager
{
    Tenant? GetTenant(string tenantName);
    Tenant? GetTenant(int tenantId);
}

public class TenancyManager : ITenancyManager
{
    public Tenant? GetTenant(string tenantName)
     => tenantName switch
     {
         "client1" => new Tenant() { Id = 1, Title = "Client1" },
         "client2" => new Tenant() { Id = 2, Title = "Client2" },
         "client3" => new Tenant() { Id = 3, Title = "Client3" },
         _ => null
     };
    public Tenant? GetTenant(int tenantId)
     => tenantId switch
     {
         1 => new Tenant() { Id = 1, Title = "Client1" },
         2 => new Tenant() { Id = 2, Title = "Client2" },
         3 => new Tenant() { Id = 3, Title = "Client3" },
         _ => null
     };
}
