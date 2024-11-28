using System.CodeDom.Compiler;

namespace Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

public abstract class EquatableInterfaceGenerator : IIdentifierPartGenerator
{
	public IEnumerable<string> GenerateAttributes(Identifier identifier)
	{
		return Array.Empty<string>();
	}

	public IEnumerable<string> GenerateInterfaces(Identifier identifier)
	{
		yield return $"global::System.IEquatable<{identifier.Name}>";
	}

	public void GenerateMembers(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine($"public bool Equals({identifier.Name} other)");
		writer.WriteBlock(() => GenerateEqualsBody(identifier, writer));
	}

	protected abstract void GenerateEqualsBody(Identifier identifier, IndentedTextWriter writer);
}

public sealed class DefaultEquatableInterfaceGenerator : EquatableInterfaceGenerator
{
	protected override void GenerateEqualsBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("return Value.Equals(other.Value);");
	}
}

public sealed class StringEquatableInterfaceGenerator : EquatableInterfaceGenerator
{
	protected override void GenerateEqualsBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("return (Value, other.Value) switch");
		writer.WriteBlock(() => {
			writer.WriteLine("(null, null) => true,");
			writer.WriteLine("(null, _) => false,");
			writer.WriteLine("(_, null) => false,");
			writer.WriteLine("(_, _) => Value.Equals(other.Value)");
		}, withSemicolon: true);
	}
}
