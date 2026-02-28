using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.AssociatedProducts.Services;

namespace Nop.Plugin.Widgets.AssociatedProducts.Infrastructure;

/// <summary>
/// Registers plugin services into the DI container at application startup
/// </summary>
public class NopStartup : INopStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAssociatedProductService, AssociatedProductService>();
    }

    public void Configure(IApplicationBuilder application)
    {
    }

    /// <summary>
    /// Executed after core nopCommerce registrations (order 3000)
    /// </summary>
    public int Order => 3000;
}
