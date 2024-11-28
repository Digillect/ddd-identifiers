using System.CodeDom.Compiler;

namespace Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

public abstract class ConstructorGenerator : IIdentifierPartGenerator
{
	public IEnumerable<string> GenerateAttributes(Identifier identifier)
	{
		return Array.Empty<string>();
	}

	public IEnumerable<string> GenerateInterfaces(Identifier identifier)
	{
		return Array.Empty<string>();
	}

	public void GenerateMembers(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine($"public {identifier.Name}({identifier.ValueTypeName} value)");
		writer.WriteBlock(() => EmitConstructorBody(identifier, writer));
	}

	protected abstract void EmitConstructorBody(Identifier identifier, IndentedTextWriter writer);
}

public sealed class DefaultConstructorGenerator : ConstructorGenerator
{
	protected override void EmitConstructorBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("Value = value;");
	}
}

public sealed class StringConstructorGenerator : ConstructorGenerator
{
	protected override void EmitConstructorBody(Identifier model, IndentedTextWriter writer)
	{
		writer.WriteLine("Value = value ?? throw new global::System.ArgumentNullException(nameof(value));");
	}
}
