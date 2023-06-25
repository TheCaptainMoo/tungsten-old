using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Tungsten_Interpreter.Utilities.AST.AbstractSyntaxTree;
using Tungsten_Interpreter.Utilities.Parser.Methods;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods.System
{
    internal class Return : ILexer
    {
        public string Name { get; set; } = "RETURN";
        public Regex RegexCode { get; set; } = new Regex(@"^return$|WSreturn");
        public AstNode AstConstructor(string[] para)
        {
            return new ReturnNode(new StringAnalysisNode(TextMethods.StringAstParse(para, 1)));
        }

        public class ReturnNode : AstNode 
        {
            public ReturnNode(AstNode value) 
            {
                Value = value;
            }

            public override object? Execute()
            {
                return Encoding.UTF8.GetBytes((string)Value.Execute());
            }

            public AstNode Value { get; set; }
        }
    }
}
