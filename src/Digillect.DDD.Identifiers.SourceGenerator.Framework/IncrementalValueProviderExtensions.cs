using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Digillect.DDD.Identifiers.SourceGenerator;

public static class IncrementalValueProviderExtensions
{
	public static IncrementalValueProvider<ImmutableArray<T>> Concat<T>(
		this IncrementalValueProvider<ImmutableArray<T>> first,
		IncrementalValueProvider<ImmutableArray<T>> second)
	{
		return first
			.Combine(second)
			.SelectMany<(ImmutableArray<T>, ImmutableArray<T>), T>((c, _) => [..c.Item1, ..c.Item2])
			.Collect();
	}
}
