using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Namotion.Reflection;
using System;
using System.Linq;


namespace EFCommenter;
public static partial class ModelBuilderExtentionMethods
{
    /// <summary>
    /// All summaries of entities, properties, and enums will be added as comments on the corresponding database tables and columns.
    /// </summary>
    /// <remarks>
    /// <b>Note:</b>
    /// The XML documentation file option must be enabled in your <c>.csproj</c> file.
    /// <para>
    /// That means you need to add this to your project file:
    /// </para>
    /// <code>
    /// &lt;PropertyGroup&gt;
    ///     &lt;GenerateDocumentationFile&gt;true&lt;/GenerateDocumentationFile&gt;
    /// &lt;/PropertyGroup&gt;
    /// </code>
    /// </remarks>

    public static void AddEntitiesComments(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(x => x.BaseType == null))
        {
            //set table description
            var entityComment = entityType.ClrType.GetXmlDocsSummary();
            if (!string.IsNullOrEmpty(entityComment))
                entityType.SetComment(entityComment);

            //set description of property 
            foreach (var property in entityType.GetProperties())
            {
                var underlyingType = Nullable.GetUnderlyingType(property.ClrType) ?? property.ClrType;
                var comment = underlyingType.IsEnum ? GetEnumComment(underlyingType) : GetPropertyComment(entityType, property);
                if (!string.IsNullOrEmpty(comment))
                    property.SetComment(comment);
            }

            //set description of property for driverd entities
            var derivedTypes = entityType.GetDerivedTypes();
            if (derivedTypes != null && derivedTypes.Any())
            {

                //set table description
                entityComment = string.IsNullOrEmpty(entityComment) ? entityType.ShortName() : $"{entityType.ShortName()} | {entityComment}";
                var derivedTypesComments = derivedTypes.Select(t =>
                        {
                            string entityComment = t.ClrType.GetXmlDocsSummary();
                            entityComment = string.IsNullOrEmpty(entityComment) ? null : $" | {entityComment} ";
                            return t.ShortName() + entityComment;
                        }
                    );
                entityComment += ":\n   " + string.Join("\n   ", derivedTypesComments);
                if (!string.IsNullOrEmpty(entityComment))
                    entityType.SetComment(entityComment);

                //set properties descriptions
                var pGroups = derivedTypes.SelectMany(x => x.GetProperties().Select(p => new { entityType = x, property = p }))
                    .GroupBy(x => x.property.Name);
                foreach (var g in pGroups)
                {
                    string enumComment = null;
                    string entitiesComment = null;

                    //enum comment
                    if (g.FirstOrDefault()!.property.ClrType.IsEnum)
                    {
                        var underlyingType = Nullable.GetUnderlyingType(g.First().property.ClrType) ?? g.First().property.ClrType;
                        enumComment = GetEnumComment(underlyingType);
                        enumComment = "Values: \n" + enumComment;
                    }

                    //Entities comment
                    var entitiesComments = g.Select(p =>
                    {
                        string propertyComment = GetPropertyComment(p.entityType, p.property);
                        propertyComment = string.IsNullOrEmpty(propertyComment) ? null : $" | {propertyComment}";
                        string entityComment = p.entityType.ClrType.GetXmlDocsSummary();
                        entityComment = string.IsNullOrEmpty(entityComment) ? null : $" | {entityComment} ";
                        return p.entityType.ShortName() + propertyComment + entityComment;
                    });
                    entitiesComment = string.Join("\n \n", entitiesComments.Where(x => !string.IsNullOrEmpty(x)));
                    if (entitiesComment != null)
                        entitiesComment = "Entities: \n" + entitiesComment + "\n \n \n";

                    string comment = entitiesComment + enumComment;
                    if (!string.IsNullOrEmpty(comment))
                        foreach (var p in g)
                            p.property.SetComment(comment);
                }
            }
        }

    }

    private static string GetPropertyComment(IMutableEntityType entityType, IMutableProperty property)
    {
        var memberInfo = entityType.ClrType.GetMember(property.Name).FirstOrDefault();
        if (memberInfo != null)
        {
            var comment = memberInfo.GetXmlDocsSummary();
            return comment;
        }
        return null;
    }

    private static string GetEnumComment(Type enumType)
    {
        return string.Join("\n", Enum.GetValues(enumType).Cast<Enum>()
            .Select(e => $"{Convert.ToInt32(e)}: {e} | {enumType.GetXmlSummary(e.ToString())}"));
    }
}

