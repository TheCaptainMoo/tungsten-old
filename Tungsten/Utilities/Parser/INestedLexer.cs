using System.Text.RegularExpressions;

namespace Tungsten_Interpreter.Utilities.Parser
{
    public interface INestedLexer
    {
        public string Name { get; set; }
        public Regex RegexCode { get; set; }
        AST.AbstractSyntaxTree.LinedAstReturn AstConstructor(string[] para, List<string[]> lines, int lineNum);
    }
}
