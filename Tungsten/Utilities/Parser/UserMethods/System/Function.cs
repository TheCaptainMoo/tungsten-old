using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;
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
            //List<string> paramList = new List<string>();
            List<Parameter> paramList = new List<Parameter>();

            // Get Parameters
            for (int j = 0; j < param.Length; j += 2)
            {
                switch (param[j].ToUpper())
                {
                    case "STRING":
                        paramList.Add(new Parameter(VariableSetup.VariableTypes.String, param[j + 1]));
                        break;

                    case "INT":
                        paramList.Add(new Parameter(VariableSetup.VariableTypes.Int, param[j + 1]));
                        break;

                    case "BOOL":
                        paramList.Add(new Parameter(VariableSetup.VariableTypes.Boolean, param[j + 1]));
                        break;
                }
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

            VariableSetup.functions.Add(para[1], new FunctionNode(para[1], paramList, bodyNodes));
            return new LinedAstReturn(endIndex, new FunctionNode(para[1], paramList, bodyNodes));
        }

        public class FunctionNode : AstNode
        {
            public FunctionNode(string name, List<Parameter> parameters, List<AstNode> body)
            {
                Name = name;
                Parameters = parameters;
                Body = body;
            }

            public override object? Execute()
            {
                return null;
            }

            public string Name { get; set; }
            public List<Parameter> Parameters { get; set; }
            public List<AstNode> Body { get; set; }
        }

        public struct Parameter
        {
            public Parameter(VariableSetup.VariableTypes type, string name)
            {
                Type = type;
                Name = name;
            }

            public VariableSetup.VariableTypes Type { get; set; }
            public string Name { get; set; }
        }
    }
}