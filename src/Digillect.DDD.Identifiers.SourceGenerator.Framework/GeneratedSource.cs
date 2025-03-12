namespace Digillect.DDD.Identifiers.SourceGenerator;

public sealed class GeneratedSource(string fileName, string sourceCode)
{
	public string FileName => fileName;
	public string SourceCode => sourceCode;
}
