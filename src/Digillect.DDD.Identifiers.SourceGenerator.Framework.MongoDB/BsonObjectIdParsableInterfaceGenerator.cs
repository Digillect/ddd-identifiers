using System.CodeDom.Compiler;
using Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

namespace Digillect.DDD.Identifiers.SourceGenerator;

public sealed class BsonObjectIdParsableInterfaceGenerator : ParsableInterfaceGenerator
{
	/// <inheritdoc />
	protected override void EmitParseBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine($"return new {identifier.Name}(global::MongoDB.Bson.ObjectId.Parse(s));");
	}

	/// <inheritdoc />
	protected override void EmitTryParseBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("if (global::MongoDB.Bson.ObjectId.TryParse(s, out global::MongoDB.Bson.ObjectId parsedValue))");
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
