﻿using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using SomeSandwich.Donut.Abstractions.OpenApi;
using SomeSandwich.Donut.Abstractions.OpenApi.Extensions;

namespace SomeSandwich.Donut.Application.Common.Startup.OpenApi.Examples;

/// <summary>
/// A class representing a processor that can add examples to OpenAPI operations and schemas.
/// </summary>
public abstract class ExamplesProcessor
{
    private static readonly OpenApiJsonSerializerContext Context = OpenApiJsonSerializerContext.Default;

    /// <summary>
    /// Processes the OpenAPI operation to add examples to parameters, request bodies, and responses.
    /// </summary>
    /// <param name="operation">The OpenAPI operation to process.</param>
    /// <param name="description">The API description of the operation.</param>
    protected void Process(OpenApiOperation operation, ApiDescription description)
    {
        // Get all the examples that may apply to the operation through attributes
        // configured globally, on an API group, or on a specific endpoint.
        var examples = description.ActionDescriptor.EndpointMetadata
            .OfType<IOpenApiExampleMetadata>()
            .ToArray();

        if (operation.Parameters is { Count: > 0 } parameters)
        {
            TryAddParameterExamples(parameters, description, examples);
        }

        if (operation.RequestBody is { } body)
        {
            TryAddRequestExamples(body, description, examples);
        }

        TryAddResponseExamples(operation.Responses, description, examples);
    }

    /// <summary>
    /// Processes the OpenAPI schema to add examples.
    /// </summary>
    /// <param name="schema">The OpenAPI schema to process.</param>
    /// <param name="type">The type associated with the schema.</param>
    protected void Process(OpenApiSchema schema, Type type)
    {
        if (schema.Example is not null)
        {
            return;
        }

        // We cannot change ProblemDetails directly, so we need to adjust it if we see it
        if (type == typeof(ProblemDetails))
        {
            schema.Example = ExampleFormatter.AsJson<ProblemDetails, ProblemDetailsExampleProvider>(Context);
        }
        else if (type.GetExampleMetadata().FirstOrDefault() is { } metadata)
        {
            schema.Example = metadata.GenerateExample(Context);
        }
    }

    private static void TryAddParameterExamples(
        IList<OpenApiParameter> parameters,
        ApiDescription description,
        IList<IOpenApiExampleMetadata> examples)
    {
        // Find the method associated with the operation and get its arguments
        var arguments = description.ActionDescriptor.EndpointMetadata
            .OfType<MethodInfo>()
            .FirstOrDefault()?
            .GetParameters()
            .ToArray();

        if (arguments is { Length: > 0 })
        {
            foreach (var argument in arguments)
            {
                // Find the example for the argument either as a parameter attribute,
                // an attribute on the parameter's type, or metadata from the endpoint.
                var metadata =
                    argument.GetExampleMetadata().FirstOrDefault((p) => p.SchemaType == argument.ParameterType) ??
                    argument.ParameterType.GetExampleMetadata().FirstOrDefault((p) => p.SchemaType == argument.ParameterType) ??
                    examples.FirstOrDefault((p) => p.SchemaType == argument.ParameterType);

                if (metadata?.GenerateExample(Context) is { } value)
                {
                    // Find the parameter that corresponds to the argument and set its example
                    var parameter = parameters.FirstOrDefault((p) => p.Name == argument.Name);
                    if (parameter is not null)
                    {
                        parameter.Example ??= value;
                    }
                }
            }
        }
    }

    private static void TryAddRequestExamples(
        OpenApiRequestBody body,
        ApiDescription description,
        IList<IOpenApiExampleMetadata> examples)
    {
        if (!body.Content.TryGetValue("application/json", out var mediaType) || mediaType.Example is not null)
        {
            return;
        }

        var bodyParameter = description.ParameterDescriptions.Single((p) => p.Source == BindingSource.Body);

        var metadata = description.ParameterDescriptions
            .Single((p) => p.Source == BindingSource.Body)
            .Type
            .GetExampleMetadata()
            .FirstOrDefault();

        metadata ??= examples.FirstOrDefault((p) => p.SchemaType == bodyParameter.Type);

        if (metadata is not null)
        {
            mediaType.Example ??= metadata.GenerateExample(Context);
        }
    }

    private static void TryAddResponseExamples(
        OpenApiResponses responses,
        ApiDescription description,
        IList<IOpenApiExampleMetadata> examples)
    {
        foreach (var schemaResponse in description.SupportedResponseTypes)
        {
            schemaResponse.Type ??= schemaResponse.ModelMetadata?.ModelType;

            var metadata = schemaResponse.Type?
                .GetExampleMetadata()
                .FirstOrDefault((p) => p.SchemaType == schemaResponse.Type);

            foreach (var responseFormat in schemaResponse.ApiResponseFormats)
            {
                if (responses.TryGetValue(schemaResponse.StatusCode.ToString(CultureInfo.InvariantCulture), out var response) &&
                    response.Content.TryGetValue(responseFormat.MediaType, out var mediaType))
                {
                    mediaType.Example ??= (metadata ?? examples.SingleOrDefault((p) => p.SchemaType == schemaResponse.Type))?.GenerateExample(Context);
                }
            }
        }
    }
}
