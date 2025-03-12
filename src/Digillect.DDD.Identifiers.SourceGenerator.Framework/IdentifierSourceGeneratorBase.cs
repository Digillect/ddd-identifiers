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
		GenerateIdentifierSources(context, identifiers);
		GenerateAdditionalSources(context, identifiers);
	}

	private static void GenerateIdentifierSources(SourceProductionContext context, ImmutableArray<Identifier> identifiers)
    {
        foreach (var identifier in identifiers.OfType<Identifier>())
		{
			var codeGenerator = new IdentifierCodeGenerator(identifier);

            var generated = codeGenerator.Generate();

            context.AddSource(generated.FileName, SourceText.From(generated.SourceCode, Encoding.UTF8));
        }
    }

	private static void GenerateAdditionalSources(SourceProductionContext context, ImmutableArray<Identifier> identifiers)
	{
		var groupedGenerators =
			from identifier in identifiers.OfType<Identifier>()
			from additionalSourceGeneratorsProvider in identifier.Type.Generators.OfType<IAdditionalSourceInformationProvider>()
			from generatorType in additionalSourceGeneratorsProvider.GetAdditionalSourceGenerators(identifier)
			let info = new { GeneratorType = generatorType, Identifier = identifier }
			group info by info.GeneratorType
			into g
			select g;

		foreach (var groupedGenerator in groupedGenerators)
		{
			var generatorIdentifiers = groupedGenerator.Select(g => g.Identifier).ToImmutableArray();

			if (Activator.CreateInstance(groupedGenerator.Key, generatorIdentifiers)! is not IAdditionalSourceCodeGenerator codeGenerator)
			{
				continue;
			}

			var generated = codeGenerator.Generate();

			context.AddSource(generated.FileName, SourceText.From(generated.SourceCode, Encoding.UTF8));
		}
	}
}
