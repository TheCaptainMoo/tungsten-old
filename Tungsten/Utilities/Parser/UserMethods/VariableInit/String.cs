using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class StringCreation : ILexer
    {
        public string Name { get; set; } = "STRING";
        public Regex RegexCode { get; set; } = new Regex(@"^string$|WSstring");

        public AST.AbstractSyntaxTree.AstNode AstConstructor(string[] para)
        {
            return new AST.AbstractSyntaxTree.VariableNodedAssignNode(VariableSetup.VariableTypes.String, para[1], TextMethods.AstParse(para, 3, VariableSetup.VariableTypes.String));

            ////return new AST.AbstractSyntaxTree.VariableNodedAssignNode(VariableSetup.VariableTypes.String, para[1], TextMethods.StringAstParse(para, 3));
        }
    }
}