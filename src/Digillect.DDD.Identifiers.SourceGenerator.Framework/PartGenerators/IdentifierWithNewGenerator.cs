using System.CodeDom.Compiler;

namespace Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

public abstract class IdentifierWithNewGenerator : IIdentifierPartGenerator
{
	public IEnumerable<string> GenerateAttributes(Identifier identifier)
	{
		return Array.Empty<string>();
	}

	public IEnumerable<string> GenerateInterfaces(Identifier identifier)
	{
		yield return $"global::Digillect.DDD.Identifiers.IIdentifierWithNew<{identifier.Name}>";
	}

	public virtual void GenerateMembers(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine($"public static {identifier.Name} New() => new {identifier.Name}({ConstructorParameter});");
	}

	protected abstract string ConstructorParameter { get; }
}

public sealed class GuidIdentifierWithNewGenerator : IdentifierWithNewGenerator
{
	protected override string ConstructorParameter => "global::System.Guid.NewGuid()";
}

public sealed class GuidV7IdentifierWithNewGenerator : IdentifierWithNewGenerator
{
	protected override string ConstructorParameter => "global::System.Guid.CreateVersion7()";

	public override void GenerateMembers(Identifier identifier, IndentedTextWriter writer)
	{
		base.GenerateMembers(identifier, writer);

		writer.WriteLine($"public static {identifier.Name} New(global::System.DateTimeOffset timestamp) => new {identifier.Name}(global::System.Guid.CreateVersion7(timestamp));");
	}
}
