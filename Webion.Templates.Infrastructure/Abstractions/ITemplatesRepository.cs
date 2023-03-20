using Webion.Firestore.Extensions;
using Webion.Templates.Core.Data;

namespace Webion.Templates.Infrastructure.Abstractions;

public interface ITemplatesRepository
{
    Task<TemplateDbo?> FindByNameAsync(string name, CancellationToken cancellationToken);
    // Task<string> ProcessAsync(string name, CancellationToken cancellationToken);
}