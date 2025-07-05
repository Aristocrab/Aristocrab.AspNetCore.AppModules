using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Aristocrab.AspNetCore.AppModules;

public static class AppModulesExtensions
{
    private static readonly Func<Type, bool> IsNonAbstractAppModule =
        type => typeof(AppModule).IsAssignableFrom(type) && !type.IsAbstract;

    public static void AddModules(this WebApplicationBuilder builder)
    {
        AddModules(builder, Assembly.GetCallingAssembly());
    }

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