using Microsoft.Extensions.DependencyInjection;
using Webion.Templates.Mustache.Abstractions;
using Webion.Templates.Mustache.Services;

namespace Webion.Templates.Mustache.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateProcess(this IServiceCollection services)
    {
        services.AddSingleton<ITemplateProcessService, TemplateProcessService>();
        return services;
    }
}
