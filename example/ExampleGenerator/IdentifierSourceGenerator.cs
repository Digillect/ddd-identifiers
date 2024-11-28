using Digillect.DDD.Identifiers.SourceGenerator;
using Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;
using Microsoft.CodeAnalysis;

namespace ExampleGenerator;

[Generator]
public class IdentifierSourceGenerator : IdentifierSourceGeneratorBase
{
	public IdentifierSourceGenerator()
	{
		Builder
			// Enable Guid identifiers to be marked with the custom attribute
			.AddGuidV7Type(t => t
				.AddDetector(new CustomAttributeIdentifierDetector("ExampleIdentifiers.IdentifierAttribute"))
				.AddGenerator<GuidBsonSerializerGenerator>())
			.AddIntegerType(t => t.AddGenerator<IntegerBsonSerializerGenerator>())
			.AddStringType(t => t.AddGenerator<StringBsonSerializerGenerator>())
			.AddObjectIdType();
	}
}
