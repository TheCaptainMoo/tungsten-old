using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using static Tungsten_Interpreter.Utilities.AST.AbstractSyntaxTree;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class Print : ILexer
    {
        public string Name { get; set; } = "PRINT";
        public Regex RegexCode { get; set; } = new Regex(@"^print$|WSprint");

        public AstNode AstConstructor(string[] para)
        {
            return new PrintNode(new StringAnalysisNode(TextMethods.StringAstParse(para, 1)));
        }

        public class PrintNode : AstNode
        {
            public PrintNode(StringAnalysisNode value)
            {
                Value = value;
            }

            public override object? Execute()
            {
                Console.WriteLine(Value.Execute());
                return null;
            }

            public StringAnalysisNode Value { get; set; }
        }
    }

    public class PrintInline : ILexer
    {
        public string Name { get; set; } = "PRINTIN";
        public Regex RegexCode { get; set; } = new Regex(@"^printin$|WSprintin|^PRINTin$|WSPRINTin");

        public AstNode AstConstructor(string[] para)
        {
            return new PrintInlineNode(new StringAnalysisNode(TextMethods.StringAstParse(para, 1)));
        }

        public class PrintInlineNode : AstNode
        {
            public PrintInlineNode(StringAnalysisNode value)
            {
                Value = value;
            }

            public override object? Execute()
            {
                Console.Write(Value.Execute());
                return null;
            }

            public StringAnalysisNode Value { get; set; }
        }
    }
}