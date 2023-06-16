using Tungsten_Interpreter.Utilities.Parser.UserMethods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser
{
    internal interface IVarCallable
    {
        public string FunctionName { get; set; }
        public List<VariableSetup.VariableTypes>? SupportedVariableTypes { get; set; }
        public void FunctionConstructor();
    }
}