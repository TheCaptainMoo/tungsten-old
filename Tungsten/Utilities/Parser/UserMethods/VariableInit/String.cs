using System.Text;
using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class StringCreation : ILexer
    {
        public string Name { get; set; } = "STRING";
        public Regex RegexCode { get; set; } = new Regex(@"^string$|WSstring");

        public void AstConstructor(string[] para)
        {
            VariableSetup.nodes.Add(new AST.AbstractSyntaxTree.VariableAssignNode(VariableSetup.VariableTypes.String, para[1], Encoding.UTF8.GetBytes(TextMethods.ParseText(para, 3, '[', ']'))));
        }
    }
}