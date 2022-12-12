using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class While : ILineInteractable, IUsing, ILexer
    {
        public string Name { get; set; } = "WHILE";
        public string Path { get; set; } = "System";
        public Regex RegexCode { get; set; } = new Regex(@"^while$|WSwhile");

        public int lineExecute(string[] para, int lineNumber)
        {
            // Initialise Variables
            string[] whileStr = TextMethods.CalcString(String.Join(" ", para, 1, para.Length - 1), '<', '>').Split(" ");
            List<string> modifier = VariableSetup.Convert(whileStr, 0).ToList();        

            int startPos = 0;
            int endPos = 0;

            int startIndex = -1;

            // Run Through While Loop Body | Find Start & End Positions
            for(int i = lineNumber; i < VariableSetup.lines.Count; i++)
            {
                
                string[] wordsInLine = VariableSetup.lines[i];
                for(int j = 0; j < wordsInLine.Length; j++)
                {
                    if (wordsInLine[j] == "SB" && startIndex <= -1)
                    {
                        startIndex = Convert.ToInt32(wordsInLine[j + 1]);
                    }
                    if (wordsInLine[j] == "SB" && Convert.ToInt32(wordsInLine[j + 1]) == startIndex)
                    {
                        startPos = i;
                        try
                        {
                            VariableSetup.whileStartPosition.Add(Convert.ToInt32(wordsInLine[j + 1]), startPos);
                        }
                        catch { }
                        if (!Check.Operation(modifier[0], modifier[1], modifier[2]))
                        {
                            return VariableSetup.whileEndPosition[startIndex] + 2;
                        }
                    }
                    else if((wordsInLine[j] == "EB" || wordsInLine[j] == "WEB") && Convert.ToInt32(wordsInLine[j+1]) == startIndex)
                    {
                        endPos = i-1;
                        try
                        {
                            VariableSetup.whileEndPosition.Add(Convert.ToInt32(wordsInLine[j + 1]), endPos);
                        }
                        catch { }

                        if(Check.Operation(modifier[0], modifier[1], modifier[2]))
                        {
                            wordsInLine[j] = "WEB";
                            if (Convert.ToInt32(wordsInLine[j+1]) == startIndex)
                            {
                                return startPos;
                            }
                        }
                        else
                        {
                            return Convert.ToInt32(VariableSetup.whileEndPosition[Convert.ToInt32(wordsInLine[j + 1])]) + 1;
                        }
                    } 
                }
            }

            if(Check.Operation(modifier[0], modifier[1], modifier[2]))
            {
                return startPos;
            }
            else
            {
                return endPos + 1;
            }
        }
    }
}