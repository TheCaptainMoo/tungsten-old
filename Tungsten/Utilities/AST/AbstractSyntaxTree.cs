using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public class IfStatementNode : AstNode
        {
            public IfStatementNode(AstNode condition, AstNode thenStatement)
            {
                //Type = AstNodeType.IfStatement;
                Condition = condition;
                ThenStatement = thenStatement;
            }

            public override object? Execute()
            {
                return null;
            }

            public AstNode Condition { get; set; }
            public AstNode ThenStatement { get; set; }
        }

        public class PrintNode : AstNode 
        {
            public PrintNode(string value)
            {
                Value = value;
            }

            public override object? Execute()
            {
                Console.WriteLine(Value);
                return null;
            }

            public string Value { get; set; }
        }

        public class VariableAssignNode : AstNode
        {
            public VariableAssignNode(VariableSetup.VariableTypes type, string name, byte value)
            {
                Type = type;
                Name = name;
                Value = value;
            }

            public override object? Execute()
            {
                switch (Type)
                {
                    case VariableSetup.VariableTypes.Typeless:
                        Console.WriteLine("Typeless");
                        break;

                    case VariableSetup.VariableTypes.String:
                        Console.WriteLine("String");
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
                }

                return null;
            }

            public VariableSetup.VariableTypes Type { get; set; }
            public string Name { get; set; }
            public byte Value { get; set; }
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
