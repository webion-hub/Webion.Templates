using Webion.Templates.Core.Data;
using Webion.Templates.Firestore.Context;
using Webion.Templates.Infrastructure.Abstractions;

namespace Webion.Templates.Infrastructure.Repositories;

internal sealed class TemplatesRepository : ITemplatesRepository
{
    private readonly TemplatesFirestoreDbContext _firestore;

    public TemplatesRepository(TemplatesFirestoreDbContext firestore)
    {
        _firestore = firestore;
    }

    public async Task<List<string>> AllAsync(CancellationToken cancellationToken) 
    {
        return await _firestore.Templates
            .StreamAsync(cancellationToken)
            .Select(t => t.GetValue(t => t.Name))
            .ToListAsync(cancellationToken);
    }

    public async Task<TemplateDbo> CreateAsync(TemplateDbo template, CancellationToken cancellationToken)
    {
        var existing = await FindByNameAsync(template.Name, cancellationToken);
        if(existing is not null)
            return null!;

        var created = await _firestore.Templates.AddAsync(template, cancellationToken);
        
        return template;
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