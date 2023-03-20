using Webion.Firestore.Abstractions;
using Webion.Firestore.Options;

namespace Webion.Firestore.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddFirestore<TDbContext>(this IServiceCollection services,
        string projectId, 
        bool emulation
    )
        where TDbContext : FirestoreDbContext<TDbContext>
    {
        services.Configure<FirestoreDbOptions<TDbContext>>(options =>
        {
            options.ProjectId = projectId;
            options.Emulation = emulation;
        });

        services.AddSingleton<TDbContext>();
        return services;
    }
}
