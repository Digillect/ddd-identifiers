using System.CodeDom.Compiler;

namespace Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

public sealed class BsonObjectIdBsonSerializerGenerator : BsonSerializerGenerator
{
	protected override string BsonType => "ObjectId";

	protected override void EmitDeserializeBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("global::MongoDB.Bson.ObjectId idValue = context.Reader.ReadObjectId();");
		writer.WriteEmptyLine();
		writer.WriteLine($"return new {identifier.FullTypeName}(idValue);");
	}

	protected override void EmitSerializeBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("context.Writer.WriteObjectId(value.Value);");
	}
}