using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Digillect.DDD.Identifiers.SourceGenerator;

public sealed class AttributeBasedIdentifierSelector : IdentifierSelector
{
	private static readonly SymbolDisplayFormat FullTypeNameFormat = new(
		SymbolDisplayGlobalNamespaceStyle.Included,
		SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);

	public override IncrementalValueProvider<ImmutableArray<Identifier>>? CreateValueProvider(
		IncrementalGeneratorInitializationContext context,
		IReadOnlyCollection<IdentifierType> identifierTypes)
	{
		var attributesAndTypes =
			from type in identifierTypes
			from detector in type.Detectors.OfType<AttributeBasedIdentifierDetector>()
			group type by detector.AttributeTypeName
			into g
			select new { AttributeTypeName = g.Key, IdentifierTypes = g.ToImmutableArray() };

		IncrementalValueProvider<ImmutableArray<Identifier>>? result = null;

		foreach (var attributeAndType in attributesAndTypes)
		{
			var provider = context.SyntaxProvider
				.ForAttributeWithMetadataName(
					attributeAndType.AttributeTypeName,
					static (node, _) => node is StructDeclarationSyntax,
					(ctx, ct) => InspectAttributeBasedIdentifier(attributeAndType.IdentifierTypes, ctx, attributeAndType.AttributeTypeName, ct))
				.Where(identifier => identifier.IsValid)
				.Collect();

			result = result?.Concat(provider) ?? provider;
		}

		return result;
	}

	private static Identifier InspectAttributeBasedIdentifier(
		IReadOnlyCollection<IdentifierType> identifierTypes,
		GeneratorAttributeSyntaxContext ctx,
		string attributeTypeName,
		CancellationToken cancellationToken)
	{
		StructDeclarationSyntax structDeclarationSyntax = (StructDeclarationSyntax) ctx.TargetNode;
		ISymbol? typeSymbol = ModelExtensions.GetDeclaredSymbol(ctx.SemanticModel, structDeclarationSyntax, cancellationToken);

		if (typeSymbol is not INamedTypeSymbol identifierTypeSymbol)
		{
			return Identifier.Invalid;
		}

		if (!identifierTypeSymbol.IsReadOnly || !structDeclarationSyntax.Modifiers.Any(SyntaxKind.PartialKeyword))
		{
			return Identifier.Invalid;
		}

		foreach (var identifierType in identifierTypes)
		{
			bool isValid = identifierType.Detectors
				.OfType<AttributeBasedIdentifierDetector>()
				.Where(detector => string.Equals(detector.AttributeTypeName, attributeTypeName))
				.Any(detector => ctx.Attributes.Any(detector.IsValidAttribute));

			if (isValid)
			{
				return Identifier.Create(
					identifierType,
					identifierTypeSymbol.ContainingNamespace.ToDisplayString(),
					identifierTypeSymbol.Name,
					identifierTypeSymbol.ToDisplayString(FullTypeNameFormat));
			}
		}

		return Identifier.Invalid;
	}
}
