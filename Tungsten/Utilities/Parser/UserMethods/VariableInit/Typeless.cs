using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods.VariableInit
{
    internal class Typeless : IMethod, IUsing
    {
        public string Name { get; set; } = "TL";
        public string Path { get; set; } = "Variables";

        public void Execute(string[] para)
        {
            List<string> param = para.ToList();
            param.Insert(2, "@");
            param.Insert(param.Count, "*");
            VariableSetup.AddEntry(param[1], TextMethods.ParseText(param.ToArray(), 2, '@', '*').Trim());
        }
    }
}
