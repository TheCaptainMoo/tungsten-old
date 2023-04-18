using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.AST;
using Tungsten_Interpreter.Utilities.Parser.Methods;

using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class IntegerCreation : ILexer
    {
        public string Name { get; set; } = "INT";
        public Regex RegexCode { get; set; } = new Regex(@"^int$|WSint");

        public AbstractSyntaxTree.AstNode AstConstructor(string[] para)
        {
            return new AbstractSyntaxTree.VariableNodedAssignNode(VariableSetup.VariableTypes.Int, para[1], TextMethods.IntAstParse(para, 3));
        }
    }
}