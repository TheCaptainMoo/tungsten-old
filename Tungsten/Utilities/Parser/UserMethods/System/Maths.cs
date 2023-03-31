/*using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    [Obsolete]
    public class Math : IMethod, IUsing, ILexer
    {
        public string Name { get; set; } = "MATH";
        public string Path { get; set; } = "System";
        public Regex RegexCode { get; set; } = new Regex(@"^math$|WSmath");

        public void Execute(string[] para)
        {
            string compute = "";
            try
            {
                for (int j = 1; j < para.Length; j++)
                {
                    if (VariableSetup.globalVar.ContainsKey(para[j].Replace("(", "").Replace(")", "")) || VariableSetup.globalVar.ContainsKey(Regex.Replace(para[j].Replace("(", "").Replace(")", ""), @"<[0-9]>", "")))
                    {
                        compute += VariableSetup.Convert(para, j);
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
}*/