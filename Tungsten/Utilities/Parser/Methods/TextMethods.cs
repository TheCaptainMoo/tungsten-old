using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.UserMethods;
using Tungsten_Interpreter.Utilities.Variables;
using static Tungsten_Interpreter.Utilities.AST.AbstractSyntaxTree;

namespace Tungsten_Interpreter.Utilities.Parser.Methods
{
    public class TextMethods
    {
        // Gets a Substring Between 2 Characters
        /// <summary>
        /// [Runtime]
        /// </summary>
        public static string CalcString(string input, char openChar, char closeChar)
        {
            int startPos = 0;
            int endPos = input.Length;

            for (int i = 0; i < endPos; i++)
            {
                if (input[i] == openChar)
                {
                    startPos = i + 1;
                    break;
                }
            }

            for (int i = endPos - 1; i >= 0; i--)
            {
                if (input[i] == closeChar)
                {
                    endPos = i - startPos;
                    break;
                }
            }

            return input.Substring(startPos, endPos);
        }

        // Gets a Substring Between 2 Characters In A Forward Motion
        /// <summary>
        /// [Runtime]
        /// </summary>
        public static string CalcStringForward(string input, char openChar, char closeChar)
        {
            int startPos = 0;
            int endPos = input.Length;

            for (int i = 0; i < endPos; i++)
            {
                if (input[i] == openChar)
                {
                    startPos = i + 1;
                    break;
                }
            }

            for (int i = 0; i < endPos; i++)
            {
                if (input[i] == closeChar)
                {
                    endPos = i - startPos;
                    break;
                }
            }

            return input.Substring(startPos, endPos);
        }

        // Parses Text Array Between 2 Characters
        /// <summary>
        /// [Runtime]
        /// </summary>
        public static string ParseText(string[] words, int startIndex, char startsWith, char endsWith)
        {
            StringBuilder sb = new StringBuilder();

            for (int j = startIndex; j < words.Length; j++)
            {
                if (words[j] == null)
                    continue;

                if (words[j].StartsWith(startsWith))
                {
                    sb.Append(TextMethods.CalcStringForward(String.Join(" ", words, j, words.Length - j), startsWith, endsWith));
                }
                else if (VariableSetup.globalVar.ContainsKey(words[j]) || VariableSetup.globalVar.ContainsKey(Regex.Replace(words[j], @"<[0-9]+>|<[a-zA-Z]+>", "")))
                {
                    sb.Append(VariableSetup.Convert(words[j]));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// [Gentime]
        /// </summary>
        public static List<AstNode> StringAstParse(string[] para, int startIndex)
        {
            List<AstNode> nodes = new List<AstNode>();
            bool insideString = false;
            bool stringProtection = false;

            for (int i = startIndex; i < para.Length; i++)
            {
                if (para[i].StartsWith('['))
                {
                    insideString = true;
                }
                else if (para[i].EndsWith(']'))
                {
                    insideString = false;
                    stringProtection = false;
                    continue;
                }

                if (!insideString)
                {
                    nodes.Add(new VariableNode(para[i]));
                }
                else if (stringProtection == false)
                {
                    nodes.Add(new ValueNode(Encoding.UTF8.GetBytes(CalcStringForward(String.Join(" ", para, i, para.Length - i), '[', ']'))));
                    stringProtection = true;
                }
            }

            return nodes;
        }

        /// <summary>
        /// [Gentime]
        /// </summary>
        public static List<AstNode> IntAstParse(string[] para, int startIndex)
        {
            List<AstNode> nodes = new List<AstNode>();

            for (int i = startIndex; i < para.Length; i++)
            {
                if (Regex.IsMatch(para[i], @"^[0-9]+$"))
                {
                    //nodes.Add(new AST.AbstractSyntaxTree.ValueNode(BitConverter.GetBytes(Convert.ToInt32(para[i]))));
                    nodes.Add(new ValueNode(Encoding.UTF8.GetBytes(para[i])));
                }
                else if (para[i] == "+" || para[i] == "-" || para[i] == "(" || para[i] == ")" || para[i] == "*" || para[i] == "/")
                {
                    nodes.Add(new ValueNode(Encoding.UTF8.GetBytes(para[i])));
                }
                else
                {
                    nodes.Add(new VariableNode(para[i]));
                }
            }

            return nodes;
        }

        /// <summary>
        /// [Gentime]
        /// </summary>
        public static List<AstNode> GenericAstParse(string[] para, int startIndex)
        {
            List<AstNode> nodes = new List<AstNode>();
            bool insideString = false;


            for (int i = startIndex; i < para.Length; i++)
            {
                // Strings
                if (para[i].StartsWith('['))
                {
                    insideString = true;
                }
                else if (para[i].EndsWith(']'))
                {
                    insideString = false;
                    continue;
                }


                if (insideString)
                {
                    nodes.Add(new ValueNode(Encoding.UTF8.GetBytes(CalcStringForward(String.Join(" ", para, i, para.Length - i), '[', ']'))));
                }
                else if (para[i].ToLower() == "true" || para[i].ToLower() == "false")
                {
                    nodes.Add(new ValueNode(BitConverter.GetBytes(Convert.ToBoolean(para[i]))));
                }
                // Integers
                else if (Regex.IsMatch(para[i], @"^[0-9]+$"))
                {
                    nodes.Add(new ValueNode(Encoding.UTF8.GetBytes(para[i])));
                }
                else if (para[i] == "==" || para[i] == "<=" || para[i] == ">=" || para[i] == "<" || para[i] == ">" || para[i] == "!=")
                {
                    continue;
                }
                else
                {
                    nodes.Add(new VariableNode(para[i]));
                }
            }

            return nodes;
        }

        public static List<AstNode> ParameterAstParse(string[] para, int startIndex)
        {
            List<AstNode> nodes = new List<AstNode>();

            bool insideString = false;
            bool stringProtection = false;

            for (int i = startIndex; i < para.Length; i++)
            {
                string word = para[i].Replace("<", "").Replace(">", "");

                if (word.StartsWith('['))
                {
                    insideString = true;
                    stringProtection = false;
                }
                else if (word.EndsWith(']'))
                {
                    insideString = false;
                    stringProtection = false;
                    continue;
                }

                if (!insideString)
                {
                    if (word.Length > 0)
                        nodes.Add(new VariableNode(word));
                }
                else if (stringProtection == false)
                {
                    nodes.Add(new ValueNode(Encoding.UTF8.GetBytes(CalcStringForward(String.Join(" ", para, i, para.Length - i), '[', ']'))));
                    stringProtection = true;
                }
            }

            return nodes;
        }

        public static ContextualReturn NewParameterAstParse(string[] para, int startIndex)
        {
            List<AstNode> nodes = new List<AstNode>();
            int i;

            bool insideString = false;
            bool stringProtection = false;
            string[] contents = CalcStringForward(String.Join(' ', para, startIndex, para.Length - startIndex), '<', '>').Split(' ');

            for (i = 0; i < contents.Length; i++)
            {
                //string word = para[i].Replace("<", "").Replace(">", "");
                string word = contents[i];

                if (word.StartsWith('['))
                {
                    insideString = true;
                    stringProtection = false;
                }
                else if (word.EndsWith(']'))
                {
                    insideString = false;
                    stringProtection = false;
                    continue;
                }

                if (!insideString)
                {
                    if (word.Length > 0)
                        nodes.Add(new VariableNode(word));
                }
                else if (stringProtection == false)
                {
                    nodes.Add(new ValueNode(Encoding.UTF8.GetBytes(CalcStringForward(String.Join(" ", contents, i, contents.Length - i), '[', ']'))));
                    stringProtection = true;
                }
            }

            return new ContextualReturn(nodes, i);
        }

        public static ContextualReturn NewStringAstParse(string[] para, int startIndex)
        {
            List<AstNode> nodes = new List<AstNode>();
            int output = 0;
            bool stringProtection = false;

            for (int i = startIndex; i < para.Length; i++)
            {
                if (stringProtection == false)
                {
                    nodes.Add(new ValueNode(Encoding.UTF8.GetBytes(CalcStringForward(String.Join(" ", para, i, para.Length - i), '[', ']'))));
                    stringProtection = true;
                }

                if (para[i].EndsWith(']'))
                {
                    stringProtection = false;
                    output = i;
                    break;
                }
            }

            return new ContextualReturn(nodes, output);
        }

        public static ContextualReturn BooleanAstParse(string[] para, int startIndex)
        {
            List<AstNode> nodes = new List<AstNode>();
            int output = 0;

            for (int i = startIndex; i < para.Length; i++)
            {
                if (para[i].StartsWith('['))
                {
                    var temp = NewStringAstParse(para, i);
                    for (int j = 0; j < temp.Nodes.Count; j++)
                    {
                        nodes.Add(temp.Nodes[j]);
                    }
                    i = temp.ExitPosition;
                }
                else if (Regex.IsMatch(para[i], @"[0-9]|==|!=|<|>|<=|>="))
                {
                    nodes.Add(new ValueNode(Encoding.UTF8.GetBytes(para[i])));
                }
                else
                {
                    nodes.Add(new VariableNode(para[i]));
                }
            }

            return new ContextualReturn(nodes, output);
        }

        public static List<AstNode> AstParse(string[] para, int startIndex, VariableSetup.VariableTypes type)
        {
            List<AstNode> nodes = new List<AstNode>();

            switch (type)
            {
                case VariableSetup.VariableTypes.String:
                    for (int i = startIndex; i < para.Length; i++)
                    {
                        if (para[i] == "CALL_LITERAL")
                        {
                            var temp = NewParameterAstParse(para, i);
                            for (int j = 0; j < temp.Nodes.Count; j++)
                            {
                                nodes.Add(new CallLiteral.FunctionCallNode(para[i + 1], temp.Nodes));
                            }
                            i += temp.ExitPosition + 1;
                        }
                        else if (para[i].StartsWith('['))
                        {
                            var temp = NewStringAstParse(para, i);
                            for (int j = 0; j < temp.Nodes.Count; j++)
                            {
                                nodes.Add(temp.Nodes[j]);
                            }
                            i = temp.ExitPosition;
                        }
                        else
                        {
                            nodes.Add(new VariableNode(para[i]));
                        }
                    }
                    break;

                case VariableSetup.VariableTypes.Int:
                    for(int i = startIndex; i < para.Length; i++)
                    {
                        if (para[i] == "CALL_LITERAL")
                        {
                            var temp = NewParameterAstParse(para, i);
                            for (int j = 0; j < temp.Nodes.Count; j++)
                            {
                                nodes.Add(new CallLiteral.FunctionCallNode(para[i + 1], temp.Nodes));
                            }
                            i += temp.ExitPosition + 1;
                        }
                        else
                        {
                            for (int j = 0; j < para[i].Length; j++)
                            {
                                if (Regex.IsMatch(para[i][j].ToString(), @"[0-9]|\+|\-|\/|\*|\(|\)"))
                                {
                                    nodes.Add(new ValueNode(Encoding.UTF8.GetBytes(para[i][j].ToString())));
                                }
                                else
                                {
                                    StringBuilder sb = new StringBuilder();
                                    try
                                    {
                                        while (!Regex.IsMatch(para[i][j].ToString(), @"\+|\-|\/|\*|\(|\)"))
                                        {
                                            sb.Append(para[i][j++]);
                                        }
                                    } 
                                    catch (IndexOutOfRangeException e) { }
                                    catch (Exception e) { ErrorHandling.Alert("Something went wrong: " + e, ConsoleColor.Red); }

                                    j--;
                                    nodes.Add(new VariableNode(sb.ToString()));
                                }
                            }
                        }
                    }
                    break;

                case VariableSetup.VariableTypes.Boolean:
                    for(int i = startIndex; i < para.Length; i++)
                    {
                        if (para[i] == "CALL_LITERAL")
                        {
                            var temp = NewParameterAstParse(para, i);
                            for (int j = 0; j < temp.Nodes.Count; j++)
                            {
                                nodes.Add(new CallLiteral.FunctionCallNode(para[i + 1], temp.Nodes));
                            }
                            i += temp.ExitPosition + 1;
                        }
                        else
                        {
                            try
                            {
                                nodes.Add(new ValueNode(BitConverter.GetBytes(Convert.ToBoolean(para[startIndex]))));
                            }
                            catch
                            {
                                var temp = BooleanAstParse(para, i);
                                if(temp.Nodes.Count == 3)
                                {
                                    nodes.Add(new ConditionNode(temp.Nodes[0], Encoding.UTF8.GetString(ByteManipulation.GetValue(temp.Nodes[1])), temp.Nodes[2]));
                                }
                                else
                                {
                                    ErrorHandling.Alert("3 arguments expected at: " + String.Join(" ", para), ConsoleColor.Red);
                                }
                                i += temp.ExitPosition + 2;
                            }
                        }
                    }
                    break;

                default:

                    break;
            }
            
            return nodes;
        }

        public struct ContextualReturn
        {
            public ContextualReturn(List<AstNode> nodes, int position)
            {
                Nodes = nodes;
                ExitPosition = position;
            }

            public List<AstNode> Nodes { get; set; }
            public int ExitPosition { get; set; }
        }
    }
}