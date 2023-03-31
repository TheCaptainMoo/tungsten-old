/*using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods.System
{
    public class IfStatement : ILineInteractable, IUsing, ILexer
    {
        public string Name { get; set; } = "IF";
        public string Path { get; set; } = "System";
        public Regex RegexCode { get; set; } = new Regex(@"^if$|WSif");

        public int lineExecute(string[] para, int startLine)
        {
            // Basic Formatting
            string[] ifStr = TextMethods.CalcString(String.Join(" ", para, 1, para.Length - 1), '<', '>').Split(" ");
            List<string> modifier = VariableSetup.Convert(ifStr, 0).ToList();

            // String Check
            for (int i = 0; i < 3; i++)
            {
                if (modifier[i].StartsWith('['))
                {
                    modifier[i] = TextMethods.ParseText(ifStr, i, '[', ']');

                    if (i + 1 >= 3)
                        break;

                    while (!modifier[i+1].EndsWith(']'))
                    {
                        modifier.RemoveAt(i+1);
                    }
                    modifier.RemoveAt(i+1);
                }
            }

            // Running If Statement
            if (!Check.Operation(modifier[0], modifier[1], modifier[2]))
            {
                for(int i = startLine; i < VariableSetup.lines.Count; i++)
                {
                    string[] wordsInLine = VariableSetup.lines[i];
                    for(int j = 0; j < wordsInLine.Length; j++)
                    {
                        if(wordsInLine[j] == "EB" || wordsInLine[j] == "IEF")
                        {
                            wordsInLine[j] = "IEF";
                            return i;
                        }
                    }
                }
            }
            return startLine;
        }
    }
}
*/