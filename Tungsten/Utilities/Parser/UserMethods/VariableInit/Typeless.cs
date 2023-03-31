using System.Text;
using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods.VariableInit
{
    public class Typeless : ILexer
    {
        public string Name { get; set; } = "TL";
        public Regex RegexCode { get; set; } = new Regex(@"^var$|WSvar|#");

        public void AstConstructor(string[] para)
        {
            List<string> param = para.ToList();
            param.Insert(3, "@");
            param.Insert(param.Count, "*");

            VariableSetup.nodes.Add(new AST.AbstractSyntaxTree.VariableAssignNode(VariableSetup.VariableTypes.Typeless, param[1], Encoding.UTF8.GetBytes(TextMethods.ParseText(param.ToArray(), 3, '@', '*').Trim())));
        }
    }
}
