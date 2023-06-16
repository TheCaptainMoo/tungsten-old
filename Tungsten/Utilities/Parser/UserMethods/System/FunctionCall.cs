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
        public Regex RegexCode { get; set; } = new Regex(@"^call$|WScall|\|>");

        public AstNode AstConstructor(string[] para)
        {
            if (Program.callableFunctions.ContainsKey(para[1]))
            {
                return new CallableFunctionNode(para[1], TextMethods.ParameterAstParse(para, 2));
            }
            else if (VariableSetup.functions.ContainsKey(para[1]))
            {
                return new FunctionCallNode(para[1], TextMethods.ParameterAstParse(para, 2));
            }
            else
            {
                // Variable Call Node
                return null;
            }
        }

        public class CallableFunctionNode : AstNode
        {
            public CallableFunctionNode(string name, List<AstNode> parameters)
            {
                Name = name;
                Parameters = parameters;
            }
            
            public override object? Execute()
            {
                Program.callableFunctions[Name].Function(Parameters);
                return null;
            }

            string Name { get; set; }
            List<AstNode> Parameters { get; set; }
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
                ProcessAstNode();

                for (int i = 0; i < FunctNode.Body.Count; i++)
                {
                    FunctNode.Body[i].Execute();
                }

                for (int i = 0; i < Parameters.Count; i++)
                {
                    VariableSetup.RemoveEntry(FunctNode.Parameters[i].Name);
                }

                return null;
            }

            private void ProcessAstNode()
            {
                for(int i = 0; i < FunctNode.Parameters.Count; i++)
                {
                    switch (FunctNode.Parameters[i].Type) {
                        case VariableSetup.VariableTypes.String:
                            VariableSetup.AddEntry(FunctNode.Parameters[i].Name, FunctNode.Parameters[i].Type, ByteManipulation.GetValue(Parameters[i]));
                            break;

                        case VariableSetup.VariableTypes.Int:
                            VariableSetup.AddEntry(FunctNode.Parameters[i].Name, FunctNode.Parameters[i].Type, ByteManipulation.CharsToInt(ByteManipulation.GetValue(Parameters[i])));
                            break;

                    }
                    
                }
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