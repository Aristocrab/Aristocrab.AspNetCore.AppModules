using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Aristocrab.AspNetCore.AppModules;

/// <summary>
/// Provides extension methods for discovering and executing <see cref="AppModule"/> instances
/// from assemblies in an ASP.NET Core application.
/// </summary>
public static class AppModulesExtensions
{
    private static readonly Func<Type, bool> IsNonAbstractAppModule =
        type => typeof(AppModule).IsAssignableFrom(type) && !type.IsAbstract;

    /// <summary>
    /// Discovers and registers modules from the calling assembly into the DI container.
    /// </summary>
    /// <param name="builder">The application builder.</param>
    public static void AddModules(this WebApplicationBuilder builder)
    {
        AddModules(builder, Assembly.GetCallingAssembly());
    }

    /// <summary>
    /// Discovers and registers modules from the specified assemblies into the DI container.
    /// </summary>
    /// <param name="builder">The application builder.</param>
    /// <param name="assemblies">Assemblies to scan for modules.</param>
    public static void AddModules(this WebApplicationBuilder builder, params Assembly[] assemblies)
    {
        AddAppModules(builder, assemblies);
    }

    private static void AddAppModules(this WebApplicationBuilder builder, params Assembly[] assemblies)
    {
        var modulesCollection = new AppModulesCollection();

        foreach (var assembly in assemblies)
        {
            var builderServiceProvider = builder.Services.BuildServiceProvider();

            var modules = assembly.GetTypes().Where(IsNonAbstractAppModule);

            var instances = modules
                .Select(x => ActivatorUtilities.CreateInstance(builderServiceProvider, x))
                .Cast<AppModule>()
                .ToList();

            foreach (var instance in instances
                         .Where(module => module.Enabled)
                         .OrderBy(x => x.OrderIndex))
            {
                instance.ConfigureServices(builder);
                modulesCollection.AppModules.Add(instance);
            }
        }

        builder.Services.AddSingleton(modulesCollection);
    }

    /// <summary>
    /// Configures the application pipeline by calling <see cref="AppModule.ConfigureApplication"/>
    /// on all enabled modules in the order defined by <see cref="AppModule.OrderIndex"/>.
    /// </summary>
    /// <param name="app">The application instance.</param>
    public static void UseModules(this WebApplication app)
    {
        var modulesCollection = app.Services.GetRequiredService<AppModulesCollection>();

        foreach (var module in modulesCollection.AppModules
                     .Where(module => module.Enabled)
                     .OrderBy(x => x.OrderIndex))
        {
            module.ConfigureApplication(app);
        }
    }
}