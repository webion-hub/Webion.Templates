using Webion.Templates.Http.Abstraction;
using Webion.Templates.Http.Client;
using Webion.Templates.Http.Options;

namespace Webion.Templates.Http.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddTemplatesClient(this IServiceCollection services, string connectionUrl)
    {
        services.Configure<TemplatesOptions>(config =>
        {
            config.Url = connectionUrl;
        });
        services.AddHttpClient<ITemplatesClient, TemplatesClient>();

        return services;
    }
}
