using Kaire.Templates.Api.Config;

namespace Kaire.Templates.Api.Extensions;

internal static class WebApplicationBuilderExtension
{
    public static void Add<TStartupConfig>(this WebApplicationBuilder builder)
        where TStartupConfig : IWebApplicationConfig, new()
    {
        new TStartupConfig().AddServices(builder);
    }
}
