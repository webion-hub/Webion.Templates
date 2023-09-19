namespace Webion.Templates.Mustache.Abstractions;

public interface ITemplateProcessService
{
    public Task<string> ProcessAsync(
        dynamic? req,
        string template,
        CancellationToken cancellationToken
    );
}
