using Stubble.Core.Builders;
using Webion.Templates.Mustache.Abstractions;
namespace Webion.Templates.Mustache.Services;

public class TemplateProcessService : ITemplateProcessService
{
    public async Task<string> ProcessAsync(
        dynamic? req,
        string template,
        CancellationToken cancellationToken
    )
    {
        var stubble = new StubbleBuilder()
            .Configure(settings => {
                settings.SetIgnoreCaseOnKeyLookup(true);
                settings.SetMaxRecursionDepth(512);
            })
            .Build();

        return await stubble.RenderAsync(template, req);
    }
}
