using Google.Cloud.Firestore;
using Microsoft.Extensions.Options;
using Webion.Firestore.Options;

namespace Webion.Firestore.Abstractions;

public abstract class FirestoreDbContext<TContext>
{
    protected FirestoreDb Db { get; init; }

    public FirestoreDbContext(IOptions<FirestoreDbOptions<TContext>> options)
    {
        if(options.Value.Emulation)
        {
            Environment.SetEnvironmentVariable("FIRESTORE_EMULATOR_HOST", "localhost:8080");
            Db = new FirestoreDbBuilder
                {
                    ProjectId = options.Value.ProjectId,
                    EmulatorDetection = Google.Api.Gax.EmulatorDetection.EmulatorOnly,
                }
                .Build();
        }
        else 
            Db = FirestoreDb.Create(options.Value.ProjectId);
    }
}