using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;
using static Tungsten_Interpreter.Utilities.AST.AbstractSyntaxTree;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class VariableUpdate : ILexer
    {
        public string Name { get; set; } = "UPDATE";
        public Regex RegexCode { get; set; } = new Regex(@"^update$|WSupdate");

        public AstNode AstConstructor(string[] para)
        {
            switch (para[1])
            {
                case "STRING":
                    return new VariableNodedAssignNode(VariableSetup.VariableTypes.String, para[2], TextMethods.StringAstParse(para, 4));

                case "INT":
                    return new VariableNodedAssignNode(VariableSetup.VariableTypes.Int, para[2], TextMethods.IntAstParse(para, 4));

                case "BOOL":
                    return new VariableAssignNode(VariableSetup.VariableTypes.Boolean, para[2], BitConverter.GetBytes(Convert.ToBoolean(para[4])));

                default:
                    // Error Generation - Variable Type Not Supported
                    return null;
            }
        }
    }
}