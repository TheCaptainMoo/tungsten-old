using System.Text;
using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.Methods
{
    public class TextMethods
    {
        // Gets a Substring Between 2 Characters
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
    }
}