using Webion.Firestore.Extensions;

namespace Google.Cloud.Firestore;

public static class CollectionReferenceExtension
{
    public static TypedCollectionReference<TDoc> Of<TDoc>(this CollectionReference coll)
    {
        return new TypedCollectionReference<TDoc>(coll, coll);
    }
}