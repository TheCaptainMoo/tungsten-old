using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class Function : ILineInteractable
    {
        public string Name { get; set; } = "FUNCT";

        public int lineExecute(string[] para, int lineNumber)
        {

            return 0;
        }
    }
}