using Tungsten_Interpreter.Utilities.Parser.Methods;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class Print : IMethod, IUsing
    {
        public string Name { get; set; } = "PRINT";
        public string Path { get; set; } = "System";

        public void Execute(string[] para)
        {
            Console.WriteLine(TextMethods.ParseText(para, 1, '[', ']'));
        }
    }
}