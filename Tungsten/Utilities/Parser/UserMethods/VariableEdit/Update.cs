using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class VariableUpdate : IMethod, IUsing
    {
        public string Name { get; set; } = "UPDATE";
        public string Path { get; set; } = "Variables";

        // Formats & Modifies Variables
        public void Execute(string[] para)
        {
            para = VariableSetup.Convert(para, 3);

            switch (para[1])
            {
                case "STRING":
                    VariableSetup.UpdateEntry(para[2], TextMethods.ParseText(para, 3, '[', ']'));
                    break;

                case "INT":
                    try
                    {
                        double maths = Maths.Evaluate(TextMethods.CalcString(String.Join(" ", para, 1, para.Length - 1), '(', ')'));
                        VariableSetup.UpdateEntry(para[2], maths);
                    }
                    catch
                    {
                        VariableSetup.UpdateEntry(para[2], para[3]);
                    }
                    break;

                case "BOOL":
                    try
                    {
                        VariableSetup.UpdateEntry(para[2], Convert.ToBoolean(para[3]));
                    }
                    catch
                    {
                        Console.WriteLine("Unsupported Bool Type");
                    }
                    break;
                case "MATRIX":
                    List<string> value = new List<string>();

                    value = TextMethods.ParseText(para, 3, '<', '>').Split(",").ToList();

                    for (int i = 0; i < value.Count; i++)
                    {
                        value[i] = value[i].Trim();
                        value[i] = value[i].Substring(1, value[i].Length - 2);
                    }

                    VariableSetup.UpdateEntry(para[2], value.ToArray());
                    break;
            }
        }
    }
}