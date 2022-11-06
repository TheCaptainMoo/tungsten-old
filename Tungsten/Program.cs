using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Tungsten_Interpreter
{
    internal class Program
    {
        public enum TokenList
        {
            WS,
            STRING,
            INT,
            BOOL,
            NL,
            FUNCT,
            PRINT,
            MATH,
            UPDATE
        }

        public class TokenAssign
        {
            public TokenAssign(TokenList tokenList, Regex regex)
            {
                TokenList = tokenList;
                this.regex = regex;
            }

            public TokenList TokenList { get; set; }
            public Regex regex { get; set; }
        }

        public class FunctionDeclaration
        {
            public FunctionDeclaration(/*Dictionary<string[], string[]>*/List<string> functionP, Dictionary<int, string[]> functionB)
            {
                functionBody = functionB;
                functionParameters = functionP;
            }
            public Dictionary<int, string[]> functionBody { get; set; }
            public /*Dictionary<string[], string[]>*/ List<string> functionParameters { get; set; }
        }

        static IDictionary<int, string[]> lines = new Dictionary<int, string[]>();

        // Runtime Parsed Variables
        static IDictionary<string, string> variableString = new Dictionary<string, string>();
        static IDictionary<string, int> variableInt = new Dictionary<string, int>();
        static IDictionary<string, bool> variableBool = new Dictionary<string, bool>();
        static IDictionary<string, FunctionDeclaration> functionDeclarations = new Dictionary<string, FunctionDeclaration>();

        static void Main(string[] args)
        {
            string[] splitChars =
            {
                " ",
                //"\n",
                "\r",
                "\t",
                //";"
            };

            string[] lineChars =
            {
                "WS",
                "\0",
                "NL"
            };

            reset:
            Clean();
            string path = Console.ReadLine().Replace("\"", "");
            StreamReader sr = new StreamReader(path);

            string[] _args = sr.ReadToEnd().Split(splitChars, StringSplitOptions.RemoveEmptyEntries); //Console.ReadLine().Split("\n");

            //Parser(Lexer(_args).ToArray());
            string[] lexerArr = Lexer(_args).ToArray();
            string lexerOut = "";

            foreach (string lexer in lexerArr)
            {
                lexerOut += lexer + "WS";
            }

            string[] line = lexerOut.Split("NL");

            for (int i = 0; i < line.Length; i++)
            {
                lines.Add(i, line[i].Split(lineChars, StringSplitOptions.RemoveEmptyEntries));
            }

            Console.WriteLine("Lexer: " + lexerOut);

            Parser();

            goto reset;
        }

        public static void Clean()
        {
            lines = new Dictionary<int, string[]>();
            variableString = new Dictionary<string, string>();
            variableInt = new Dictionary<string, int>();
            variableBool = new Dictionary<string, bool>();
        }

        static List<string> Lexer(string[] args)
        {
            List<TokenAssign> ta = LexerInit();
            List<string> output = new List<string>();
            string res;

            //Console.WriteLine(args.Length);


            //Lex Values
            foreach (string arg in args)
            {
                res = arg;
                for (int i = 0; i < ta.Count; i++)
                {
                    res = Regex.Replace(res, ta[i].regex.ToString(), ta[i].TokenList.ToString());
                    //Console.WriteLine(res);
                    if (res.EndsWith('{') || res.StartsWith('}'))
                    {
                        res += "NL";
                    }
                }
                output.Add(res);
            }

            foreach (string outp in output)
            {
                Console.WriteLine(outp);
            }

            return output;
        }

        static List<TokenAssign> LexerInit()
        {
            List<TokenAssign> ta = new List<TokenAssign>();

            ta.Add(new TokenAssign(TokenList.WS, new Regex(@"\s+|\t")));
            ta.Add(new TokenAssign(TokenList.STRING, new Regex(@"^string$|^string:$|WSstring")));
            ta.Add(new TokenAssign(TokenList.INT, new Regex(@"^int$|^int:$|WSint")));
            ta.Add(new TokenAssign(TokenList.BOOL, new Regex(@"^bool$|^bool:$|WSbool")));
            ta.Add(new TokenAssign(TokenList.NL, new Regex(@";|\n|\r")));
            ta.Add(new TokenAssign(TokenList.FUNCT, new Regex(@"^funct$|WSfunct")));
            ta.Add(new TokenAssign(TokenList.PRINT, new Regex(@"^print$|^print:$|WSprint")));
            ta.Add(new TokenAssign(TokenList.MATH, new Regex(@"^math$|^math:$|WSmath")));
            ta.Add(new TokenAssign(TokenList.UPDATE, new Regex(@"^update$|WSupdate")));
            
            return ta;
        }

        static void Parser()
        {
            for (int i = 0; i < lines.Count; i++)
            {
                string[] words = lines[i];

                functParse:
                words = words.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                //words[0] = words[0].ToUpper();

                if (words != null)
                {
                    try
                    {
                        words[0] = words[0].ToUpper();
                    }
                    catch { }
                }

                if (words.Length == 0)
                {
                    break;
                }

                if ((words[0] == "STRING" || words[0] == "INT" || words[0] == "BOOL") && !words[1].EndsWith(':'))
                {
                    Console.WriteLine("Relevant Assigner ':' Expected At: '" + words[1] + "'");
                    return;
                }
                else
                {
                    try
                    {
                        words[1] = words[1].Replace(":", "");
                    }
                    catch { }
                }

                if (words[0] == "STRING")
                {
                    if (variableString.ContainsKey(words[1]))
                    {
                        Console.WriteLine("Please Use The 'update' Keyword To Reassign: " + words[1]);
                        return;
                    }
                    variableString.Add(words[1], ParseText(words, 2, '[', ']'));
                }
                else if (words[0] == "INT")
                {
                    if (variableInt.ContainsKey(words[1]))
                    {
                        Console.WriteLine("Please Use The 'update' Keyword To Reassign: " + words[1]);
                        return;
                    }
                    try {
                        double maths = Evaluate(CalcString(String.Join(" ", words, 1, words.Length - 1), '(', ')'));
                        variableInt.Add(words[1], Convert.ToInt32(maths));
                    }
                    catch
                    {
                        variableInt.Add(words[1], Convert.ToInt32(words[2]));
                    }
                    //Console.WriteLine(variableInt[words[1]]);
                }
                else if (words[0] == "BOOL")
                {
                    if (variableBool.ContainsKey(words[1]))
                    {
                        Console.WriteLine("Please Use The 'update' Keyword To Reassign: " + words[1]);
                        return;
                    }
                    try
                    {
                        variableBool.Add(words[1], Convert.ToBoolean(words[2]));
                    }
                    catch
                    {
                        Console.WriteLine("Unsupported Bool Type");
                    }
                }
                else if (words[0] == "FUNCT")
                {
                    //Dictionary<string[], string[]> parameters = new Dictionary<string[], string[]>();
                    List<string> parameters = new List<string>();

                    Dictionary<int, string[]> body = new Dictionary<int, string[]>();

                    string str = CalcStringForward(String.Join(" ", words, 1, words.Length - 1), '<', '>'); ;
                    string[] para;

                    //List<string> type = new List<string>();
                    List<string> name = new List<string>();

                    para = str.Replace(",", "").Split(" ");
                    para[0] = para[0].ToUpper();
                    
                    for(int j = 0; j < para.Length; j += 2)
                    {
                        //type.Add(para[j]);
                        //name.Add(para[j+1]);
                        name.Add(para[j + 1]);
                    }

                    int startPos = 0;
                    int endPos = 0;
                    int index = 0;

                    for(int j = i; j < lines.Count; j++)
                    {
                        string[] wordsInLine = lines[j];
                        foreach (string word in wordsInLine)
                        {
                            foreach(char c in word)
                            {
                                if(c == '{')
                                {
                                    startPos = j;
                                }

                                if(c == '}')
                                {
                                    endPos = j - 1;
                                }
                            }
                        }
                    }

                    while(startPos < endPos)
                    {
                        body.Add(index++, lines[startPos+1]);

                        startPos++;
                    }

                    //parameters.Add(type.ToArray(), name.ToArray());

                    functionDeclarations.Add(words[1].ToUpper(), new FunctionDeclaration(name, body));

                    i = endPos+1;

                    Console.WriteLine();
                }
                else if (words[0] == "PRINT")
                {
                    Console.WriteLine(ParseText(words, 1, '[', ']'));
                } 
                else if(words[0] == "MATH")
                {
                    string compute = "";
                    try
                    {
                        for(int j = 1; j < words.Length; j++)
                        {
                            if (variableInt.ContainsKey(words[j])) {
                                compute += variableInt[words[j]];
                            } 
                            else if (variableString.ContainsKey(words[j]))
                            {
                                compute = variableString[words[j]];
                                break;
                            }
                            else
                            {
                                compute += words[j];
                            }
                        }
                        Console.WriteLine(Evaluate(compute));
                    }
                    catch
                    {
                        Console.WriteLine(Evaluate(CalcString(String.Join(" ", words, 1, words.Length - 1), '(', ')')));
                    }
                }
                else if (words[0] == "UPDATE")
                {
                    if (variableString.ContainsKey(words[1]))
                    {
                        variableString.Remove(words[1]);
                        variableString.Add(words[1], ParseText(words, 2, '[', ']'));
                    } 
                    else if (variableInt.ContainsKey(words[1]))
                    {
                        variableInt.Remove(words[1]);
                        try
                        {
                            double maths = Evaluate(CalcString(String.Join(" ", words, 1, words.Length - 1), '(', ')'));
                            variableInt.Add(words[1], Convert.ToInt32(maths));
                        }
                        catch
                        {
                            variableInt.Add(words[1], Convert.ToInt32(words[2]));
                        }
                    } 
                    else if (variableBool.ContainsKey(words[1]))
                    {
                        if (variableBool.ContainsKey(words[1]))
                        {
                            variableBool.Remove(words[1]);
                        }
                        try
                        {
                            variableBool.Add(words[1], Convert.ToBoolean(words[2]));
                        }
                        catch
                        {
                            Console.WriteLine("Unsupported Bool Type");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Variable: " + words[1] + " Doesn't Exist");
                    }
                }
                else if (functionDeclarations.ContainsKey(words[0]))
                {
                    Console.WriteLine("You found a function");
                    string[] args = ParseText(words, 0, '<', '>').Split(",");

                    //Console.WriteLine(args);

                    for(int j = 0; j < args.Length; j++)
                    {
                        // Replace All Mentions Of Parameters With Args 
                    }

                    int index = 0;

                    for (int l = 0; l < args.Length; l++)
                    {
                        string arg = args[l];
                        for (int j = 0; j < functionDeclarations[words[0]].functionParameters.Count; j++)
                        {
                            for (int k = 0; k < functionDeclarations[words[0]].functionBody[j].Length; k++)
                            {
                                //Console.WriteLine(functionDeclarations[words[0]].functionBody[j][k]);

                                //Console.WriteLine("Parameters: " + functionDeclarations[words[0]].functionParameters[l]);

                                if(functionDeclarations[words[0]].functionBody[j][k] == functionDeclarations[words[0]].functionParameters[l])
                                {
                                    //Console.WriteLine("Replace With: " + arg);
                                    functionDeclarations[words[0]].functionBody[j][k] = arg;
                                }

                                //functionDeclarations[words[0]].functionBody[j][k] = 
                            }
                        }
                    }

                    for (int j = 0; j < functionDeclarations[words[0]].functionBody.Count; j++) {
                        lines.Add(lines.Count, functionDeclarations[words[0]].functionBody[j]);
                    }

                    //goto functParse;
                }
            }
        }

        static string CalcString(string input, char openChar, char closeChar)
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

        static string CalcStringForward(string input, char openChar, char closeChar)
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

        static int CalcStringEnd(string input, int startPos, char closeChar)
        {
            
            int endPos = input.Length;

            for (int i = 0; i < endPos; i++)
            {
                if (input[i] == closeChar)
                {
                    endPos = i - startPos;
                    break;
                }
            }
            return endPos;
        }

        static int CalcCharPosArr(string[] input, int startPos, char charToFind)
        {
            string str = "";
            int pos = 0;

            foreach (string i in input)
            {
                str = i;
                if (i.EndsWith(charToFind))
                {
                    return pos;
                }
                pos++;
            }

            return -1;
        }

        static string ParseText(string[] words, int startIndex, char startsWith, char endsWith)
        {
            StringBuilder sb = new StringBuilder();

            for (int j = startIndex; j < words.Length; j++)
            {
                if (words[j].StartsWith(startsWith))
                {
                    sb.Append(CalcStringForward(String.Join(" ", words, j, words.Length - j), startsWith, endsWith));
                }
                else if (variableString.ContainsKey(words[j]))
                {
                    sb.Append(variableString[words[j]]);
                }
                else if (variableInt.ContainsKey(words[j]))
                {
                    sb.Append(variableInt[words[j]]);
                }
                else if (variableBool.ContainsKey(words[j]))
                {
                    sb.Append(variableBool[words[j]]);
                }
            }

            return sb.ToString();
        }

        public static double Evaluate(string expression)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add("expression", string.Empty.GetType(), expression);
            System.Data.DataRow row = table.NewRow();
            table.Rows.Add(row);
            return double.Parse((string)row["expression"]);
        }      
    }
}