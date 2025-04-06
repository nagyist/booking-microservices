using BuildingBlocks.Core;
using BuildingBlocks.EFCore;
using Identity.Configurations;
using Identity.Data;
using Identity.Data.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Extensions.Infrastructure;


public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddIdentityModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEventMapper, EventMapper>();
        builder.AddCustomDbContext<IdentityContext>(nameof(Identity));
        builder.Services.AddScoped<IDataSeeder, IdentityDataSeeder>();
        builder.AddCustomIdentityServer();

        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

        builder.Services.AddCustomMediatR();

        return builder;
    }


    public static WebApplication UseIdentityModules(this WebApplication app)
    {
        app.UseForwardedHeaders();
        app.UseIdentityServer();
        app.UseMigration<IdentityContext>();

        return app;
    }
}
