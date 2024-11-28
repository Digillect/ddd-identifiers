using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Digillect.DDD.Identifiers.SourceGenerator;

public interface IIdentifierPartGenerator
{
	IEnumerable<string> GenerateAttributes(Identifier identifier);
	IEnumerable<string> GenerateInterfaces(Identifier identifier);

	void GenerateMembers(Identifier identifier, IndentedTextWriter writer);
}
