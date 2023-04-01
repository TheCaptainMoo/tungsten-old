using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tungsten_Interpreter.Utilities.Parser
{
    public interface INestedLexer
    {
        public string Name { get; set; }
        public Regex RegexCode { get; set; }
        AST.AbstractSyntaxTree.LinedAstReturn AstConstructor(string[] para, List<string[]> lines, int lineNum);
    }
}
