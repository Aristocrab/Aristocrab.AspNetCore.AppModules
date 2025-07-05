namespace Aristocrab.AspNetCore.AppModules;

/// <summary>
/// Represents a container for storing discovered and initialized <see cref="AppModule"/> instances.
/// This collection is populated during application startup and used to configure the application pipeline.
/// </summary>
public class AppModulesCollection
{
    /// <summary>
    /// Gets the list of active <see cref="AppModule"/> instances that have been discovered and initialized.
    /// </summary>
    public List<AppModule> AppModules { get; } = [];
}