using Webion.Templates.Core.Data;

namespace Webion.Templates.Infrastructure.Abstractions;

public interface ITemplatesRepository
{
    public Task<TemplateDbo> CreateAsync(TemplateDbo template, CancellationToken cancellationToken);
    public Task<TemplateDbo?> FindByNameAsync(string name, CancellationToken cancellationToken);
    public Task<bool> DeleteAsync(string name, CancellationToken cancellationToken);
    public Task<List<string>> AllAsync(CancellationToken cancellationToken);
}