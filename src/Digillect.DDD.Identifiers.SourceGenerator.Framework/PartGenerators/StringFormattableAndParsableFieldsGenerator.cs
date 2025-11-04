using System.CodeDom.Compiler;

namespace Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

public sealed class StringFormattableAndParsableFieldsGenerator : IIdentifierPartGenerator
{
	public IEnumerable<string> GenerateAttributes(Identifier identifier) => [];

	public IEnumerable<string> GenerateInterfaces(Identifier identifier) => [];

	public void GenerateMembers(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("private static readonly global::System.Text.Encoding _utf8Encoding = global::System.Text.Encoding.UTF8;");
		writer.WriteEmptyLine();
	}
}
