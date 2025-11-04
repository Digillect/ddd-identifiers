using System.CodeDom.Compiler;

namespace Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

public abstract class ComparableInterfaceGenerator : IIdentifierPartGenerator
{
	public IEnumerable<string> GenerateAttributes(Identifier identifier)
	{
		return Array.Empty<string>();
	}

	public IEnumerable<string> GenerateInterfaces(Identifier identifier)
	{
		yield return $"global::System.IComparable<{identifier.Name}>";
	}

	public void GenerateMembers(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine($"public int CompareTo({identifier.Name} other)");
		writer.WriteBlock(() => GenerateCompareToBody(identifier, writer));
		writer.WriteEmptyLine();
		writer.WriteLine($"public static bool operator <(in {identifier.Name} left, in {identifier.Name} right) => left.CompareTo(right) < 0;");
		writer.WriteLine($"public static bool operator <=(in {identifier.Name} left, in {identifier.Name} right) => left.CompareTo(right) <= 0;");
		writer.WriteLine($"public static bool operator >(in {identifier.Name} left, in {identifier.Name} right) => left.CompareTo(right) > 0;");
		writer.WriteLine($"public static bool operator >=(in {identifier.Name} left, in {identifier.Name} right) => left.CompareTo(right) >= 0;");
	}

	protected abstract void GenerateCompareToBody(Identifier identifier, IndentedTextWriter writer);
}

public sealed class DefaultComparableInterfaceGenerator : ComparableInterfaceGenerator
{
	protected override void GenerateCompareToBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("return Value.CompareTo(other.Value);");
	}
}

public sealed class StringComparableInterfaceGenerator : ComparableInterfaceGenerator
{
	protected override void GenerateCompareToBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("return (Value, other.Value) switch");
		writer.WriteBlock(() => {
			writer.WriteLine("(null, null) => 0,");
			writer.WriteLine("(null, _) => -1,");
			writer.WriteLine("(_, null) => 2,");
			writer.WriteLine("(_, _) => Value.CompareTo(other.Value)");
		}, withSemicolon: true);
	}
}
