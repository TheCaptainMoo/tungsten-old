using System.Collections;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class MatrixCreation : IMethod, IUsing
    {
        public string Name { get; set; } = "MATRIX";
        public string Path { get; set; } = "Variables.Matrix";

        // Creates a 'Matrix' (Array) in Memory
        public void Execute(string[] para)
        {
            List<string> value = new List<string>();

            value = TextMethods.ParseText(para, 2, '<', '>').Split(",").ToList();

            for(int i = 0; i < value.Count; i++)
            {
                value[i] = value[i].Trim();
                value[i] = value[i].Substring(1, value[i].Length - 2);
            }

            VariableSetup.AddEntry(para[1], value.ToArray());
        }
    }
}