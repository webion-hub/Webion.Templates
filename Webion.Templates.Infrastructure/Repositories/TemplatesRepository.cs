using Webion.Templates.Api.Mappings;
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

    public async Task<List<string>> GetAllAsync(CancellationToken cancellationToken) 
    {
        return await _firestore.Templates
            .StreamAsync(cancellationToken)
            .Select(t => t.ToDbo().Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<TemplateDbo> CreateAsync(TemplateDbo template)
    {
        var existing = await _firestore.Templates
            .WhereEqualTo(a => a.Name, template.Name)
            .StreamAsync()
            .FirstOrDefaultAsync();

        if(existing is not null)
            return null!;

        var created = await _firestore.Templates.AddAsync(template);
        
        return template;
    }

    public async Task<bool> DeleteAsync(string name)
    {
        var found = await _firestore.Templates
            .WhereEqualTo(a => a.Name, name)
            .StreamAsync()
            .FirstOrDefaultAsync();
        
        if(found is null)
            return false;
        
        return await found.DeleteAsync();
    }

    public async Task<TemplateDbo?> FindByNameAsync(string name, CancellationToken cancellationToken)
    {
        var found = await _firestore.Templates
            .WhereEqualTo(a => a.Name, name)
            .StreamAsync(cancellationToken)
            .FirstOrDefaultAsync(cancellationToken);

        if(found is null)
            return null;

        return found.ToDbo();
    }
}