namespace Webion.Firestore.Options;

public sealed class FirestoreDbOptions<TContext>
{
    public string ProjectId { get; set; } = null!;
    public bool Emulation { get; set; } = false;
}