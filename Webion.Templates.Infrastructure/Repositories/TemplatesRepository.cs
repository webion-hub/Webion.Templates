using System.Security.Cryptography;
using System.Text;
using Webion.Templates.Core.Data;
using Webion.Templates.Firestore.Context;
using Webion.Templates.Infrastructure.Abstractions;
using Webion.Firestore.Extensions;

namespace Webion.Templates.Infrastructure.Repositories;

internal sealed class TemplatesRepository : ITemplatesRepository
{
    private readonly TemplatesFirestoreDbContext _firestore;

    public TemplatesRepository(TemplatesFirestoreDbContext firestore)
    {
        _firestore = firestore;
    }

    public async Task<TemplateDbo?> FindByNameAsync(string name, CancellationToken cancellationToken)
    {
        var found = await _firestore.Templates
            .WhereEqualTo(a => a.Name, name)
            .StreamAsync(cancellationToken)
            .FirstOrDefaultAsync(cancellationToken) ?? null;

        if(found is null)
            return null;

        return new TemplateDbo 
        {
            Name = found.GetValue(t => t.Name),
            Template = found.GetValue(t => t.Template)
        };
    }
}