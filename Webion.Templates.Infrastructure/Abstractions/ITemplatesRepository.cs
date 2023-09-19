using Webion.Templates.Core.Data;

namespace Webion.Templates.Infrastructure.Abstractions;

public interface ITemplatesRepository
{
    public Task<TemplateDbo?> CreateAsync(TemplateDbo template);
    public Task<TemplateDbo?> FindByNameAsync(string name, CancellationToken cancellationToken);
    public Task<bool> DeleteAsync(string name);
    public Task<bool> UpdateAsync(string name, string value);
    public Task<List<string>> GetAllAsync(CancellationToken cancellationToken);
}