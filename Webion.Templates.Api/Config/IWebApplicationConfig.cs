namespace Kaire.Templates.Api.Config;

public interface IWebApplicationConfig
{
    void AddServices(WebApplicationBuilder builder);
    void Use(WebApplication app);
}
