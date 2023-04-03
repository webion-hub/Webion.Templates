using System.Linq.Expressions;
using System.Reflection;
using Google.Cloud.Firestore;

namespace Webion.Firestore.Extensions;

public class TypedCollectionReference<TDoc>
{
    private readonly CollectionReference _coll;
    private readonly Query _query;
    public TypedCollectionReference(Query query, CollectionReference coll)
    {
        _query = query;
        _coll = coll;
    }

    public TypedCollectionReference<TDoc> WhereEqualTo<TProperty>(Expression<Func<TDoc, TProperty>> filter, object value)
    {
        var name = GetPropertyName<TDoc, TProperty>(filter);
        return new(_query.WhereEqualTo(name, value), _coll);
    }
    public TypedCollectionReference<TDoc> WhereGreaterThanOrEqualTo<TProperty>(Expression<Func<TDoc, TProperty>> filter, object value)
    {
        var name = GetPropertyName<TDoc, TProperty>(filter);
        return new (_query.WhereGreaterThanOrEqualTo(name, value), _coll);
    }

    public TypedCollectionReference<TDoc> Select<TProperty>(Expression<Func<TDoc, TProperty>> filter)
    {
        var name = GetPropertyName<TDoc, TProperty>(filter);
        return new (_query.Select(name), _coll);
    }

    public async Task<TypedDocumentSnapshot<TDoc>> AddAsync(TDoc doc)
    {
        var target = await _coll.AddAsync(doc);
        var snap = await target.GetSnapshotAsync();
        return snap.Of<TDoc>();
    }

    public IAsyncEnumerable<TypedDocumentSnapshot<TDoc>> StreamAsync(CancellationToken cancellationToken)
    {
        return _query
            .StreamAsync(cancellationToken)
            .Select(d => d.Of<TDoc>());
    }

    public IAsyncEnumerable<TypedDocumentSnapshot<TDoc>> StreamAsync()
    {
        return _query
            .StreamAsync()
            .Select(d => d.Of<TDoc>());
    }

    private string GetPropertyName<TSource, TProperty>(
        Expression<Func<TSource, TProperty>> propertyLambda
    )
    {
        Type type = typeof(TSource);

        MemberExpression? member = propertyLambda.Body as MemberExpression;
        if (member == null)
            throw new ArgumentException(string.Format(
                "Expression '{0}' refers to a method, not a property.",
                propertyLambda.ToString()));

        PropertyInfo? propInfo = member.Member as PropertyInfo;
        if (propInfo == null)
            throw new ArgumentException(string.Format(
                "Expression '{0}' refers to a field, not a property.",
                propertyLambda.ToString()));

        if (type != propInfo.ReflectedType &&
            !type.IsSubclassOf(propInfo.ReflectedType!))
            throw new ArgumentException(string.Format(
                "Expression '{0}' refers to a property that is not from type {1}.",
                propertyLambda.ToString(),
                type));

        var name = propInfo.Name;
        return char.ToLower(name[0]) + name.Substring(1);
    }
}
