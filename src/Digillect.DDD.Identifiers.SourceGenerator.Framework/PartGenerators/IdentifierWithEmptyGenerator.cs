using System.CodeDom.Compiler;

namespace Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

public abstract class IdentifierWithEmptyGenerator : IIdentifierPartGenerator
{
	public IEnumerable<string> GenerateAttributes(Identifier identifier)
	{
		return Array.Empty<string>();
	}

	public IEnumerable<string> GenerateInterfaces(Identifier identifier)
	{
		yield return $"global::Digillect.DDD.Identifiers.IIdentifierWithEmpty<{identifier.Name}>";
	}

	public virtual void GenerateMembers(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine($"public static {identifier.Name} Empty {{ get; }} = new {identifier.Name}({ConstructorParameter});");
	}

	protected abstract string ConstructorParameter { get; }
}

public sealed class GuidIdentifierWithEmptyGenerator : IdentifierWithEmptyGenerator
{
	protected override string ConstructorParameter => "global::System.Guid.Empty";
}

public sealed class IntegerIdentifierWithEmptyGenerator : IdentifierWithEmptyGenerator
{
	protected override string ConstructorParameter => "0";
}

public sealed class StringIdentifierWithEmptyGenerator : IdentifierWithEmptyGenerator
{
	protected override string ConstructorParameter => "string.Empty";
}
