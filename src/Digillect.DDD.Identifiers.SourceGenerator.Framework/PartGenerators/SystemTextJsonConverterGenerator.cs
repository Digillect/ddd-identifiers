using System.CodeDom.Compiler;

namespace Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

public abstract class SystemTextJsonConverterGenerator : IIdentifierPartGenerator
{
	/// <inheritdoc />
	public IEnumerable<string> GenerateAttributes(Identifier identifier)
	{
		yield return $"[global::System.Text.Json.Serialization.JsonConverter(typeof({identifier.Name}.SystemTextJsonConverter))]";
	}

	/// <inheritdoc />
	public IEnumerable<string> GenerateInterfaces(Identifier identifier)
	{
		return Array.Empty<string>();
	}

	/// <inheritdoc />
	public void GenerateMembers(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine($"class SystemTextJsonConverter : global::System.Text.Json.Serialization.JsonConverter<{identifier.Name}>");
		writer.WriteBlock(() => {
			writer.WriteLine(
				$"public override {identifier.Name} Read(ref global::System.Text.Json.Utf8JsonReader reader, global::System.Type typeToConvert, global::System.Text.Json.JsonSerializerOptions options)");
			writer.WriteBlock(() => EmitReadBody(identifier, writer));
			writer.WriteEmptyLine();
			writer.WriteLine(
				$"public override void Write(global::System.Text.Json.Utf8JsonWriter writer, {identifier.Name} value, global::System.Text.Json.JsonSerializerOptions options)");
			writer.WriteBlock(() => EmitWriteBody(identifier, writer));
		});
	}


	/// <summary>
	/// Генерирует код метода <c>Read</c> системного конвертера JSON.
	/// </summary>
	protected abstract void EmitReadBody(Identifier identifier, IndentedTextWriter writer);

	/// <summary>
	/// Генерирует код метода <c>Write</c> системного конвертера JSON.
	/// </summary>
	protected abstract void EmitWriteBody(Identifier identifier, IndentedTextWriter writer);
}

public sealed class GuidSystemTextJsonConverterGenerator : SystemTextJsonConverterGenerator
{
	protected override void EmitReadBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine($"return new {identifier.FullTypeName}(global::System.Guid.Parse(reader.GetString()!));");
	}

	protected override void EmitWriteBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("writer.WriteStringValue(value.Value);");
	}
}

public sealed class IntegerSystemTextJsonConverterGenerator : SystemTextJsonConverterGenerator
{
	protected override void EmitReadBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine($"return new {identifier.FullTypeName}(reader.GetInt32());");
	}

	protected override void EmitWriteBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("writer.WriteNumberValue(value.Value);");
	}
}

public sealed class StringSystemTextJsonConverterGenerator : SystemTextJsonConverterGenerator
{
	protected override void EmitReadBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine($"return new {identifier.FullTypeName}(reader.GetString()!);");
	}

	protected override void EmitWriteBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("writer.WriteStringValue(value.Value);");
	}
}
