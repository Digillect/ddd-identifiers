using System.CodeDom.Compiler;

namespace Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

public abstract class ParsableInterfaceGenerator : IIdentifierPartGenerator
{
	public virtual IEnumerable<string> GenerateAttributes(Identifier identifier) => [];

	public virtual IEnumerable<string> GenerateInterfaces(Identifier identifier) =>
		[$"global::System.IParsable<{identifier.Name}>", $"global::System.IUtf8SpanParsable<{identifier.Name}>"];

	public void GenerateMembers(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine($"public static {identifier.Name} Parse(string s) => Parse(s, null);");
		writer.WriteLine($"public static {identifier.Name} Parse(string s, global::System.IFormatProvider? provider)");
		writer.WriteBlock(() => EmitParseBody(identifier, writer));
		writer.WriteEmptyLine();
		writer.WriteLine($"public static bool TryParse(string? s, [global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out {identifier.Name} result) => TryParse(s, null, out result);");
		writer.WriteLine($"public static bool TryParse(string? s, global::System.IFormatProvider? provider, [global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out {identifier.Name} result)");
		writer.WriteBlock(() => EmitTryParseBody(identifier, writer));
		writer.WriteEmptyLine();
		writer.WriteLine($"public static {identifier.Name} Parse(global::System.ReadOnlySpan<byte> utf8Text, global::System.IFormatProvider? provider)");
		writer.WriteBlock(() => EmitUtf8SpanParseBody(identifier, writer));
		writer.WriteEmptyLine();
		writer.WriteLine($"public static bool TryParse(global::System.ReadOnlySpan<byte> utf8Text, global::System.IFormatProvider? provider, [global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out {identifier.Name} result)");
		writer.WriteBlock(() => EmitUtf8SpanTryParseBody(identifier, writer));
	}

	/// <summary>
	/// Генерирует код метода <c>Parse</c>
	/// </summary>
	protected abstract void EmitParseBody(Identifier identifier, IndentedTextWriter writer);

	/// <summary>
	/// Генерирует код метода <c>TryParse</c>
	/// </summary>
	protected abstract void EmitTryParseBody(Identifier identifier, IndentedTextWriter writer);

	/// <summary>
	/// Генерирует код метода <c>Parse</c> интерфейса <c>IUtf8SpanParsable</c>
	/// </summary>
	protected abstract void EmitUtf8SpanParseBody(Identifier identifier, IndentedTextWriter writer);

	/// <summary>
	/// Генерирует код метода <c>TryParse</c> интерфейса <c>IUtf8SpanParsable</c>
	/// </summary>
	protected abstract void EmitUtf8SpanTryParseBody(Identifier identifier, IndentedTextWriter writer);
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

	/// <inheritdoc />
	protected override void EmitUtf8SpanParseBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("if (!global::System.Buffers.Text.Utf8Parser.TryParse(utf8Text, out global::System.Guid value, out _))");
		writer.WriteBlock(() => writer.WriteLine("throw new global::System.FormatException(\"The value is not a valid Guid.\");"));
		writer.WriteEmptyLine();
		writer.WriteLine($"return new {identifier.Name}(value);");
	}

	/// <inheritdoc />
	protected override void EmitUtf8SpanTryParseBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("if (!global::System.Buffers.Text.Utf8Parser.TryParse(utf8Text, out global::System.Guid value, out _))");
		writer.WriteBlock(() => {
			writer.WriteLine("result = default;");
			writer.WriteLine("return false;");
		});
		writer.WriteEmptyLine();
		writer.WriteLine($"result = new {identifier.Name}(value);");
		writer.WriteEmptyLine();
		writer.WriteLine("return true;");
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

	/// <inheritdoc />
	protected override void EmitUtf8SpanParseBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("if (!global::System.Buffers.Text.Utf8Parser.TryParse(utf8Text, out int value, out _))");
		writer.WriteBlock(() => writer.WriteLine("throw new global::System.FormatException(\"The value is not a valid Int32 value.\");"));
		writer.WriteEmptyLine();
		writer.WriteLine($"return new {identifier.Name}(value);");
	}

	/// <inheritdoc />
	protected override void EmitUtf8SpanTryParseBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("if (!global::System.Buffers.Text.Utf8Parser.TryParse(utf8Text, out int value, out _))");
		writer.WriteBlock(() => {
			writer.WriteLine("result = default;");
			writer.WriteLine("return false;");
		});
		writer.WriteEmptyLine();
		writer.WriteLine($"result = new {identifier.Name}(value);");
		writer.WriteEmptyLine();
		writer.WriteLine("return true;");
	}
}

public sealed class StringParsableInterfaceGenerator : ParsableInterfaceGenerator
{
	protected override void EmitParseBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine($"return new {identifier.Name}(s);");
	}

	protected override void EmitTryParseBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("if (string.IsNullOrWhiteSpace(s))");
		writer.WriteBlock(() => {
			writer.WriteLine("result = default;");
			writer.WriteLine("return false;");
		});
		writer.WriteEmptyLine();
		writer.WriteLine($"result = new {identifier.Name}(s);");
		writer.WriteEmptyLine();
		writer.WriteLine("return true;");
	}

	protected override void EmitUtf8SpanParseBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine($"return new {identifier.Name}(_utf8Encoding.GetString(utf8Text));");
	}

	protected override void EmitUtf8SpanTryParseBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine($"result = new {identifier.Name}(_utf8Encoding.GetString(utf8Text));");
		writer.WriteEmptyLine();
		writer.WriteLine("return true;");
	}
}
