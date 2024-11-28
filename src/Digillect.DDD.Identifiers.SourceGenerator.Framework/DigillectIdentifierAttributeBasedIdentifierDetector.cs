using Microsoft.CodeAnalysis;

namespace Digillect.DDD.Identifiers.SourceGenerator;

public class DigillectIdentifierAttributeBasedIdentifierDetector(string valueTypeName) : AttributeBasedIdentifierDetector
{
	private static readonly SymbolDisplayFormat TypeArgumentFormat = new(
		SymbolDisplayGlobalNamespaceStyle.Included,
		SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);

	private const string IdentifierAttributeNamespaceName = "Digillect.DDD.Identifiers";
	private const string IdentifierAttributeMetadataName = "IdentifierAttribute`1";
	private const string IdentifierAttributeTypeName = $"{IdentifierAttributeNamespaceName}.{IdentifierAttributeMetadataName}";

	public override string AttributeTypeName => IdentifierAttributeTypeName;

	public override bool IsValidAttribute(AttributeData attribute)
	{
		var attributeClass = attribute.AttributeClass;

		if (attributeClass is null || !attributeClass.IsGenericType)
		{
			return false;
		}

		var constructedFrom = attributeClass.ConstructedFrom;

		string namespaceName = constructedFrom.ContainingNamespace.ToDisplayString();
		string metadataName = constructedFrom.MetadataName;

		if (!string.Equals(IdentifierAttributeNamespaceName, namespaceName) || !string.Equals(IdentifierAttributeMetadataName, metadataName))
		{
			return false;
		}

		if (attributeClass.TypeArguments.Length != 1)
		{
			return false;
		}

		string typeArgumentName = attributeClass.TypeArguments[0].ToDisplayString(TypeArgumentFormat);

		bool result = string.Equals(valueTypeName, typeArgumentName);

		return result;
	}
}
