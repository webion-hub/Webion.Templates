using Kaire.Templates.Api.Config;

namespace Kaire.Templates.Api.Extensions;

internal static class WebApplicationExtension
{
    public static void Use<TStartupConfig>(this WebApplication app)
        where TStartupConfig : IWebApplicationConfig, new()
    {
        new TStartupConfig().Use(app);
    }
}
