using System.Text;
using System.Text.RegularExpressions;
using System.IO;

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
            PRINT,
            MATH
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

        static IDictionary<int, string[]> lines = new Dictionary<int, string[]>();

        static void Main(string[] args)
        {
            reset:
            Clean();
            string path = Console.ReadLine().Replace("\"", "");
            StreamReader sr = new StreamReader(path);

            string[] _args = sr.ReadToEnd().Split(" "); //Console.ReadLine().Split("\n");

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
                lines.Add(i, line[i].Split("WS"));
            }

            //Console.WriteLine("Lexer: " + lexerOut);

            Parser();

            goto reset;
        }

        public static void Clean()
        {
            lines = new Dictionary<int, string[]>();
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
                }
                output.Add(res);
            }

            foreach (string outp in output)
            {
                //Console.WriteLine(outp);
            }

            return output;
        }

        static List<TokenAssign> LexerInit()
        {
            List<TokenAssign> ta = new List<TokenAssign>();

            ta.Add(new TokenAssign(TokenList.WS, new Regex(@"\s+|=")));
            ta.Add(new TokenAssign(TokenList.STRING, new Regex(@"^string$|^string:$|^WSstring$")));
            ta.Add(new TokenAssign(TokenList.INT, new Regex(@"^int$|^int:$|^WSint$")));
            ta.Add(new TokenAssign(TokenList.BOOL, new Regex(@"^bool$|^bool:$|^WSbool$")));
            ta.Add(new TokenAssign(TokenList.NL, new Regex(@";|\n")));
            ta.Add(new TokenAssign(TokenList.PRINT, new Regex(@"^print$|^print:$|^WSprint$")));
            ta.Add(new TokenAssign(TokenList.MATH, new Regex(@"^math$|^math:$|^WSmath$")));

            return ta;
        }

        static void Parser()
        {
            IDictionary<string, string> variableString = new Dictionary<string, string>();
            IDictionary<string, int> variableInt = new Dictionary<string, int>();
            IDictionary<string, bool> variableBool = new Dictionary<string, bool>();

            for (int i = 0; i < lines.Count; i++)
            {
                string[] words = lines[i];
                words = words.Where(x => !string.IsNullOrEmpty(x)).ToArray();

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
                    words[1] = words[1].Replace(":", "");
                }

                if (words[0] == "STRING")
                {
                    if (variableString.ContainsKey(words[1]))
                    {
                        variableString.Remove(words[1]);
                        //variableString.Add(words[1], CalcString(String.Join(" ", words, 2, words.Length - 2), '[', ']'));
                    }
                    variableString.Add(words[1], CalcString(String.Join(" ", words, 2, words.Length - 2), '[', ']'));
                    
                    //Console.WriteLine(CalcString(String.Join(" ", words, j + 2, words.Length - (j + 2)), '[', ']'));

                    //Console.WriteLine("String Detected");
                }
                else if (words[0] == "INT")
                {
                    if (variableInt.ContainsKey(words[1]))
                    {
                        variableInt.Remove(words[1]);
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
                else if (words[0] == "PRINT")
                {
                    StringBuilder sb = new StringBuilder();

                    for (int j = 1; j < words.Length; j++)
                    {
                        if (words[j].StartsWith("["))
                        {
                            sb.Append(CalcStringForward(String.Join(" ", words, j, words.Length - j), '[', ']'));
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

                    Console.WriteLine(sb.ToString());
                } 
                else if(words[0] == "MATH")
                {
                    string compute = "";
                    try
                    {
                        for(i = 1; i < words.Length; i++)
                        {
                            if (variableInt.ContainsKey(words[i])) {
                                compute += variableInt[words[i]];
                            } 
                            else if (variableString.ContainsKey(words[i]))
                            {
                                compute = variableString[words[i]];
                                break;
                            }
                            else
                            {
                                compute += words[i];
                            }
                        }
                        Console.WriteLine(Evaluate(compute));
                    }
                    catch
                    {
                        Console.WriteLine(Evaluate(CalcString(String.Join(" ", words, 1, words.Length - 1), '(', ')')));
                    }
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