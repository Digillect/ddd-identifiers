using Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

namespace Digillect.DDD.Identifiers.SourceGenerator;

public static class MongoDbIdentifierSourceGeneratorBuilderExtensions
{
	public static IdentifierSourceGeneratorBuilder AddObjectIdType(this IdentifierSourceGeneratorBuilder builder, Action<IdentifierType>? configure = null)
	{
		var type = builder.HandleStructsDecoratedWithDigillectIdentifierAttribute("global::MongoDB.Bson.ObjectId")
			.AddGenerator<DefaultConstructorGenerator>()
			.AddGenerator<BsonObjectIdIdentifierWithNewGenerator>()
			.AddGenerator<BsonObjectIdIdentifierWithEmptyGenerator>()
			.AddGenerator<BsonObjectIdParsableInterfaceGenerator>()
			.AddGenerator<DefaultEquatableInterfaceGenerator>()
			.AddGenerator<DefaultComparableInterfaceGenerator>()
			.AddGenerator<DefaultSystemObjectOverridesGenerator>()
			.AddGenerator<BsonObjectIdComponentModelTypeConverterGenerator>()
			.AddGenerator<BsonObjectIdSystemTextJsonConverterGenerator>()
			.AddGenerator<BsonObjectIdBsonSerializerGenerator>();

		configure?.Invoke(type);

		return builder;
	}
}
