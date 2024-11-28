using System.Collections.Immutable;

namespace Digillect.DDD.Identifiers.SourceGenerator;

public sealed class IdentifierSourceGeneratorBuilder
{
	private readonly HashSet<IdentifierSelector> _selectors = [];
	private readonly List<IdentifierType> _identifierTypes = [];

	public IReadOnlyCollection<IdentifierType> IdentifierTypes => _identifierTypes.AsReadOnly();
	public IReadOnlyCollection<IdentifierSelector> Selectors => _selectors.ToImmutableArray();

	public IdentifierType AddType(string valueTypeName)
	{
		var builder = new IdentifierType(valueTypeName);

		_identifierTypes.Add(builder);

		return builder;
	}

	public void AddSelector<T>() where T : IdentifierSelector, new()
	{
		if (!_selectors.Any(s => s is T))
		{
			_selectors.Add(new T());
		}
	}
}
