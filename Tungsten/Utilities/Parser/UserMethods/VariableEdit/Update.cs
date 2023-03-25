using System.Text;
using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class VariableUpdate : IMethod, IUsing, ILexer
    { 
        public string Name { get; set; } = "UPDATE";
        public string Path { get; set; } = "Variables";
        public Regex RegexCode { get; set; } = new Regex(@"^update$|WSupdate");

        // Formats & Modifies Variables
        public void Execute(string[] para)
        {
            para = VariableSetup.Convert(para, 3);

            switch (para[1])
            {
                case "STRING":
                    //VariableSetup.UpdateEntry(para[2], TextMethods.ParseText(para, 3, '[', ']'));
                    VariableSetup.AddEntry(para[2], VariableSetup.VariableTypes.String, Encoding.UTF8.GetBytes(TextMethods.ParseText(para, 3, '[', ']')));
                    break;

                case "INT":
                    try
                    {
                        double maths = Maths.Evaluate(TextMethods.CalcString(String.Join(" ", para, 1, para.Length - 1), '(', ')'));
                        //VariableSetup.UpdateEntry(para[2], maths);
                        VariableSetup.AddEntry(para[2], VariableSetup.VariableTypes.Int,BitConverter.GetBytes(Convert.ToInt32(maths)));
                    }
                    catch
                    {
                        //VariableSetup.UpdateEntry(para[2], para[3]);
                        VariableSetup.AddEntry(para[2], VariableSetup.VariableTypes.Int, BitConverter.GetBytes(Convert.ToInt32(para[3])));
                    }
                    break;

                case "BOOL":
                    try
                    {
                        //VariableSetup.UpdateEntry(para[2], Convert.ToBoolean(para[3]));
                        VariableSetup.AddEntry(para[2], VariableSetup.VariableTypes.Boolean, BitConverter.GetBytes(Convert.ToBoolean(para[3])));
                    }
                    catch
                    {
                        Console.WriteLine("Unsupported Bool Type");
                    }
                    break;
                case "MATRIX":
                    List<string> value = new List<string>();
                    List<string> param = VariableSetup.Format(para, 2).ToList();
                    param.RemoveAt(0);

                    for (int i = 2; i < para.Length; i++)
                    {
                        value.Add(TextMethods.CalcStringForward(String.Join(" ", param), '[', ']'));

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

                        //param.Remove("[" + TextMethods.CalcStringForward(String.Join(" ", param), '[', ']') + "]");
                        //param.RemoveAt(2);

                        if (param.Count <= 3)
                        {
                            //Console.WriteLine("End Matrix Write");
                            break;
                        }
                    }
                    //VariableSetup.UpdateEntry(para[2], value.ToArray());
                    VariableSetup.AddEntry(para[2], VariableSetup.VariableTypes.Matrix, Encoding.UTF8.GetBytes(string.Join("\u0004", value.ToArray())));
                    break;
            }
        }
    }
}