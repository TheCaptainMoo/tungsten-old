using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class StringCreation : IMethod
    {
        public string Name { get; set; } = "STRING";

        public void Execute(string[] para)
        {
            VariableSetup.AddEntry(para[1], TextMethods.ParseText(para, 2, '[', ']'));
            //Console.WriteLine(VariableSetup.globalVar[para[1]]);
        }
    }
}