using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kaire.Templates.Api.Config;

public sealed class ControllersConfig : IWebApplicationConfig
{
    public void AddServices(WebApplicationBuilder builder)
    {
        builder.Services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
    }

    public void Use(WebApplication app)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
