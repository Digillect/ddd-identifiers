using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Digillect.DDD.Identifiers.SourceGenerator;

public abstract class IdentifierSelector
{
	public abstract IncrementalValueProvider<ImmutableArray<Identifier>>? CreateValueProvider(
		IncrementalGeneratorInitializationContext context,
		IReadOnlyCollection<IdentifierType> identifierTypes);
}
