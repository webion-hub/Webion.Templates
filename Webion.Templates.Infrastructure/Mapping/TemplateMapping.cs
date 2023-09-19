using Webion.Firestore.Extensions;
using Webion.Templates.Core.Data;

namespace Webion.Templates.Api.Mappings;

public static class TemplateMapping
{
    public static TemplateDbo ToDbo(this TypedDocumentSnapshot<TemplateDbo> doc) => new()
    {
        Name = doc.GetValue(d => d.Name),
        Template = doc.GetValue(d => d.Template)
    };
}