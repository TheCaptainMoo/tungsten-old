using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class Print : IMethod, IUsing, ILexer
    {
        public string Name { get; set; } = "PRINT";
        public string Path { get; set; } = "System";
        public Regex RegexCode { get; set; } = new Regex(@"^print$|WSprint");

        public void Execute(string[] para)
        {
            Console.WriteLine(TextMethods.ParseText(para, 1, '[', ']'));
        }
    }
}