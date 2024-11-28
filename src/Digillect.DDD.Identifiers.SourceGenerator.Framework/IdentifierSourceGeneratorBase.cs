using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Digillect.DDD.Identifiers.SourceGenerator;

/// <summary>
/// Генератор методов и конвертеров для типов-идентификаторов.
/// </summary>
public abstract class IdentifierSourceGeneratorBase : IIncrementalGenerator
{
	protected IdentifierSourceGeneratorBuilder Builder { get; } = new();

    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		var identifierTypes = Builder.IdentifierTypes;

		IncrementalValueProvider<ImmutableArray<Identifier>>? result = null;

		foreach (var selector in Builder.Selectors)
		{
			var provider = selector.CreateValueProvider(context, identifierTypes);

			if (provider is not null)
			{
				result = result?.Concat(provider.Value) ?? provider;
			}
		}

		if (result is not null)
		{
			context.RegisterSourceOutput(result.Value, GenerateCode);
		}
    }

	private static void GenerateCode(SourceProductionContext context, ImmutableArray<Identifier> identifiers)
    {
        foreach (var identifier in identifiers.OfType<Identifier>())
		{
			var codeGenerator = new CodeGenerator(identifier);

            string code = codeGenerator.Emit();

            // Add the source code to the compilation
            context.AddSource($"{identifier.Namespace}.{identifier.Name}.g.cs", SourceText.From(code, Encoding.UTF8));
        }
    }
}
