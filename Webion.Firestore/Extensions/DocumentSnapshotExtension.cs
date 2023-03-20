using Webion.Firestore.Extensions;

namespace Google.Cloud.Firestore;

public static class DocumentSnapshotExtension
{
    public static TypedDocumentSnapshot<TDoc> Of<TDoc>(this DocumentSnapshot doc)
    {
        return new TypedDocumentSnapshot<TDoc>(doc);
    }
}