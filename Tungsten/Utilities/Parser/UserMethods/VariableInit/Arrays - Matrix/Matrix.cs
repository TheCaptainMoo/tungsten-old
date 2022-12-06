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

            //para = VariableSetup.Format(para, 2);
            List<string> param = VariableSetup.Format(para, 2).ToList();

            for (int i = 2; i < para.Length; i++)
            {
                value.Add(TextMethods.CalcStringForward(String.Join(" ", param), '[', ']'));

                if (param[2].EndsWith(']'))
                {
                    param.RemoveAt(2);
                }
                else
                {
                    while (!param[2].EndsWith(']')) {
                        param.RemoveAt(2);
                    }
                    param.RemoveAt(2);
                }              

                //param.Remove("[" + TextMethods.CalcStringForward(String.Join(" ", param), '[', ']') + "]");
                //param.RemoveAt(2);

                if (param.Count <= 3)
                {
                    //Console.WriteLine("End Matrix Write");
                    break;
                }
            }

            //value = TextMethods.ParseText(VariableSetup.Convert(para, 2), 2, '<', '>').Split(",").ToList();

            //for(int i = 0; i < value.Count; i++)
            //{
            //    value[i] = value[i].Trim();
                //value[i] = value[i].Substring(1, value[i].Length - 2);
            //}

            VariableSetup.AddEntry(para[1], value.ToArray());
        }
    }
}