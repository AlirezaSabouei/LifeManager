using Api;
using Application.Common.Interfaces;
using Infrustructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace AcceptanceTests.Drivers;

public abstract class DriverBase
{
    public IntegrationsWebApplicationFactory<Program> Factory { get; set; }
    public readonly HttpClient HttpClient;
    
    public ApplicationDbContext? DbContext;

    public DriverBase(IntegrationsWebApplicationFactory<Program> factory)
    {
        Factory = factory;        
        HttpClient = Factory.CreateClient();
        GetDbContext();
    }

    private void GetDbContext()
    {
        IServiceScopeFactory scopeFactory = Factory.Services.GetRequiredService<IServiceScopeFactory>();
        IServiceScope scope = scopeFactory.CreateScope();
        DbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        DbContext.Database.EnsureCreated();
    }
}
