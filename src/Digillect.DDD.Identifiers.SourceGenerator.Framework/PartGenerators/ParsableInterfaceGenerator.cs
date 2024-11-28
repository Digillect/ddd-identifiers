using System.CodeDom.Compiler;

namespace Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

public abstract class ParsableInterfaceGenerator : IIdentifierPartGenerator
{
	public IEnumerable<string> GenerateAttributes(Identifier identifier)
	{
		return Array.Empty<string>();
	}

	public IEnumerable<string> GenerateInterfaces(Identifier identifier)
	{
		return [$"global::System.IParsable<{identifier.Name}>"];
	}

	public void GenerateMembers(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine($"public static {identifier.Name} Parse(string s) => Parse(s, null);");
		writer.WriteLine($"public static {identifier.Name} Parse(string s, global::System.IFormatProvider? provider)");
		writer.WriteBlock(() => EmitParseBody(identifier, writer));
		writer.WriteEmptyLine();
		writer.WriteLine($"public static bool TryParse(string? s, [global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out {identifier.Name} result) => TryParse(s, null, out result);");
		writer.WriteLine($"public static bool TryParse(string? s, global::System.IFormatProvider? provider, [global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out {identifier.Name} result)");
		writer.WriteBlock(() => EmitTryParseBody(identifier, writer));
	}

	/// <summary>
	/// Генерирует код метода <c>Parse</c>
	/// </summary>
	protected abstract void EmitParseBody(Identifier identifier, IndentedTextWriter writer);

	/// <summary>
	/// Генерирует код метода <c>TryParse</c>
	/// </summary>
	protected abstract void EmitTryParseBody(Identifier identifier, IndentedTextWriter writer);
}

public sealed class GuidParsableInterfaceGenerator : ParsableInterfaceGenerator
{
	/// <inheritdoc />
	protected override void EmitParseBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine($"return new {identifier.Name}(global::System.Guid.Parse(s, provider));");
	}

	/// <inheritdoc />
	protected override void EmitTryParseBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("if (global::System.Guid.TryParse(s, provider, out global::System.Guid parsedValue))");
		writer.WriteBlock(() => {
			writer.WriteLine($"result = new {identifier.Name}(parsedValue);");
			writer.WriteEmptyLine();
			writer.WriteLine("return true;");
		});
		writer.WriteEmptyLine();
		writer.WriteLine("result = default;");
		writer.WriteEmptyLine();
		writer.WriteLine("return false;");
	}
}

public sealed class IntegerParsableInterfaceGenerator : ParsableInterfaceGenerator
{
	/// <inheritdoc />
	protected override void EmitParseBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine($"return new {identifier.Name}(int.Parse(s, provider));");
	}

	/// <inheritdoc />
	protected override void EmitTryParseBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("if (int.TryParse(s, provider, out int parsedValue))");
		writer.WriteBlock(() => {
			writer.WriteLine($"result = new {identifier.Name}(parsedValue);");
			writer.WriteEmptyLine();
			writer.WriteLine("return true;");
		});
		writer.WriteEmptyLine();
		writer.WriteLine("result = default;");
		writer.WriteEmptyLine();
		writer.WriteLine("return false;");
	}
}
