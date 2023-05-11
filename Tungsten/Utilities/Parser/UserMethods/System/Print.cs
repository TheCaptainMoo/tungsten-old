﻿using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using static Tungsten_Interpreter.Utilities.AST.AbstractSyntaxTree;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class Print : ILexer
    {
        public string Name { get; set; } = "PRINT";
        //public string Path { get; set; } = "System";
        public Regex RegexCode { get; set; } = new Regex(@"^print$|WSprint");

        public AstNode AstConstructor(string[] para)
        {
            //VariableSetup.nodes.Add(new PrintNode(new StringAnalysisNode(para, 1)));

            // Text Methods Function To Make Para Into ASTNodes


            return new PrintNode(new StringAnalysisNode(TextMethods.StringAstParse(para, 1)));
        }

        //TextMethods.ParseText(para, 1, '[', ']')

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
}