using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Parser.UserMethods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.AST
{
    public static class AbstractSyntaxTree
    {
        public abstract class AstNode
        {
            public abstract object? Execute();
        }

        public class NumberNode : AstNode
        {
            public NumberNode(int value)
            {
                //Type = AstNodeType.Number;
                Value = value;
            }

            public override object? Execute()
            {
                return null;
            }

            public int Value { get; set; }
        }

        public class StringAnalysisNode : AstNode
        {
            public StringAnalysisNode(string[] value, int startIndex)
            {
                Value = value;
                StartIndex = startIndex;
            }

            public override object Execute()
            {
                return TextMethods.ParseText(Value, StartIndex, '[', ']');
            }

            public string[] Value { get; set; }
            public int StartIndex { get; set; }
        }

        

        public class ConditionNode : AstNode
        {
            public ConditionNode(dynamic leftStatement, string condition, dynamic rightStatement)
            {
                LeftStatement = leftStatement;
                Condition = condition;
                RightStatement = rightStatement;
            }

            public override object? Execute()
            {
                try
                {
                    LeftStatement = VariableSetup.Convert(LeftStatement);
                    RightStatement = VariableSetup.Convert(RightStatement);
                }
                catch { }

                return Check.Operation(LeftStatement, Condition, RightStatement);
            }

            public dynamic LeftStatement { get; set; }
            public string Condition { get; set; }
            public dynamic RightStatement { get; set;}
        }

        public class VariableAssignNode : AstNode
        {
            public VariableAssignNode(VariableSetup.VariableTypes type, string name, byte[] value)
            {
                Type = type;
                Name = name;
                Value = value;
            }

            public override object? Execute()
            {
                /*switch (Type)
                {
                    case VariableSetup.VariableTypes.Typeless:
                        Console.WriteLine("Typeless");
                        break;

                    case VariableSetup.VariableTypes.String:
                        VariableSetup.globalVar.Add(Name, new VariableSetup.Variable(Type, Value));
                        break;

                    case VariableSetup.VariableTypes.Int:
                        Console.WriteLine("Int");
                        break;

                    case VariableSetup.VariableTypes.Boolean:
                        Console.WriteLine("Boolean");
                        break;

                    case VariableSetup.VariableTypes.Matrix:
                        Console.WriteLine("Matrix");
                        break;
                        
                    default: 
                        // Throw Error
                        break;
                }*/

                VariableSetup.globalVar.Add(Name, new VariableSetup.Variable(Type, Value));

                return null;
            }

            public VariableSetup.VariableTypes Type { get; set; }
            public string Name { get; set; }
            public byte[] Value { get; set; }
        }

        public struct LinedAstReturn
        {
            public LinedAstReturn(int returnIndex, AstNode returnNode)
            {
                ReturnIndex = returnIndex;
                ReturnNode = returnNode;
            }

            public int ReturnIndex { get; set; }
            public AstNode ReturnNode { get; set; }
        }

        // Program Start - Acts As The Root
        public class Program : AstNode
        {
            public Program(List<AstNode> children) 
            {
                Children = children;
            }

            public override object? Execute()
            {
                for(int i = 0; i < Children.Count; i++)
                {
                    return Children[i].Execute();
                }
                return null;
            }

            public List<AstNode> Children { get; set; }
        }
    }
}
