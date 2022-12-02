using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class StringCreation : IMethod, IUsing
    {
        public string Name { get; set; } = "STRING";
        public string Path { get; set; } = "Variables";

        public void Execute(string[] para)
        {
            VariableSetup.AddEntry(para[1], TextMethods.ParseText(para, 2, '[', ']'));
        }
    }
}