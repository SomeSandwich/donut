using System.Reflection;
using Scalar.AspNetCore;
using SomeSandwich.Donut.Identity.Infrastructure.Extensions;
using SomeSandwich.Donut.Identity.Infrastructure.Startup;
using SomeSandwich.Donut.Identity.Infrastructure.Startup.OpenApi;

namespace SomeSandwich.Donut.Identity;

/// <summary>
/// Entry point class.
/// </summary>
public static class Program
{
    /// <summary>
    /// Entry point method.
    /// </summary>
    /// <param name="args">Program arguments.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
        builder.Services.AddAuthorization();
        builder.Services.AddOpenApi(new OpenApiOptionSetup().Setup);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(new ScalarOptionSetup().Setup);
        }

        app.MapEndpoints();
        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.Run();
    }
}
