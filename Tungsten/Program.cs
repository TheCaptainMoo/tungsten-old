﻿using System.Text.RegularExpressions;
using static Tungsten_Interpreter.Program;

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
            NL
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
            string[] _args = Console.ReadLine().Split(" "); //Console.ReadLine().Split("\n");

            //Parser(Lexer(_args).ToArray());
            string[] lexerArr = Lexer(_args).ToArray();
            string lexerOut = "";

            foreach(string lexer in lexerArr)
            {
                lexerOut += lexer + "WS";
            }

            string[] line = lexerOut.Split("NLWS");

            for(int i = 0; i < line.Length; i++)
            {
                lines.Add(i, line[i].Split("WS"));
            }

            Console.WriteLine(lexerOut);

            Parser();
        }

        static List<string> Lexer(string[] args)
        {
            List<TokenAssign> ta = LexerInit();
            List<string> output = new List<string>();
            string res;

            Console.WriteLine(args.Length);


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

            foreach(string outp in output)
            {
                Console.WriteLine(outp);
            }

            return output;
        }

        static List<TokenAssign> LexerInit()
        {
            List<TokenAssign> ta = new List<TokenAssign>();

            ta.Add(new TokenAssign(TokenList.WS, new Regex(@"\s+")));
            ta.Add(new TokenAssign(TokenList.STRING, new Regex(@"string|string:")));
            ta.Add(new TokenAssign(TokenList.INT, new Regex(@"int|int:")));
            ta.Add(new TokenAssign(TokenList.BOOL, new Regex(@"bool|bool:")));
            ta.Add(new TokenAssign(TokenList.NL, new Regex(@";")));

            return ta;
        }

        static void Parser()
        {
            IDictionary<string, string> variableString = new Dictionary<string, string>();
            IDictionary<string, int> variableInt = new Dictionary<string, int>();
            IDictionary<string, bool> variableBool = new Dictionary<string, bool>();

            /*for(int i = 0; i < parsedArgs.Length; i++)
            {
                //Console.WriteLine(parsedArgs[i]);
                if(parsedArgs[i] == "STRING"){
                    //variableString.Add(parsedArgs[i+1], parsedArgs[i+2]);

                    Console.WriteLine(CalcString(parsedArgs[i + 2], '[', ']'));

                    //Console.WriteLine("Var name: " + parsedArgs[i + 1] + " Assignment: " + parsedArgs[i + 2]);
                }
            }*/

            for(int i = 0; i < lines.Count; i++)
            {
                string[] words = lines[i];

                if (words.Length == 0)
                {
                    break;
                }
                
                for(int j = 0; j < words.Length; j++)
                {
                    if (words[j] == "STRING")
                    {
                        variableString.Add(words[j+1], CalcString(String.Join(" ", words, j+2, words.Length - (j+2)), '[', ']'));

                        Console.WriteLine(CalcString(String.Join(" ", words, j + 2, words.Length - (j + 2)), '[', ']'));

                        Console.WriteLine("String Detected");
                    }
                }
            }

            /*

            for (int i = 0; i < parsedArgs.Length; i++)
            {
                if (parsedArgs[i] == "funct")
                {
                    Console.WriteLine("FUNCTION");
                }
                if(parsedArgs[i] == "math")
                {
                    Console.WriteLine(Evaluate(parsedArgs[i + 1] + parsedArgs[i + 2] + parsedArgs[i + 3]));
                }
                if (parsedArgs[i] == "var")
                {
                    variables.Add(parsedArgs[i + 1], parsedArgs[i + 2]);
                    //Console.WriteLine("Var name: " + parsedArgs[i + 1] + " Assignment: " + parsedArgs[i + 2]);
                }
                if (parsedArgs[i] == "print")
                {
                    if (variables.ContainsKey(parsedArgs[i + 1]))
                    {
                        Console.WriteLine(variables[parsedArgs[i + 1]]);
                    }
                    else
                    {
                        Console.WriteLine(parsedArgs[i + 1]);
                    }
                }
            }

            */
        }

        static string CalcString(string input, char openChar, char closeChar)
        {
            int startPos = 0;
            int endPos = input.Length;

            for(int i = 0; i < endPos; i++)
            {
                if (input[i] == openChar)
                {
                    startPos = i + 1;
                    break;
                }
            }

            for(int i = endPos - 1; i >=0; i--)
            {
                if (input[i] == closeChar)
                {
                    endPos = i - startPos;
                    break;
                }
            }

            return input.Substring(startPos, endPos);
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