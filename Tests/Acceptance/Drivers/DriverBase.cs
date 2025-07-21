using Api;
using Application.Common.Interfaces;
using Infrustructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace AcceptanceTests.Drivers;

public abstract class DriverBase
{
    private readonly IntegrationsWebApplicationFactory<Program> _factory;
    public readonly HttpClient HttpClient;
    
    public ApplicationDbContext? DbContext;

    public DriverBase(IntegrationsWebApplicationFactory<Program> factory)
    {
        _factory = factory;        
        HttpClient = _factory.CreateClient();
        GetDbContext();
    }

    private void GetDbContext()
    {
        IServiceScopeFactory scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
        IServiceScope scope = scopeFactory.CreateScope();
        DbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        DbContext.Database.EnsureCreated();
    }
}
