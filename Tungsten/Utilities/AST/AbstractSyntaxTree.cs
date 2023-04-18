using System.Text;
using Tungsten_Interpreter.Utilities.Parser.Methods;
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
            public StringAnalysisNode(List<AstNode> values)
            {
                Values = values;
            }

            public override object? Execute()
            {
                StringBuilder sb = new StringBuilder();

                for(int i = 0; i < Values.Count; i++)
                {
                    if (Values[i] is VariableNode)
                    {
                        VariableNode vn = (VariableNode)Values[i];
                        Memory<byte> memory = (Memory<byte>)Values[i].Execute();

                        switch (VariableSetup.globalVar[vn.Name].type)
                        {
                            case VariableSetup.VariableTypes.String:
                                sb.Append(Encoding.UTF8.GetString(memory.Span));
                                break;

                            case VariableSetup.VariableTypes.Int:
                                sb.Append(BitConverter.ToInt32(memory.Span).ToString());
                                break;
                        }
                    }
                    else
                    {
                        sb.Append(Encoding.UTF8.GetString((byte[])Values[i].Execute()));
                    }
                }

                return sb.ToString();
            }

            public List<AstNode> Values { get; set; }
        }

        public class ConditionNode : AstNode
        {
            public ConditionNode(AstNode leftStatement, string condition, AstNode rightStatement)
            {
                LeftStatement = leftStatement;
                Condition = condition;
                RightStatement = rightStatement;
            }

            public override object? Execute()
            {
                byte[] ls;
                byte[] rs;

                if (LeftStatement is VariableNode)
                {
                    Memory<byte> memory = (Memory<byte>)LeftStatement.Execute();
                    ls = memory.Span.ToArray();
                }
                else
                {
                    try
                    {
                        ls = BitConverter.GetBytes(Convert.ToInt32(Encoding.UTF8.GetString((byte[])LeftStatement.Execute())));
                    }
                    catch
                    {
                        ls =(byte[])LeftStatement.Execute();
                    }
                }

                if (RightStatement is VariableNode)
                {
                    Memory<byte> memory = (Memory<byte>)RightStatement.Execute();
                    rs = memory.Span.ToArray();
                }
                else
                {
                    try
                    {
                        rs = BitConverter.GetBytes(Convert.ToInt32(Encoding.UTF8.GetString((byte[])RightStatement.Execute())));
                    }
                    catch
                    {
                        rs = (byte[])RightStatement.Execute();
                    }
                }

                return Check.Operation(ls, Condition, rs);
            }

            public AstNode LeftStatement { get; set; }
            public string Condition { get; set; }
            public AstNode RightStatement { get; set; }
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
                VariableSetup.AddEntry(Name, Type, Value);
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
                switch (Type) {
                    case VariableSetup.VariableTypes.String:
                        List<byte[]> bytes = new List<byte[]>();

                        for (int i = 0; i < Value.Count; i++)
                        {
                            if (Value[i] is VariableNode)
                            {
                                Memory<byte> memory = (Memory<byte>)Value[i].Execute();
                                bytes.Add(memory.Span.ToArray());
                                continue;
                            }

                            bytes.Add((byte[])Value[i].Execute());
                        }

                        byte[] output = bytes.SelectMany(bytes => bytes).ToArray();

                        VariableSetup.AddEntry(Name, Type, output);
                        break;

                    case VariableSetup.VariableTypes.Int:
                        StringBuilder sb1 = new StringBuilder();

                        for (int i = 0; i < Value.Count; i++)
                        {
                            if (Value[i] is VariableNode)
                            {
                                Memory<byte> memory = (Memory<byte>)Value[i].Execute();
                                sb1.Append(BitConverter.ToInt32(memory.Span).ToString());
                                continue;
                            }

                            sb1.Append(Encoding.UTF8.GetString((byte[])Value[i].Execute()));
                        }

                        VariableSetup.AddEntry(Name, Type, BitConverter.GetBytes((int)new Maths(sb1.ToString()).Execute()));
                        break;

                    case VariableSetup.VariableTypes.Boolean:
                        break;
                }


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
                return VariableSetup.globalVar[Name].data;
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
                return Value;
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
