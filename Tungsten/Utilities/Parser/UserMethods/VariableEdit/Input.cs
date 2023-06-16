/*using System.Text;
using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class VariableInput : IMethod, IUsing, ILexer
    {
        public string Name { get; set; } = "INPUT";
        public string Path { get; set; } = "Variables";
        public Regex RegexCode { get; set; } = new Regex(@"^input$|WSinput|=>");

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
                    //VariableSetup.AddEntry(para[2], TextMethods.ParseText(inputStr, 0, '[', ']'));
                    VariableSetup.AddEntry(para[2], VariableSetup.VariableTypes.String, Encoding.UTF8.GetBytes(TextMethods.ParseText(inputStr, 0, '[', ']')));
                    #endregion
                    break;

                case "INT":
                    #region Int Input
                    string[] inputInt = Console.ReadLine().Split(" ");
                    try
                    {
                        double maths = Maths.Evaluate(TextMethods.CalcString(String.Join(" ", inputInt, 0, inputInt.Length), '(', ')'));
                        //VariableSetup.AddEntry(para[2], Convert.ToInt32(maths));
                        VariableSetup.AddEntry(para[2], VariableSetup.VariableTypes.Int, BitConverter.GetBytes(maths));
                    }
                    catch
                    {
                        //VariableSetup.AddEntry(para[2], Convert.ToInt32(inputInt[0]));
                        VariableSetup.AddEntry(para[2], VariableSetup.VariableTypes.Int, BitConverter.GetBytes(Convert.ToInt32(inputInt[0])));
                    }
                    #endregion
                    break;

                case "BOOL":
                    #region Bool Input
                    string inputBool = Console.ReadLine();
                    try
                    {
                        //VariableSetup.AddEntry(para[2], Convert.ToBoolean(inputBool));
                        VariableSetup.AddEntry(para[2], VariableSetup.VariableTypes.Boolean, BitConverter.GetBytes(Convert.ToBoolean(inputBool)));
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
}*/

using System.Text;
using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;
using static Tungsten_Interpreter.Utilities.AST.AbstractSyntaxTree;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class VariableInput : ILexer
    {
        public string Name { get; set; } = "INPUT";
        public Regex RegexCode { get; set; } = new Regex(@"^input$|WSinput|=>");

        public AstNode AstConstructor(string[] para)
        {
            switch (para[1])
            {
                case "STRING":
                    return new VariableNodedAssignNode(VariableSetup.VariableTypes.String, para[2], TextMethods.StringAstParse(Console.ReadLine().Split(" "), 4));

                case "INT":
                    return new VariableNodedAssignNode(VariableSetup.VariableTypes.Int, para[2], TextMethods.IntAstParse(Console.ReadLine().Split(" "), 4));

                case "BOOL":
                    return new VariableAssignNode(VariableSetup.VariableTypes.Boolean, para[2], BitConverter.GetBytes(Convert.ToBoolean(Console.ReadLine())));

                default:
                    // Error Generation - Variable Type Not Supported
                    ErrorHandling.Alert("Value type not supported at: " + String.Join(" ", para), ConsoleColor.Red);
                    return null;
            }
        }
    }
}