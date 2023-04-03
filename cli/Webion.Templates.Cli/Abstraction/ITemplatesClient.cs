using Webion.Templates.Cli.Model;

namespace Webion.Templates.Cli.Abstraction;

public interface ITemplatesClient
{
    public Task<IEnumerable<string>> GetAllAsync(CancellationToken cancellationToken);
    public Task<TemplateModel?> FindByNameAsync(string name, CancellationToken cancellationToken);
    public Task<bool> CreateAsync(TemplateModel template, CancellationToken cancellationToken);
    public Task<bool> RemoveAsync(string name, CancellationToken cancellationToken);
}