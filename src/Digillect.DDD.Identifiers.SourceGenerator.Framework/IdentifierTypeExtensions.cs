namespace Digillect.DDD.Identifiers.SourceGenerator;

public static class IdentifierTypeExtensions
{
	public static IdentifierType AddGenerator<T>(this IdentifierType type)
		where T : IIdentifierPartGenerator, new()
	{
		return type.AddGenerator(new T());
	}
}
