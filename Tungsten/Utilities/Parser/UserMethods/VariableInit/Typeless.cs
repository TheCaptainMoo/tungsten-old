using System.Text;
using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods.VariableInit
{
    public class Typeless : IMethod, IUsing, ILexer
    {
        public string Name { get; set; } = "TL";
        public string Path { get; set; } = "Variables";
        public Regex RegexCode { get; set; } = new Regex(@"^var$|WSvar|#");

        // Creates a Typeless Variable in Memory
        public void Execute(string[] para)
        {
            List<string> param = para.ToList();
            param.Insert(3, "@");
            param.Insert(param.Count, "*");
            //VariableSetup.AddEntry(param[1], TextMethods.ParseText(param.ToArray(), 2, '@', '*').Trim());
            VariableSetup.AddEntry(param[1], VariableSetup.VariableTypes.Typeless, Encoding.UTF8.GetBytes(TextMethods.ParseText(param.ToArray(), 3, '@', '*').Trim()));
        }
    }
}
