using System.Collections;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class StringMatrixCreation : IMethod, IUsing
    {
        public string Name { get; set; } = "STRING[]";
        public string Path { get; set; } = "Variables.Matrix";

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
            
            //foreach(DictionaryEntry item in VariableSetup.globalVar)
            //{
            //    string[] val = VariableSetup.globalVar[item.Key] as string[];
            //    
            //    for(int i = 0; i < val.Length; i++)
            //    {
            //        Console.WriteLine(val[i]);
            //    }
            //}
        }
    }
}