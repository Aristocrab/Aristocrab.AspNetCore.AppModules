using Microsoft.AspNetCore.Builder;

namespace Aristocrab.AspNetCore.AppModules;

/// <summary>
/// Represents a modular component of the application that can contribute to service registration
/// and application pipeline configuration.
/// </summary>
public abstract class AppModule
{
    /// <summary>
    /// Gets or sets a value indicating whether the module is enabled. If set to false, the module will be skipped
    /// during application startup. Default is true.
    /// </summary>
    public virtual bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the order index for the module.
    /// Modules with lower OrderIndex values are initialized earlier.
    /// </summary>
    public virtual int OrderIndex { get; set; }

    /// <summary>
    /// Configures services for the module. Override this method to register services
    /// into the dependency injection container.
    /// </summary>
    /// <param name="builder">The application builder used to configure services.</param>
    public virtual void ConfigureServices(WebApplicationBuilder builder) { }

    /// <summary>
    /// Configures the application's request pipeline. Override this method to add middleware,
    /// endpoints, or other application-level settings.
    /// </summary>
    /// <param name="app">The application instance used to configure the request pipeline.</param>
    public virtual void ConfigureApplication(WebApplication app) { }
}