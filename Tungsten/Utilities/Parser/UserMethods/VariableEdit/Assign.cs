using System.Text;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods.VariableEdit
{
    internal class Assign : ILateMethod, IUsing
    {
        public string Name { get; set; } = "ASSIGN";
        public string Path { get; set; } = "Variables";

        public void LateExecute(string[] para)
        {
            if (VariableSetup.globalVar.ContainsKey(para[0]))
            {
                switch (VariableSetup.globalVar[para[0]].type) 
                {
                    case VariableSetup.VariableTypes.String:
                        VariableSetup.AddEntry(para[0], VariableSetup.VariableTypes.String, Encoding.UTF8.GetBytes(TextMethods.ParseText(para, 2, '[', ']')));
                        break;

                    case VariableSetup.VariableTypes.Int:
                        try
                        {
                            double maths = Maths.Evaluate(TextMethods.CalcString(String.Join(" ", VariableSetup.Convert(para, 2), 2, para.Length - 2), '(', ')'));
                            VariableSetup.AddEntry(para[0], VariableSetup.VariableTypes.Int, BitConverter.GetBytes(Convert.ToInt32(maths)));
                        }
                        catch
                        {
                            VariableSetup.AddEntry(para[0], VariableSetup.VariableTypes.Int, BitConverter.GetBytes(Convert.ToInt32(para[2])));
                        }
                        break;

                    case VariableSetup.VariableTypes.Boolean:
                        try
                        {
                            VariableSetup.AddEntry(para[0], VariableSetup.VariableTypes.Boolean, BitConverter.GetBytes(Convert.ToBoolean(para[2])));
                        }
                        catch
                        {
                            Console.WriteLine("Unsupported Bool Type");
                        }
                        break;

                    default:
                        Console.WriteLine("Variable Type Not Supported");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Variable: '" + para[0] + "' Does Not Exist.");
            }
        }
    }
}
