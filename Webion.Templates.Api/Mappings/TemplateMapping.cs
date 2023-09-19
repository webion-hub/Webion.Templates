using Webion.Firestore.Extensions;
using Webion.Templates.Api.Model;
using Webion.Templates.Core.Data;

namespace Webion.Templates.Api.Mappings;

public static class TemplateMapping
{
    public static TemplateDbo ToDbo(this TemplateModel template) => new()
    {
        Name = template.Name,
        Template = template.Template,
    };

    public static TemplateModel ToModel(this TemplateDbo template) => new()
    {
        Name = template.Name,
        Template = template.Template,
    };

    public static TemplateDbo ToDbo(this TypedDocumentSnapshot<TemplateDbo> doc) => new()
    {
        Name = doc.GetValue(d => d.Name),
        Template = doc.GetValue(d => d.Template)
    };
}