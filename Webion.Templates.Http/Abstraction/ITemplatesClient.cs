using Webion.Templates.Http.Model;

namespace Webion.Templates.Http.Abstraction;

public interface ITemplatesClient
{
    public Task<IEnumerable<string>> GetAllAsync(CancellationToken cancellationToken);
    public Task<TemplateModel?> FindByNameAsync(string name, CancellationToken cancellationToken);
    public Task<bool> CreateAsync(TemplateModel template);
    public Task<bool> RemoveAsync(string name);
    public Task<bool> UpdateAsync(string name, string value);
}