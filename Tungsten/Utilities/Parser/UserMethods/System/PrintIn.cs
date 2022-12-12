using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class PrintIn : IMethod, IUsing, ILexer
    {
        public string Name { get; set; } = "PRINTIN";
        public string Path { get; set; } = "System";
        public Regex RegexCode { get; set; } = new Regex(@"^printin$|^printin:$|WSprintin");

        public void Execute(string[] para)
        {
            Console.Write(TextMethods.ParseText(para, 1, '[', ']'));
        }
    }
}