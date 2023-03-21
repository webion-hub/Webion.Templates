using Webion.Templates.Api.Model;
using Webion.Templates.Core.Data;

namespace Webion.Templates.Api.Mappings;

public static class TemplateMapping
{
    public static TemplateDbo ToDbo(this TemplateModel template)
    {
        return new TemplateDbo
        {
            Name = template.Name,
            Template = template.Template,
        };
    }

    public static TemplateModel ToModel(this TemplateDbo template)
    {
        return new TemplateModel
        {
            Name = template.Name,
            Template = template.Template,
        };
    }
}