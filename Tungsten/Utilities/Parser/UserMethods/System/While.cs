using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class While : ILineInteractable
    {
        public string Name { get; set; } = "WHILE";

        public int lineExecute(string[] para, int lineNumber)
        {

            return 0;
        }
    }
}