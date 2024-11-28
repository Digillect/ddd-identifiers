namespace Digillect.DDD.Identifiers;

/// <summary>
/// Detects the value type of the identifiers.
/// </summary>
public static class IdentifierBackingTypeDetector
{
    private static readonly Type IdentifierInterfaceType = typeof(IIdentifier<,>);

	/// <summary>
	/// Returns the type of the identifier's value if the given type is an identifier.
	/// </summary>
	/// <param name="potentialIdentifierType">The type to check.</param>
	/// <returns>Identifier's value type or <c>null</c> if <paramref name="potentialIdentifierType"/> is not an identifier.</returns>
	public static Type? GetBackingType(Type potentialIdentifierType)
    {
	    Type? givenType = potentialIdentifierType;

	    while (givenType is not null)
	    {
			if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == IdentifierInterfaceType)
			{
				return givenType.GetGenericArguments()[1];
			}

			foreach (var it in givenType.GetInterfaces())
		    {
			    if (it.IsGenericType && it.GetGenericTypeDefinition() == IdentifierInterfaceType)
			    {
				    return it.GetGenericArguments()[1];
			    }
		    }

			givenType = givenType.BaseType;
	    }

		return null;
	}
}
