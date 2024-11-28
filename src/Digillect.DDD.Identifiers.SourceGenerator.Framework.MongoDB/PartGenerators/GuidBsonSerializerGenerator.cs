using System.CodeDom.Compiler;

namespace Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

public sealed class GuidBsonSerializerGenerator : BsonSerializerGenerator
{
	protected override string BsonType => "Binary";

	protected override void EmitClassFields(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine(
			"private static readonly global::MongoDB.Bson.Serialization.Serializers.GuidSerializer Serializer = new global::MongoDB.Bson.Serialization.Serializers.GuidSerializer();");
		writer.WriteEmptyLine();
	}

	protected override void EmitDeserializeBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("global::System.Guid guidValue = Serializer.Deserialize(context, args);");
		writer.WriteEmptyLine();
		writer.WriteLine($"return new {identifier.FullTypeName}(guidValue);");
	}

	protected override void EmitSerializeBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("Serializer.Serialize(context, args, value.Value);");
	}
}
