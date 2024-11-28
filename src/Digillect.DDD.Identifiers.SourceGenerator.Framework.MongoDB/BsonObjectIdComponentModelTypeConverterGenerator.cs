using System.CodeDom.Compiler;
using System.Collections.Generic;
using Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

namespace Digillect.DDD.Identifiers.SourceGenerator;

public sealed class BsonObjectIdComponentModelTypeConverterGenerator : ComponentModelTypeConverterGenerator
{
	protected override IEnumerable<string> ConversionSourceTypes => ["global::MongoDB.Bson.ObjectId", "string"];

	protected override void EmitConvertFromBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("return value switch");
		writer.WriteBlock(() => {
			writer.WriteLine($"global::MongoDB.Bson.ObjectId idValue => new {identifier.Name}(idValue),");
			writer.WriteLine($"string stringValue when !string.IsNullOrEmpty(stringValue) && global::MongoDB.Bson.ObjectId.TryParse(stringValue, out global::MongoDB.Bson.ObjectId result) => new {identifier.Name}(result),");
			writer.WriteLine("_ => base.ConvertFrom(context, culture, value)");
		}, withSemicolon: true);
	}

	protected override void EmitConvertToBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("if (destinationType == typeof(global::MongoDB.Bson.ObjectId))");
		writer.WriteBlock(() => writer.WriteLine("return idValue.Value;"));
		writer.WriteEmptyLine();
		writer.WriteLine("if (destinationType == typeof(string))");
		writer.WriteBlock(() => writer.WriteLine("return idValue.Value.ToString();"));
	}
}
