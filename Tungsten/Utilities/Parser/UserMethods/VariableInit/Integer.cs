using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;

using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class IntegerCreation : IMethod, IUsing, ILexer
    {
        public string Name { get; set; } = "INT";
        public string Path { get; set; } = "Variables";
        public Regex RegexCode { get; set; } = new Regex(@"^int$|^int:$|WSint");

        // Creates an Integer in Memorys
        public void Execute(string[] para)
        {
            try
            {
                double maths = Maths.Evaluate(TextMethods.CalcString(String.Join(" ", VariableSetup.Convert(para, 2), 1, para.Length - 1), '(', ')'));
                VariableSetup.AddEntry(para[1], Convert.ToInt32(maths));
            }
            catch
            {
                VariableSetup.AddEntry(para[1], Convert.ToInt32(para[2]));
            }
        }
    }
}