using Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

namespace Digillect.DDD.Identifiers.SourceGenerator;

public sealed class BsonObjectIdIdentifierWithNewGenerator : IdentifierWithNewGenerator
{
	protected override string ConstructorParameter => "global::MongoDB.Bson.ObjectId.GenerateNewId()";
}