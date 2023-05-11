using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;
using static Tungsten_Interpreter.Utilities.AST.AbstractSyntaxTree;
using static Tungsten_Interpreter.Utilities.Parser.UserMethods.Print;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class CallLiteral : ILexer
    {
        public string Name { get; set; } = "CALL_LITERAL";
        public Regex RegexCode { get; set; } = new Regex(@"^call$|WScall");

        public AstNode AstConstructor(string[] para)
        {
            if (VariableSetup.functions.ContainsKey(para[1]))
            {
                return new FunctionCallNode(para[1], TextMethods.ParameterAstParse(para, 2));
            }
            else
            {
                // Variable Call Node
                return null;
            }
        }

        public class FunctionCallNode : AstNode
        {
            public FunctionCallNode(string name, List<AstNode> parameters)
            {
                Name = name;
                Parameters = parameters;

                FunctNode = VariableSetup.functions[name];
            }

            public override object? Execute()
            {
                for (int i = 0; i < FunctNode.Body.Count; i++)
                {
                    ProcessAstNode(FunctNode.Body[i]).Execute();
                }

                for (int i = 0; i < Parameters.Count; i++)
                {
                    VariableSetup.RemoveEntry(FunctNode.Parameters[i].Name);
                }

                return null;
            }

            private AstNode ProcessAstNode(AstNode node)
            {
                if (node is VariableNode)
                {
                    VariableNode variableNode = (VariableNode)node;
                    for (int i = 0; i < FunctNode.Parameters.Count; i++)
                    {
                        if (FunctNode.Parameters[i].Name == variableNode.Name)
                        {
                            //return new ValueNode((byte[])Parameters[i].Execute());
                            try
                            {
                                VariableSetup.AddEntry(FunctNode.Parameters[i].Name, FunctNode.Parameters[i].Type, (byte[])Parameters[i].Execute());
                            }
                            catch
                            {
                                Memory<byte> memory = (Memory<byte>)Parameters[i].Execute();
                                VariableSetup.AddEntry(FunctNode.Parameters[i].Name, FunctNode.Parameters[i].Type, memory.Span.ToArray());
                            }
                        }
                    }
                }
                else if (node is StringAnalysisNode analysisNode)
                {
                    for (int i = 0; i < analysisNode.Values.Count; i++)
                    {
                        analysisNode.Values[i] = ProcessAstNode(analysisNode.Values[i]);
                    }
                }
                else if (node is PrintNode printNode)
                {
                    printNode.Value = (StringAnalysisNode)ProcessAstNode(printNode.Value);
                }

                return node;
            }

            public string Name { get; set; }
            public List<AstNode> Parameters { get; set; }
            public Function.FunctionNode FunctNode { get; set; }
        }

        /*public class VariableCallNode : AstNode
        {

        }*/
    }
}