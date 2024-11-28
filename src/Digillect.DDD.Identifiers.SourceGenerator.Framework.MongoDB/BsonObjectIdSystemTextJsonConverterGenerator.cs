using System.CodeDom.Compiler;
using Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

namespace Digillect.DDD.Identifiers.SourceGenerator;

public sealed class BsonObjectIdSystemTextJsonConverterGenerator : SystemTextJsonConverterGenerator
{
	protected override void EmitReadBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine($"return new {identifier.Name}(global::MongoDB.Bson.ObjectId.Parse(reader.GetString()));");
	}

	protected override void EmitWriteBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("writer.WriteStringValue(value.Value.ToString());");
	}
}