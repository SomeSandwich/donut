using Identity.API.Infrastructure.Startup;
using Serilog;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;

namespace Identity.API;

internal sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddServiceDiscovery(s => s.UseEureka());

        // Logging.
        builder.Services.AddSerilog(new SerilogConfiguration(configuration).Setup);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
