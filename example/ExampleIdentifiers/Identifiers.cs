using Digillect.DDD.Identifiers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace ExampleIdentifiers;

[Identifier<Guid>]
public readonly partial struct GuidBasedIdentifier;

[Identifier<ObjectId>]
public readonly partial struct ObjectIdBasedIdentifier;

[AttributeUsage(AttributeTargets.Struct)]
public sealed class IdentifierAttribute : Attribute
{
	public IdentifierAttribute()
	{
		var s = new GuidSerializer(GuidRepresentation.Standard);
	}
}
