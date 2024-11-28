namespace Digillect.DDD.Identifiers.SourceGenerator;

public sealed class Identifier
{
	public static readonly Identifier Invalid = new(default!, string.Empty, string.Empty, string.Empty);

	public string Namespace { get; }

	public string Name { get; }

	public string FullTypeName { get; }

	public IdentifierType Type { get; }

	public bool IsValid => this != Invalid;
	public string ValueTypeName => Type.ValueTypeName;

	private Identifier(IdentifierType type, string ns, string name, string fullTypeName)
	{
		Namespace = ns;
		Name = name;
		FullTypeName = fullTypeName;
		Type = type;
	}

	public static Identifier Create(IdentifierType type, string ns, string name, string fullTypeName)
	{
		if (string.IsNullOrWhiteSpace(ns))
		{
			throw new ArgumentException("Namespace can't be null or whitespace.", nameof(ns));
		}

		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Name can't be null or whitespace.", nameof(name));
		}

		if (string.IsNullOrWhiteSpace(fullTypeName))
		{
			throw new ArgumentException("FullTypeName can't be null or whitespace.", nameof(fullTypeName));
		}

		return new Identifier(type, ns, name, fullTypeName);
	}
}
