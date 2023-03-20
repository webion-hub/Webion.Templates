using Microsoft.Extensions.DependencyInjection;
using Webion.Templates.Infrastructure.Abstractions;
using Webion.Templates.Infrastructure.Repositories;

namespace Webion.Templates.Infrastructure.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<ITemplatesRepository, TemplatesRepository>();
        return services;
    }
}
