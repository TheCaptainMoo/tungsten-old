/*using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class Function : ILineInteractable, IUsing, ILexer
    {
        public string Name { get; set; } = "FUNCT";
        public string Path { get; set; } = "System";
        public Regex RegexCode { get; set; } = new Regex(@"^funct$|WSfunct");

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
}*/
using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using static Tungsten_Interpreter.Utilities.AST.AbstractSyntaxTree;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class Function : INestedLexer
    {
        public string Name { get; set; } = "FUNCT";
        public Regex RegexCode { get; set; } = new Regex(@"^funct$|WSfunct");

        public LinedAstReturn AstConstructor(string[] para, List<string[]> lines, int lineNum)
        {
            string str = TextMethods.CalcStringForward(String.Join(" ", para, 1, para.Length - 1), '<', '>'); ;
            string[] param = str.Replace(",", "").Split(" ");
            List<string> paramList = new List<string>();

            // Get Parameters
            for (int j = 0; j < param.Length; j++)
            {
                paramList.Add(param[j]);
            }

            // Body Finding
            int bracketIndex = Convert.ToInt32(lines[lineNum + 1][1]);
            int startIndex = lineNum + 2;
            int endIndex = 0;
            for (int i = lineNum + 2; i < lines.Count; i++)
            {
                if (lines[i].Length >= 2)
                {
                    if (lines[i][0] == "EB" || lines[i][1] == bracketIndex.ToString())
                    {
                        endIndex = i;
                        break;
                    }
                }
            }

            List<string[]> bodyLines = lines.GetRange(startIndex, endIndex - startIndex);
            List<AstNode> bodyNodes = Lexer.TungstenLexer.CreateNestedNode(bodyLines);

            return new LinedAstReturn(endIndex, new FunctionNode(para[1], paramList.ToArray(), bodyNodes));
        }

        public class FunctionNode : AstNode
        {
            public FunctionNode(string name, string[] parameters, List<AstNode> body)
            {
                Name = name;
                Parameters = parameters;
                Body = body;
            }

            public override object? Execute()
            {
                throw new NotImplementedException();
            }

            public string Name { get; set; }
            public string[] Parameters { get; set; }
            public List<AstNode> Body { get; set; }

        }
    }
}