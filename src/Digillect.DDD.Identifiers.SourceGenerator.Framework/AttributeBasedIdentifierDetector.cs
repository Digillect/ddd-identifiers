using Microsoft.CodeAnalysis;

namespace Digillect.DDD.Identifiers.SourceGenerator;

public abstract class AttributeBasedIdentifierDetector : IdentifierDetector
{
	public abstract string AttributeTypeName { get; }
	public abstract bool IsValidAttribute(AttributeData attribute);
}
