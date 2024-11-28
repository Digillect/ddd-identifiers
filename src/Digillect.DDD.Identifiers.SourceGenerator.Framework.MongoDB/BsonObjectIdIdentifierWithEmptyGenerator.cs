using Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

namespace Digillect.DDD.Identifiers.SourceGenerator;

public sealed class BsonObjectIdIdentifierWithEmptyGenerator : IdentifierWithEmptyGenerator
{
	protected override string ConstructorParameter => "global::MongoDB.Bson.ObjectId.Empty";
}