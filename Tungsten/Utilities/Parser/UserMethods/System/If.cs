using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using static Tungsten_Interpreter.Utilities.AST.AbstractSyntaxTree;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods.System
{
    public class IfStatement : INestedLexer
    {
        public string Name { get; set; } = "IF";
        public Regex RegexCode { get; set; } = new Regex(@"^if$|WSif");

        public LinedAstReturn AstConstructor(string[] para, List<string[]> lines, int lineNum)
        {
            string[] ifStr = TextMethods.CalcString(String.Join(" ", para, 1, para.Length - 1), '<', '>').Split(" ");
            string[] output = new string[3];

            bool insideString = false;
            int index = 0;

            // Condition Finding
            if (ifStr.Length > 3)
            {
                for (int i = 0; i < ifStr.Length; i++)
                {
                    if (ifStr[i].StartsWith('['))
                    {
                        insideString = true;
                        output[index] = "[" + TextMethods.CalcStringForward(String.Join(" ", ifStr, i, ifStr.Length - i), '[', ']') + "]";
                        index++;
                    }
                    else if (ifStr[i].EndsWith(']'))
                    {
                        insideString = false;
                    }
                    else if (insideString)
                        continue;
                    else if (index == 1)
                    {
                        output[index] = ifStr[i];
                        index++;
                    }
                }
            }
            else
            {
                output = ifStr;
            }

            // Body Finding


            int bracketIndex = Convert.ToInt32(lines[lineNum+1][1]);
            int startIndex = lineNum + 2;
            int endIndex = 0;
            for(int i = lineNum + 2; i < lines.Count; i++)
            {
                if (lines[i].Length >= 2)
                {
                    if (lines[i][0] == "EB" || lines[i][1] == bracketIndex.ToString())
                    {
                        endIndex = i;
                    }
                }
            }

            List<string[]> bodyLines = lines.GetRange(startIndex, endIndex-startIndex);

            List<AstNode> bodyNodes = Lexer.TungstenLexer.CreateNestedNode(bodyLines);

            return new LinedAstReturn(endIndex, new IfStatementNode(new ConditionNode(output[0], output[1], output[2]), bodyNodes )); // ------------------------------------------------------------------------------------ Return Ending Line Num; 
        }

        public class IfStatementNode : AstNode
        {
            public IfStatementNode(AstNode condition, List<AstNode> thenStatement)
            {
                //Type = AstNodeType.IfStatement;
                Condition = condition;
                ThenStatement = thenStatement;
            }

            public override object? Execute()
            {
                bool conditionalResult = (bool)Condition.Execute();

                if (conditionalResult)
                {
                    for (int i = 0; i < ThenStatement.Count; i++)
                    {
                        ThenStatement[i].Execute();
                    }
                }

                return null;
            }

            public AstNode Condition { get; set; }
            public List<AstNode> ThenStatement { get; set; }
        }
    }
}