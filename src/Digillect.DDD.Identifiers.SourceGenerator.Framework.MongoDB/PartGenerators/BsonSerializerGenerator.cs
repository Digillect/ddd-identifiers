using System.CodeDom.Compiler;

namespace Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

public abstract class BsonSerializerGenerator : IIdentifierPartGenerator
{
	public IEnumerable<string> GenerateAttributes(Identifier identifier)
	{
		yield return $"[global::MongoDB.Bson.Serialization.Attributes.BsonSerializer(typeof({identifier.Name}.BsonSerializer))]";
	}

	public IEnumerable<string> GenerateInterfaces(Identifier identifier)
	{
		return Array.Empty<string>();
	}

	public void GenerateMembers(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine($"private class BsonSerializer : global::MongoDB.Bson.Serialization.IBsonSerializer<{identifier.Name}>");
		writer.WriteBlock(() => {
			EmitClassFields(identifier, writer);
			writer.WriteLine(
				"object global::MongoDB.Bson.Serialization.IBsonSerializer.Deserialize(global::MongoDB.Bson.Serialization.BsonDeserializationContext context, global::MongoDB.Bson.Serialization.BsonDeserializationArgs args)");
			writer.WriteBlock(() => writer.WriteLine("return Deserialize(context, args);"));
			writer.WriteEmptyLine();
			writer.WriteLine(
				"void global::MongoDB.Bson.Serialization.IBsonSerializer.Serialize(global::MongoDB.Bson.Serialization.BsonSerializationContext context, global::MongoDB.Bson.Serialization.BsonSerializationArgs args, object value)");
			writer.WriteBlock(() => writer.WriteLine($"Serialize(context, args, ({identifier.FullTypeName}) value);"));
			writer.WriteEmptyLine();
			writer.WriteLine(
				$"public {identifier.Name} Deserialize(global::MongoDB.Bson.Serialization.BsonDeserializationContext context, global::MongoDB.Bson.Serialization.BsonDeserializationArgs args)");
			writer.WriteBlock(() => {
				writer.WriteLine("global::MongoDB.Bson.BsonType bsonType = context.Reader.GetCurrentBsonType();");
				writer.WriteEmptyLine();
				writer.WriteLine($"if (bsonType != global::MongoDB.Bson.BsonType.{BsonType})");
				writer.WriteBlock(() =>
					writer.WriteLine(
						$$"""throw new global::MongoDB.Bson.BsonSerializationException($"Unable to deserialize {typeof({{identifier.Name}}).FullName} from BSON type {bsonType}");"""));
				writer.WriteEmptyLine();
				EmitDeserializeBody(identifier, writer);
			});
			writer.WriteEmptyLine();
			writer.WriteLine(
				$"public void Serialize(global::MongoDB.Bson.Serialization.BsonSerializationContext context, global::MongoDB.Bson.Serialization.BsonSerializationArgs args, {identifier.Name} value)");
			writer.WriteBlock(() => EmitSerializeBody(identifier, writer));
			writer.WriteEmptyLine();
			writer.WriteLine($"public Type ValueType => typeof({identifier.FullTypeName});");
		});
	}

	protected abstract string BsonType { get; }
	protected abstract void EmitDeserializeBody(Identifier identifier, IndentedTextWriter writer);
	protected abstract void EmitSerializeBody(Identifier identifier, IndentedTextWriter writer);

	protected virtual void EmitClassFields(Identifier identifier, IndentedTextWriter writer)
	{
	}
}
