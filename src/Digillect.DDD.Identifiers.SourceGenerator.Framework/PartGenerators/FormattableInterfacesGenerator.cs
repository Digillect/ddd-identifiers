using System.Buffers.Text;
using System.CodeDom.Compiler;
using System.Text;

namespace Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

public abstract class FormattableInterfacesGenerator : IIdentifierPartGenerator
{
	public IEnumerable<string> GenerateAttributes(Identifier identifier) => [];

	public IEnumerable<string> GenerateInterfaces(Identifier identifier) =>
		["global::System.IFormattable", "global::System.IUtf8SpanFormattable"];

	public void GenerateMembers(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("public string ToString(string? format, global::System.IFormatProvider? formatProvider)");
		writer.WriteBlock(() => EmitFormattableBody(identifier, writer));
		writer.WriteEmptyLine();
		writer.WriteLine("public bool TryFormat(global::System.Span<byte> utf8Destination, out int bytesWritten, global::System.ReadOnlySpan<char> format, global::System.IFormatProvider? provider)");
		writer.WriteBlock(() => EmitUtf8SpanFormattableBody(identifier, writer));
	}

	protected abstract void EmitFormattableBody(Identifier identifier, IndentedTextWriter writer);
	protected abstract void EmitUtf8SpanFormattableBody(Identifier identifier, IndentedTextWriter writer);
}

public sealed class DefaultFormattableInterfacesGenerator : FormattableInterfacesGenerator
{
	protected override void EmitFormattableBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("return ((global::System.IFormattable) Value).ToString(format, formatProvider);");
	}

	protected override void EmitUtf8SpanFormattableBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("return ((global::System.IUtf8SpanFormattable) Value).TryFormat(utf8Destination, out bytesWritten, format, provider);");
	}
}

public sealed class StringFormattableInterfacesGenerator : FormattableInterfacesGenerator
{
	protected override void EmitFormattableBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("return Value;");
	}

	protected override void EmitUtf8SpanFormattableBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("return _utf8Encoding.TryGetBytes(Value, utf8Destination, out bytesWritten);");
	}
}

public sealed class GuidFormattableInterfacesGenerator : FormattableInterfacesGenerator
{
	protected override void EmitFormattableBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("return Value.ToString(format, provider);");
	}

	protected override void EmitUtf8SpanFormattableBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("return Value.TryFormat(utf8Destination, out bytesWritten, format);");
	}
}
