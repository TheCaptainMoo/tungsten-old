/*using System.Text.RegularExpressions;
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
            Span<string> modifier = VariableSetup.Convert(TextMethods.CalcString(String.Join(" ", para, 1, para.Length - 1), '<', '>').Split(" "), 0).AsSpan();

            int startPos = 0;
            int endPos = 0;

            int startIndex = -1;

            // Run Through While Loop Body | Find Start & End Positions
            if (!VariableSetup.whileSetup.ContainsKey(int.Parse(VariableSetup.lines[lineNumber + 1][1])))
            {
                if (VariableSetup.whileStartPosition.ContainsKey(int.Parse(VariableSetup.lines[lineNumber + 1][1])) && VariableSetup.whileEndPosition.ContainsKey(int.Parse(VariableSetup.lines[lineNumber + 1][1])))
                {
                    VariableSetup.whileSetup.Add(int.Parse(VariableSetup.lines[lineNumber + 1][1]), true);
                }

                for (int i = lineNumber; i < VariableSetup.lines.Count; i++)
                {

                    string[] wordsInLine = VariableSetup.lines[i];
                    for (int j = 0; j < wordsInLine.Length; j++)
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
                        else if ((wordsInLine[j] == "EB" || wordsInLine[j] == "WEB") && Convert.ToInt32(wordsInLine[j + 1]) == startIndex)
                        {
                            endPos = i - 1;
                            try
                            {
                                VariableSetup.whileEndPosition.Add(Convert.ToInt32(wordsInLine[j + 1]), endPos);
                            }
                            catch { }

                            if (Check.Operation(modifier[0], modifier[1], modifier[2]))
                            {
                                wordsInLine[j] = "WEB";
                                if (Convert.ToInt32(wordsInLine[j + 1]) == startIndex)
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

                
            }

            if(Check.Operation(modifier[0], modifier[1], modifier[2]))
            {
                return VariableSetup.whileStartPosition[int.Parse(VariableSetup.lines[lineNumber + 1][1])];
            }
            else
            {
                return VariableSetup.whileEndPosition[int.Parse(VariableSetup.lines[lineNumber + 1][1])] + 1;
            }
        }
    }
}*/
using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;
using static Tungsten_Interpreter.Utilities.AST.AbstractSyntaxTree;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class While : INestedLexer
    {
        public string Name { get; set; } = "WHILE";
        public Regex RegexCode { get; set; } = new Regex(@"^while$|WSwhile");

        public LinedAstReturn AstConstructor(string[] para, List<string[]> lines, int lineNum)
        {
            //Span<string> whileCondition = VariableSetup.Convert(TextMethods.CalcString(String.Join(" ", para, 1, para.Length - 1), '<', '>').Split(" "), 0).AsSpan();
            string[] whileCondition = TextMethods.CalcString(String.Join(" ", para, 1, para.Length - 1), '<', '>').Split(" ");

            // Body Calculations 
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
            List<AstNode> conditionNodes = TextMethods.GenericAstParse(whileCondition, 0);

            //return new LinedAstReturn(endIndex, new WhileStatementNode(new ConditionNode(whileCondition[0], whileCondition[1], whileCondition[2]), bodyNodes));
            return new LinedAstReturn(endIndex, new WhileStatementNode(new ConditionNode(conditionNodes[0], whileCondition[1], conditionNodes[1]), bodyNodes));
        }

        public class WhileStatementNode : AstNode
        {
            public WhileStatementNode(AstNode condition, List<AstNode> body)
            {
                Condition = condition;
                BodyNodes = body;
            }

            public override object? Execute()
            {
                bool conditionalResult = (bool)Condition.Execute();

                while(conditionalResult)
                {
                    for (int i = 0; i < BodyNodes.Count; i++)
                    {
                        BodyNodes[i].Execute();
                    }

                    conditionalResult = (bool)Condition.Execute();
                }

                return null;
            }

            public AstNode Condition { get; set; }
            public List<AstNode> BodyNodes { get; set; }
        }

        /*public int lineExecute(string[] para, int lineNumber)
                {
                    // Initialise Variables
                    Span<string> modifier = VariableSetup.Convert(TextMethods.CalcString(String.Join(" ", para, 1, para.Length - 1), '<', '>').Split(" "), 0).AsSpan();

                    int startPos = 0;
                    int endPos = 0;

                    int startIndex = -1;

                    // Run Through While Loop Body | Find Start & End Positions
                    if (!VariableSetup.whileSetup.ContainsKey(int.Parse(VariableSetup.lines[lineNumber + 1][1])))
                    {
                        if (VariableSetup.whileStartPosition.ContainsKey(int.Parse(VariableSetup.lines[lineNumber + 1][1])) && VariableSetup.whileEndPosition.ContainsKey(int.Parse(VariableSetup.lines[lineNumber + 1][1])))
                        {
                            VariableSetup.whileSetup.Add(int.Parse(VariableSetup.lines[lineNumber + 1][1]), true);
                        }

                        for (int i = lineNumber; i < VariableSetup.lines.Count; i++)
                        {

                            string[] wordsInLine = VariableSetup.lines[i];
                            for (int j = 0; j < wordsInLine.Length; j++)
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
                                else if ((wordsInLine[j] == "EB" || wordsInLine[j] == "WEB") && Convert.ToInt32(wordsInLine[j + 1]) == startIndex)
                                {
                                    endPos = i - 1;
                                    try
                                    {
                                        VariableSetup.whileEndPosition.Add(Convert.ToInt32(wordsInLine[j + 1]), endPos);
                                    }
                                    catch { }

                                    if (Check.Operation(modifier[0], modifier[1], modifier[2]))
                                    {
                                        wordsInLine[j] = "WEB";
                                        if (Convert.ToInt32(wordsInLine[j + 1]) == startIndex)
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


                    }

                    if (Check.Operation(modifier[0], modifier[1], modifier[2]))
                    {
                        return VariableSetup.whileStartPosition[int.Parse(VariableSetup.lines[lineNumber + 1][1])];
                    }
                    else
                    {
                        return VariableSetup.whileEndPosition[int.Parse(VariableSetup.lines[lineNumber + 1][1])] + 1;
                    }
                }*/

    }
}