using System.Text;
using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Variables;

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
        public static List<AST.AbstractSyntaxTree.AstNode> StringAstParse(string[] para, int startIndex)
        {
            List<AST.AbstractSyntaxTree.AstNode> nodes = new List<AST.AbstractSyntaxTree.AstNode>();
            bool insideString = false;
            bool stringProtection = false;

            for(int i = startIndex; i < para.Length; i++)
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
                    nodes.Add(new AST.AbstractSyntaxTree.VariableNode(para[i]));
                }
                else if(stringProtection == false)
                {
                    nodes.Add(new AST.AbstractSyntaxTree.ValueNode(Encoding.UTF8.GetBytes(CalcStringForward(String.Join(" ", para, i, para.Length-i), '[', ']'))));
                    stringProtection = true;
                }
            }

            return nodes;
        }

        /// <summary>
        /// [Gentime]
        /// </summary>
        public static List<AST.AbstractSyntaxTree.AstNode> IntAstParse(string[] para, int startIndex)
        {
            List<AST.AbstractSyntaxTree.AstNode> nodes = new List<AST.AbstractSyntaxTree.AstNode>();

            for (int i = startIndex; i < para.Length; i++)
            {
                if (Regex.IsMatch(para[i], @"^[0-9]+$"))
                {
                    //nodes.Add(new AST.AbstractSyntaxTree.ValueNode(BitConverter.GetBytes(Convert.ToInt32(para[i]))));
                    nodes.Add(new AST.AbstractSyntaxTree.ValueNode(Encoding.UTF8.GetBytes(para[i])));
                }
                else if (para[i] == "+" || para[i] == "-" || para[i] == "(" || para[i] == ")" || para[i] == "*" || para[i] == "/")
                {
                    nodes.Add(new AST.AbstractSyntaxTree.ValueNode(Encoding.UTF8.GetBytes(para[i])));
                }
                else
                {
                    nodes.Add(new AST.AbstractSyntaxTree.VariableNode(para[i]));
                }
            }

            return nodes;
        }
    
        /// <summary>
        /// [Gentime]
        /// </summary>
        public static List<AST.AbstractSyntaxTree.AstNode> GenericAstParse(string[] para, int startIndex)
        {
            List<AST.AbstractSyntaxTree.AstNode> nodes = new List<AST.AbstractSyntaxTree.AstNode>();
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


                if(insideString)
                {
                    nodes.Add(new AST.AbstractSyntaxTree.ValueNode(Encoding.UTF8.GetBytes(CalcStringForward(String.Join(" ", para, i, para.Length - i), '[', ']'))));
                }
                else if (para[i].ToLower() == "true" || para[i].ToLower() == "false")
                {
                    nodes.Add(new AST.AbstractSyntaxTree.ValueNode(BitConverter.GetBytes(Convert.ToBoolean(para[i]))));
                }
                // Integers
                else if (Regex.IsMatch(para[i], @"^[0-9]+$"))
                {
                    nodes.Add(new AST.AbstractSyntaxTree.ValueNode(Encoding.UTF8.GetBytes(para[i])));
                }
                else if (para[i] == "==" || para[i] == "<=" || para[i] == ">=" || para[i] == "<" || para[i] == ">" || para[i] == "!=")
                {
                    continue;
                }
                else
                {
                    nodes.Add(new AST.AbstractSyntaxTree.VariableNode(para[i]));
                }
            }

            return nodes;
        } 
    
        public static List<AST.AbstractSyntaxTree.AstNode> ParameterAstParse(string[] para, int startIndex)
        {
            List<AST.AbstractSyntaxTree.AstNode> nodes = new List<AST.AbstractSyntaxTree.AstNode>();
            bool insideString = false;
            bool stringProtection = false;

            for (int i = startIndex; i < para.Length; i++)
            {
                string word = para[i].Replace("<", "").Replace(">", "");

                if (word.StartsWith('['))
                {
                    insideString = true;
                }
                else if (word.EndsWith(']'))
                {
                    insideString = false;
                    stringProtection = false;
                    continue;
                }

                if (!insideString)
                {
                    if(word.Length > 0)
                        nodes.Add(new AST.AbstractSyntaxTree.VariableNode(word));
                }
                else if (stringProtection == false)
                {
                    nodes.Add(new AST.AbstractSyntaxTree.ValueNode(Encoding.UTF8.GetBytes(CalcStringForward(String.Join(" ", para, i, para.Length - i), '[', ']'))));
                    stringProtection = true;
                }
            }

            return nodes;
        }
    }
}