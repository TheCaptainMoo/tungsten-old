using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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

        public class StringAnalysisNode : AstNode
        {
            /*public StringAnalysisNode(string[] value, int startIndex)
            {
                Value = value;
                StartIndex = startIndex;
            }

            public override object Execute()
            {
                //return TextMethods.ParseText(Value, StartIndex, '[', ']');
                return TextMethods.AstParse(Value, StartIndex);
            }

            public string[] Value { get; set; }
            public int StartIndex { get; set; }*/

            public StringAnalysisNode(List<AstNode> values)
            {
                Values = values;
            }

            public override object? Execute()
            {
                StringBuilder sb = new StringBuilder();

                for(int i = 0; i < Values.Count; i++)
                {
                    sb.Append(Values[i].Execute());
                }

                return sb.ToString();
            }

            public List<AstNode> Values { get; set; }
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
                VariableSetup.globalVar.Add(Name, new VariableSetup.Variable(Type, Value));

                return null;
            }

            public VariableSetup.VariableTypes Type { get; set; }
            public string Name { get; set; }
            public byte[] Value { get; set; }
        }

        public class VariableNodedAssignNode : AstNode
        {
            public VariableNodedAssignNode(VariableSetup.VariableTypes type, string name, List<AstNode> value)
            {
                Type = type;
                Name = name;
                Value = value;
            }

            public override object? Execute()
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < Value.Count; i++)
                {
                    sb.Append(Value[i].Execute());
                }

                VariableSetup.globalVar.Add(Name, new VariableSetup.Variable(Type, Encoding.UTF8.GetBytes(sb.ToString())));

                return null;
            }

            public VariableSetup.VariableTypes Type { get; set; }
            public string Name { get; set; }
            public List<AstNode> Value { get; set; }
        }

        public class VariableNode : AstNode
        {
            public VariableNode(string name)
            {
                Name = name;
            }

            public override object? Execute()
            {
                return System.Text.Encoding.UTF8.GetString(VariableSetup.globalVar[Name].data.Span);
            }

            public string Name { get; set; }
        }

        public class ValueNode : AstNode
        {
            public ValueNode(byte[] value)
            {
                Value = value;
            }

            public override object? Execute()
            {
                return System.Text.Encoding.UTF8.GetString(Value);
            }

            public byte[] Value { get; set; }
        }

        /*public class PropertyNode : AstNode
        {
            public PropertyNode()
            {

            }

            public override object? Execute()
            {

            }

            public string Name { get; set; }
            public string[] Arguments { get; set; }

        }*/

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
