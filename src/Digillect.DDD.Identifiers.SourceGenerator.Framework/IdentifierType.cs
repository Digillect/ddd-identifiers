using System.Diagnostics;

namespace Digillect.DDD.Identifiers.SourceGenerator;

[DebuggerDisplay("{ValueTypeName}")]
public class IdentifierType(string valueTypeName)
{
	private readonly List<IdentifierDetector> _detectors = [];
	private readonly List<IIdentifierPartGenerator> _generators = [];

	public string ValueTypeName { get; } = valueTypeName;
	public IReadOnlyCollection<IdentifierDetector> Detectors => _detectors.AsReadOnly();
	public IReadOnlyCollection<IIdentifierPartGenerator> Generators => _generators.AsReadOnly();

	public IdentifierType AddDetector(IdentifierDetector detector)
	{
		_detectors.Add(detector);

		return this;
	}

	public IdentifierType AddGenerator(IIdentifierPartGenerator generator)
	{
		_generators.Add(generator);

		return this;
	}
}
