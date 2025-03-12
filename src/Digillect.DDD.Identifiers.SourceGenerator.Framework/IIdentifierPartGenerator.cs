using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Digillect.DDD.Identifiers.SourceGenerator;

public interface IIdentifierPartGenerator
{
	IEnumerable<string> GenerateAttributes(Identifier identifier);
	IEnumerable<string> GenerateInterfaces(Identifier identifier);

	void GenerateMembers(Identifier identifier, IndentedTextWriter writer);
}

public interface IAdditionalSourceInformationProvider
{
	IEnumerable<Type> GetAdditionalSourceGenerators(Identifier identifier);
}

public sealed class AdditionalSourceGenerator(Type generatorType, object? additionalInformation = null)
{
	public Type GeneratorType { get; } = generatorType;
	public object? AdditionalInformation { get; } = additionalInformation;
}
