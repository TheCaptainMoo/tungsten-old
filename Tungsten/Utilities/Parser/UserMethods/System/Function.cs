using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class Function : ILineInteractable, IUsing
    {
        public string Name { get; set; } = "FUNCT";

        public string Path { get; set; } = "System";

        // Generate Function and Save Parameters & Body to Memory
        public int lineExecute(string[] words, int lineNumber)
        {
            // Initialise Variables
            List<string[]> body = new List<string[]>();
            List<string> name = new List<string>();

            string str = TextMethods.CalcStringForward(String.Join(" ", words, 1, words.Length - 1), '<', '>'); ;
            string[] para;

            para = str.Replace(",", "").Split(" ");

            // Get Parameters
            for (int j = 0; j < para.Length; j++)
            {
                name.Add(para[j]);
            }

            int startPos = 0;
            int endPos = 0;

            int startIndex = -1;

            // Calculations for Body Start / Finish
            for (int j = lineNumber; j < VariableSetup.lines.Count; j++)
            {
                string[] wordsInLine = VariableSetup.lines[j];

                for(int i = 0; i < wordsInLine.Length; i++)
                {
                    if (wordsInLine[i] == "SB" && startIndex <= -1)
                    {
                        startIndex = Convert.ToInt32(wordsInLine[i + 1]);
                    }

                    if (wordsInLine[i] == "SB" && Convert.ToInt32(wordsInLine[i+1]) == startIndex)
                    {
                        startPos = j;
                    } 
                    else if(wordsInLine[i] == "EB" && Convert.ToInt32(wordsInLine[i + 1]) == startIndex)
                    {
                        endPos = j - 1;
                    }
                }
            }

            // Get Body
            while (startPos < endPos)
            {
                body.Add(VariableSetup.lines[startPos + 1]);

                startPos++;
            }

            // Save Parameters and Body to Memory
            VariableSetup.functionParameters.Add(words[1].ToUpper(), new FunctionParam(name));
            VariableSetup.functionBody.Add(words[1].ToUpper(), new FunctionBody(body));

            // Jump to the End of the Function
            return endPos + 1;
        }
    }
}