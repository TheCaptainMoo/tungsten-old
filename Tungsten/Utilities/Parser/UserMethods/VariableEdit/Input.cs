using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class VariableInput : IMethod, IUsing
    {
        public string Name { get; set; } = "INPUT";
        public string Path { get; set; } = "Variables";

        // Allows 'ReadLine' Input from Users
        public void Execute(string[] para)
        {
            switch (para[1])
            {
                case "STRING":
                    #region String Input
                    string[] inputStr = Console.ReadLine().Split(" ");
                    inputStr[0] = "[" + inputStr[0];
                    inputStr[inputStr.Length - 1] = inputStr[inputStr.Length - 1] + "]";
                    VariableSetup.AddEntry(para[2], TextMethods.ParseText(inputStr, 0, '[', ']'));
                    #endregion
                    break;

                case "INT":
                    #region Int Input
                    string[] inputInt = Console.ReadLine().Split(" ");
                    try
                    {
                        double maths = Maths.Evaluate(TextMethods.CalcString(String.Join(" ", inputInt, 0, inputInt.Length), '(', ')'));
                        VariableSetup.AddEntry(para[2], Convert.ToInt32(maths));
                    }
                    catch
                    {
                        VariableSetup.AddEntry(para[2], Convert.ToInt32(inputInt[0]));
                    }
                    #endregion
                    break;

                case "BOOL":
                    #region Bool Input
                    string inputBool = Console.ReadLine();
                    try
                    {
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