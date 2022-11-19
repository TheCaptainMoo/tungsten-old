using Tungsten_Interpreter.Utilities.Parser.Methods;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class Print : IMethod
    {
        public string Name { get; set; } = "PRINT";

        public void Execute(string[] para)
        {
            Console.WriteLine(TextMethods.ParseText(para, 1, '[', ']'));
        }
    }
}