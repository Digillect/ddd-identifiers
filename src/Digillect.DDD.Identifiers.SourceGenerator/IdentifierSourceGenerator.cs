using Microsoft.CodeAnalysis;

namespace Digillect.DDD.Identifiers.SourceGenerator;

[Generator]
public class IdentifierSourceGenerator : IdentifierSourceGeneratorBase
{
	public IdentifierSourceGenerator()
	{
		Builder
			.AddGuidType()
			.AddIntegerType()
			.AddStringType();
	}
}
