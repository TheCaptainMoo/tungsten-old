using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class Math : IMethod
    {
        public string Name { get; set; } = "MATH";

        public void Execute(string[] para)
        {
            string compute = "";
            try
            {
                for (int j = 1; j < para.Length; j++)
                {
                    if (VariableSetup.globalVar.ContainsKey(para[j]))
                    {
                        compute += VariableSetup.globalVar[para[j]];
                    }
                    else
                    {
                        compute += para[j];
                    }
                }
                Console.WriteLine(Maths.Evaluate(compute));
            }
            catch
            {
                Console.WriteLine(Maths.Evaluate(TextMethods.CalcString(String.Join(" ", para, 1, para.Length - 1), '(', ')')));
            }
        }
    }
}