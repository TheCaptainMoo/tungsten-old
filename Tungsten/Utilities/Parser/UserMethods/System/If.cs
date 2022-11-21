using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods.System
{
    internal class IfStatement : ILineInteractable, IUsing
    {
        public string Name { get; set; } = "IF";
        public string Path { get; set; } = "System";

        public int lineExecute(string[] para, int startLine)
        {
            string[] ifStr = TextMethods.CalcString(String.Join(" ", para, 1, para.Length - 1), '<', '>').Split(" ");
            List<string> modifier = VariableSetup.Convert(ifStr, 0).ToList();

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
