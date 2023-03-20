using Google.Cloud.Firestore;

namespace Webion.Templates.Core.Data;

[FirestoreData]
public class TemplateDbo
{
    [FirestoreProperty("name")]
    public string Name { get; set; } = null!;

    [FirestoreProperty("template")]
    public string Template { get; set; } = null!;
}