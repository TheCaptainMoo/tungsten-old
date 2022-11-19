using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class VariableInput : IMethod
    {
        public string Name { get; set; } = "INPUT";

        public void Execute(string[] para)
        {
            switch (para[1])
            {
                case "STRING":
                    #region String Input
                    //if (Exist(para, variableString, 2))
                    //return;

                    string[] inputStr = Console.ReadLine().Split(" ");
                    inputStr[0] = "[" + inputStr[0];
                    inputStr[inputStr.Length - 1] = inputStr[inputStr.Length - 1] + "]";

                    //variableString.Add(para[2], ParseText(inputStr, 0, '[', ']'));
                    VariableSetup.AddEntry(para[2], TextMethods.ParseText(inputStr, 0, '[', ']'));
                    #endregion
                    break;

                case "INT":
                    #region Int Input
                    //if (Exist(para, variableInt, 2))
                    //return;

                    string[] inputInt = Console.ReadLine().Split(" ");
                    try
                    {
                        double maths = Maths.Evaluate(TextMethods.CalcString(String.Join(" ", inputInt, 0, inputInt.Length), '(', ')'));
                        //variableInt.Add(para[2], Convert.ToInt32(maths));
                        VariableSetup.AddEntry(para[2], Convert.ToInt32(maths));
                    }
                    catch
                    {
                        //variableInt.Add(para[2], Convert.ToInt32(inputInt[0]));
                        VariableSetup.AddEntry(para[2], Convert.ToInt32(inputInt[0]));
                    }
                    #endregion
                    break;

                case "BOOL":
                    #region Bool Input
                    //if (Exist(para, variableBool, 2))
                    //return;

                    string inputBool = Console.ReadLine();
                    try
                    {
                        //variableBool.Add(para[2], Convert.ToBoolean(inputBool));
                        VariableSetup.AddEntry(para[2], Convert.ToBoolean(inputBool));
                    }
                    catch
                    {
                        Console.WriteLine("Unsupported Bool Type");
                    }
                    #endregion
                    break;

                default:
                    Console.WriteLine("Unrecognised Type: {0}", para[1]);
                    break;
            }
        }
    }
}