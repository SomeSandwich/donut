﻿using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json.Serialization.Metadata;
using System.Xml;
using System.Xml.XPath;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace SomeSandwich.Donut.Application.Common.Startup.OpenApi.Descriptions;

/// <summary>
/// An OpenAPI schema transformer that adds descriptions from XML documentation.
/// </summary>
/// <remarks>
/// https://github.com/martincostello/aspnetcore-openapi/blob/main/src/TodoApp/OpenApi/AspNetCore/AddSchemaDescriptionsTransformer.cs.
/// </remarks>
public class AddSchemaDescriptionsTransformer : IOpenApiSchemaTransformer
{
    private static readonly Assembly[] Assemblies = [Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly(), typeof(AddSchemaDescriptionsTransformer).Assembly];
    private readonly ConcurrentDictionary<string, string?> descriptions = [];
    private readonly ConcurrentDictionary<Assembly, XPathNavigator> navigators = [];

    /// <inheritdoc />
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        // Assign a description from the XML documentation from either the type or the property associated with the schema
        if (schema.Description is null &&
            GetMemberName(context.JsonTypeInfo, context.JsonPropertyInfo) is { Length: > 0 } memberName &&
            GetDescription(context.JsonPropertyInfo?.DeclaringType.Assembly ?? context.JsonTypeInfo.Type.Assembly, memberName) is { Length: > 0 } description)
        {
            schema.Description = description;
        }

        return Task.CompletedTask;
    }

    private string? GetDescription(Assembly assembly, string memberName)
    {
        if (descriptions.TryGetValue(memberName, out var description))
        {
            return description;
        }

        // Try to find the summary text for the member from the XML documentation file
        var navigator = CreateNavigator(assembly);
        var node = navigator.SelectSingleNode($"/doc/members/member[@name='{memberName}']/summary");

        if (node is not null)
        {
            description = node.Value.Trim();
        }

        // Cache the description for this member
        descriptions[memberName] = description;

        return description;
    }

    private static string? GetMemberName(JsonTypeInfo typeInfo, JsonPropertyInfo? propertyInfo)
    {
        if (!Assemblies.Contains(typeInfo.Type.Assembly) &&
            !Assemblies.Contains(propertyInfo?.DeclaringType.Assembly))
        {
            // The type or member's type is not from this assembly (e.g. from the framework itself)
            return null;
        }
        else if (propertyInfo is not null)
        {
            // We need to get the summary for the property (or field)
            var typeName = propertyInfo.DeclaringType.FullName;
            var memberName =
                propertyInfo.AttributeProvider is MemberInfo member ?
                member.Name :
                $"{char.ToUpperInvariant(propertyInfo.Name[0])}{propertyInfo.Name[1..]}";

            // Is the member a property or a field?
            var memberType = propertyInfo.AttributeProvider is PropertyInfo ? "P" : "F";

            return $"{memberType}:{typeName}{Type.Delimiter}{memberName}";
        }
        else
        {
            // We need to get the summary for the type itself
            return $"T:{typeInfo.Type.FullName}";
        }
    }

    private XPathNavigator CreateNavigator(Assembly assembly)
    {
        if (navigators.TryGetValue(assembly, out var navigator))
        {
            return navigator;
        }

        // Find the .xml documentation file associated with this assembly.
        // It should be in the application's directory next to the .dll file.
        var path = Path.Combine(AppContext.BaseDirectory, $"{assembly.GetName().Name}.xml");
        using var reader = XmlReader.Create(path);
        navigator = new XPathDocument(reader).CreateNavigator();

        navigators[assembly] = navigator;

        return navigator;
    }
}
