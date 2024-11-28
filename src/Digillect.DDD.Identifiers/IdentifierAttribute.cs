namespace Digillect.DDD.Identifiers;

[AttributeUsage(AttributeTargets.Struct)]
// ReSharper disable once UnusedTypeParameter
public class IdentifierAttribute<TValue> : Attribute where TValue : notnull;
