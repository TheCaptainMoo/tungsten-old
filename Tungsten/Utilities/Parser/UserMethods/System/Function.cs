using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class Function : ILineInteractable
    {
        public string Name { get; set; } = "FUNCT";

        public int lineExecute(string[] words, int lineNumber)
        {
            List<string> parameters = new List<string>();

            Dictionary<int, string[]> body = new Dictionary<int, string[]>();

            string str = TextMethods.CalcStringForward(String.Join(" ", words, 1, words.Length - 1), '<', '>'); ;
            string[] para;

            //List<string> type = new List<string>();
            List<string> name = new List<string>();

            para = str.Replace(",", "").Split(" ");
            para[0] = para[0].ToUpper();

            for (int j = 0; j < para.Length; j += 2)
            {
                //type.Add(para[j]);
                //name.Add(para[j+1]);
                name.Add(para[j + 1]);
            }

            int startPos = 0;
            int endPos = 0;
            int index = 0;

            for (int j = lineNumber; j < VariableSetup.lines.Count; j++)
            {
                string[] wordsInLine = VariableSetup.lines[j];
                /*foreach (string word in wordsInLine)
                {
                    foreach (char c in word)
                    {
                        if (c == '{')
                        {
                            startPos = j;
                        }

                        if (c == '}')
                        {
                            endPos = j - 1;
                        }
                    }
                }*/
                for(int i = 0; i < wordsInLine.Length; i++)
                {
                    if (wordsInLine[i] == "SB")
                    {
                        startPos = j;
                    } 
                    else if(wordsInLine[i] == "EB")
                    {
                        endPos = j - 1;
                    }
                }
            }

            while (startPos < endPos)
            {
                body.Add(index++, VariableSetup.lines[startPos + 1]);

                startPos++;
            }

            //parameters.Add(type.ToArray(), name.ToArray());

            //functionDeclarations.Add(words[1].ToUpper(), new FunctionDeclaration(name, body));
            //functionDeclarations.Add("ILOVETODEBUG", new FunctionDeclaration(name, body));

            VariableSetup.functionParameters.Add(words[1].ToUpper(), new FunctionParam(name));
            VariableSetup.functionBody.Add(words[1].ToUpper(), new FunctionBody(body));

            return endPos + 1;
        }
    }
}