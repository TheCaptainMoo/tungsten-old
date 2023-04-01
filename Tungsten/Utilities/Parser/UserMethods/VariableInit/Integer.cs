using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.AST;
using Tungsten_Interpreter.Utilities.Parser.Methods;

using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class IntegerCreation : ILexer
    {
        public string Name { get; set; } = "INT";
        public Regex RegexCode { get; set; } = new Regex(@"^int$|WSint");

        public AbstractSyntaxTree.AstNode AstConstructor(string[] para)
        {
            try
            {
                double maths = Maths.Evaluate(TextMethods.CalcString(String.Join(" ", VariableSetup.Convert(para, 3), 1, para.Length - 1), '(', ')'));
                //VariableSetup.nodes.Add(new AST.AbstractSyntaxTree.VariableAssignNode(VariableSetup.VariableTypes.Int, para[1], BitConverter.GetBytes(Convert.ToInt32(maths))));
                return new AbstractSyntaxTree.VariableAssignNode(VariableSetup.VariableTypes.Int, para[1], BitConverter.GetBytes(Convert.ToInt32(maths)));
            }
            catch
            {
                //VariableSetup.nodes.Add(new AbstractSyntaxTree.VariableAssignNode(VariableSetup.VariableTypes.Int, para[1], BitConverter.GetBytes(Convert.ToInt32(para[3]))));
                return new AbstractSyntaxTree.VariableAssignNode(VariableSetup.VariableTypes.Int, para[1], BitConverter.GetBytes(Convert.ToInt32(para[3])));
            }
        }
    }
}