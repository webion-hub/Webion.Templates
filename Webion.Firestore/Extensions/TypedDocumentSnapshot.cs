using System.Linq.Expressions;
using System.Reflection;
using Google.Cloud.Firestore;

namespace Webion.Firestore.Extensions;

public class TypedDocumentSnapshot<TDoc>
{
    private readonly DocumentSnapshot _doc;
    public TypedDocumentSnapshot(DocumentSnapshot doc)
    {
        _doc = doc;
    }

    public TProperty GetValue<TProperty>(Expression<Func<TDoc, TProperty>> filter)
    {
        var name = GetPropertyName<TDoc, TProperty>(filter);
        return _doc.GetValue<TProperty>(name);
    }

    public async Task<bool> DeleteAsync()
    {
        var result = await _doc.Reference.DeleteAsync(null);
        
        return result is not null;
    }

    public async Task<bool> UpdateAsync<TProperty>(params (Expression<Func<TDoc, TProperty>> Field, TProperty Value)[] expressions)
    {
        var updateDict = new Dictionary<string, object?>();

        foreach (var e in expressions)
        {
            var name = GetPropertyName<TDoc, TProperty>(e.Field);
            updateDict.Add(name, e.Value);   
        }

        var result = await _doc.Reference.UpdateAsync(updateDict);
        
        return result is not null;
    }

    private string GetPropertyName(PropertyInfo propInfo)
    {   
        var name = propInfo.Name;
        return char.ToLower(name[0]) + name.Substring(1);
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
