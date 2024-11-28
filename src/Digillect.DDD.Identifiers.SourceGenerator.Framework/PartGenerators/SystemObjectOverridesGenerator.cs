using System.CodeDom.Compiler;

namespace Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

public abstract class SystemObjectOverridesGenerator : IIdentifierPartGenerator
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
		writer.WriteLine("public override bool Equals(object? obj)");
		writer.WriteBlock(() => {
			writer.WriteLine("if (ReferenceEquals(null, obj))");
			writer.WriteBlock(() => writer.WriteLine("return false;"));
			writer.WriteEmptyLine();
			writer.WriteLine($"return obj is {identifier.Name} other && Equals(other);");
		});
		writer.WriteEmptyLine();
		writer.WriteLine("public override int GetHashCode() => Value.GetHashCode();");
		writer.WriteEmptyLine();
		writer.Write("public override string ToString()");
		EmitToStringBody(identifier, writer);
		writer.WriteEmptyLine();
		writer.WriteLine($"public static bool operator ==({identifier.Name} a, {identifier.Name} b) => a.Equals(b);");
		writer.WriteLine($"public static bool operator !=({identifier.Name} a, {identifier.Name} b) => !(a == b);");
	}

	/// <summary>
	/// Генерирует исходный текст метода <see cref="Object.ToString" />.
	/// </summary>
	protected abstract void EmitToStringBody(Identifier identifier, IndentedTextWriter writer);
}

public sealed class DefaultSystemObjectOverridesGenerator : SystemObjectOverridesGenerator
{
	protected override void EmitToStringBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine(" => Value.ToString();");
	}
}

public sealed class StringSystemObjectOverridesGenerator : SystemObjectOverridesGenerator
{
	protected override void EmitToStringBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine(" => Value;");
	}
}
