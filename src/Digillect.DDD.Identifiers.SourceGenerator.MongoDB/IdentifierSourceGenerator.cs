using Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;
using Microsoft.CodeAnalysis;

namespace Digillect.DDD.Identifiers.SourceGenerator.MongoDB;

[Generator]
public class IdentifierSourceGenerator : IdentifierSourceGeneratorBase
{
	public IdentifierSourceGenerator()
	{
		Builder
			.AddGuidType(t => t.AddGenerator<GuidBsonSerializerGenerator>())
			.AddIntegerType(t => t.AddGenerator<IntegerBsonSerializerGenerator>())
			.AddStringType(t => t.AddGenerator<StringBsonSerializerGenerator>())
			.AddObjectIdType();
	}
}
