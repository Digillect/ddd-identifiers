using Microsoft.CodeAnalysis;

namespace Digillect.DDD.Identifiers.SourceGenerator;

public sealed class CustomAttributeIdentifierDetector(string attributeTypeName) : AttributeBasedIdentifierDetector
{
	public override string AttributeTypeName => attributeTypeName;
	public override bool IsValidAttribute(AttributeData attribute)
	{
		return string.Equals(attributeTypeName, attribute.AttributeClass?.ToString());
	}
}
