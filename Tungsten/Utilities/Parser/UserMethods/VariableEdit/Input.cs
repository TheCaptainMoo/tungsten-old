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
                    return new InputNode(VariableSetup.VariableTypes.String, para[2]);

                case "INT":
                    return new InputNode(VariableSetup.VariableTypes.Int, para[2]);

                case "BOOL":
                    return new InputNode(VariableSetup.VariableTypes.Boolean, para[2]); ;

                default:
                    ErrorHandling.Alert("Value type not supported at: " + String.Join(" ", para), ConsoleColor.Red);
                    return null;
            }
        }

        public class InputNode : AstNode
        {
            public InputNode(VariableSetup.VariableTypes type, string name) 
            {
                Type = type;
                Name = name;
            }

            public override object? Execute()
            {
                switch (Type)
                {
                    case VariableSetup.VariableTypes.String:
                        VariableSetup.AddEntry(Name, Type, Encoding.UTF8.GetBytes(Console.ReadLine()));
                        break;

                    case VariableSetup.VariableTypes.Int:
                        VariableSetup.AddEntry(Name, Type, BitConverter.GetBytes(int.Parse(Console.ReadLine())));
                        break;

                    case VariableSetup.VariableTypes.Boolean:
                        VariableSetup.AddEntry(Name, Type, BitConverter.GetBytes(Convert.ToBoolean(Console.ReadLine())));
                        break;
                }
                return null;
            }

            public string Name { get; set; }
            public VariableSetup.VariableTypes Type { get; set; }
        }
    }
}