# Aristocrab.AspNetCore.AppModules [(nuget)](https://www.nuget.org/packages/Aristocrab.AspNetCore.AppModules/)
Library that can help you organize your ASP.NET Core application into modules.

[![NuGet](https://img.shields.io/nuget/v/Aristocrab.AspNetCore.AppModules.svg)](https://www.nuget.org/packages/Aristocrab.AspNetCore.AppModules/)
[![NuGet](https://img.shields.io/nuget/dt/Aristocrab.AspNetCore.AppModules.svg)](https://www.nuget.org/packages/Aristocrab.AspNetCore.AppModules/)

`dotnet add package Aristocrab.AspNetCore.AppModules`

---

## Example

`Program.cs`
```csharp
using Aristocrab.AspNetCore.AppModules;

var builder = WebApplication.CreateBuilder(args);
builder.AddModules();

var app = builder.Build();
app.UseModules();

app.Run();
```

`CommonModule.cs`
```csharp
using Aristocrab.AspNetCore.AppModules;

namespace KoineCrm.WebApi.Modules.Common;

public class CommonModule : AppModule
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
    }

    public override void ConfigureApplication(WebApplication app)
    {
        app.MapControllers();
    }
}
```
