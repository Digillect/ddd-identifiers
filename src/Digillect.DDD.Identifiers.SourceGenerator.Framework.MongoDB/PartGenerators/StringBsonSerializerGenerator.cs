using System.CodeDom.Compiler;

namespace Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

public sealed class StringBsonSerializerGenerator : BsonSerializerGenerator
{
	protected override string BsonType => "String";

	protected override void EmitDeserializeBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("string stringValue = context.Reader.ReadString();");
		writer.WriteEmptyLine();
		writer.WriteLine($"return new {identifier.FullTypeName}(stringValue);");
	}

	protected override void EmitSerializeBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("context.Writer.WriteString(value.Value);");
	}
}