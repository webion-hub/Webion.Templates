using Google.Cloud.Firestore;
using Microsoft.Extensions.Options;
using Webion.Templates.Core.Data;
using Webion.Firestore.Abstractions;
using Webion.Firestore.Extensions;
using Webion.Firestore.Options;

namespace Webion.Templates.Firestore.Context;

public sealed class TemplatesFirestoreDbContext : FirestoreDbContext<TemplatesFirestoreDbContext>
{
    public TemplatesFirestoreDbContext(IOptions<FirestoreDbOptions<TemplatesFirestoreDbContext>> options)
        : base(options)
    {}
    
    public FirestoreDb DbContext => Db;
    public TypedCollectionReference<TemplateDbo> Templates => Db.Collection("templates").Of<TemplateDbo>();
}