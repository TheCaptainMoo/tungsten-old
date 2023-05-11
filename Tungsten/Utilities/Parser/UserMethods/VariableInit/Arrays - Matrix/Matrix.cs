using System.Text;
using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class MatrixCreation : ILexer
    {
        public string Name { get; set; } = "MATRIX";
        public Regex RegexCode { get; set; } = new Regex(@"^mat$|WSmat|#\[\]");

        public AST.AbstractSyntaxTree.AstNode AstConstructor(string[] para)
        {
            List<string> value = new List<string>();

            //para = VariableSetup.Format(para, 2);
            List<string> param = VariableSetup.Convert(VariableSetup.Format(para, 2), 2).ToList();
            param.RemoveAt(2);

            for (int i = 2; i < para.Length; i++)
            {
                if (!param[2].StartsWith('[') && !param[2].EndsWith(']'))
                {
                    param[2] = "[" + param[2] + "]";
                }

                value.Add(TextMethods.CalcStringForward(String.Join(" ", param), '[', ']'));

                if (param[2] != null)
                {

                    if (param[2].EndsWith(']'))
                    {
                        param.RemoveAt(2);
                    }
                    else
                    {
                        while (!param[2].EndsWith(']'))
                        {
                            param.RemoveAt(2);
                        }
                        param.RemoveAt(2);
                    }
                }
                else
                {
                    break;
                }

                if (param.Count < 3)
                {
                    break;
                }
            }

            //VariableSetup.nodes.Add(new AST.AbstractSyntaxTree.VariableAssignNode(VariableSetup.VariableTypes.Matrix, para[1], Encoding.UTF8.GetBytes(string.Join("\u0004", value.ToArray()))));
            return new AST.AbstractSyntaxTree.VariableAssignNode(VariableSetup.VariableTypes.Matrix, para[1], Encoding.UTF8.GetBytes(string.Join("\u0004", value.ToArray())));
        }

        // Creates a 'Matrix' (Array) in Memory
        /*public void Execute(string[] para)
        {
            List<string> value = new List<string>();

            //para = VariableSetup.Format(para, 2);
            List<string> param = VariableSetup.Convert(VariableSetup.Format(para, 2), 2).ToList();

            for (int i = 2; i < para.Length; i++)
            {
                if (!param[2].StartsWith('[') && !param[2].EndsWith(']'))
                {
                    param[2] = "[" + param[2] + "]";
                }

                value.Add(TextMethods.CalcStringForward(String.Join(" ", param), '[', ']'));

                if (param[2] != null)
                {

                    if (param[2].EndsWith(']'))
                    {
                        param.RemoveAt(2);
                    }
                    else
                    {
                        while (!param[2].EndsWith(']'))
                        {
                            param.RemoveAt(2);
                        }
                        param.RemoveAt(2);
                    }
                }
                else
                {
                    break;
                }

                //param.Remove("[" + TextMethods.CalcStringForward(String.Join(" ", param), '[', ']') + "]");
                //param.RemoveAt(2);

                if (param.Count < 3)
                {
                    //Console.WriteLine("End Matrix Write");
                    break;
                }
            }

            //value = TextMethods.ParseText(VariableSetup.Convert(para, 2), 2, '<', '>').Split(",").ToList();

            //for(int i = 0; i < value.Count; i++)
            //{
            //    value[i] = value[i].Trim();
            //value[i] = value[i].Substring(1, value[i].Length - 2);
            //}

            //VariableSetup.AddEntry(para[1], value.ToArray());
            VariableSetup.AddEntry(para[1], VariableSetup.VariableTypes.Matrix, Encoding.UTF8.GetBytes(string.Join("\u0004", value.ToArray())));
        }*/
    }
}