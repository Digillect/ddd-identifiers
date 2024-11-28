using System.CodeDom.Compiler;

namespace Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

public sealed class IntegerBsonSerializerGenerator : BsonSerializerGenerator
{
	protected override string BsonType => "Int32";

	protected override void EmitDeserializeBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("int intValue = context.Reader.ReadInt32();");
		writer.WriteEmptyLine();
		writer.WriteLine($"return new {identifier.FullTypeName}(intValue);");
	}

	protected override void EmitSerializeBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("context.Writer.WriteInt32(value.Value);");
	}
}