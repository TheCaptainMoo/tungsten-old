using System.Text.RegularExpressions;
using static Tungsten_Interpreter.Program;

namespace Tungsten_Interpreter
{
    internal class Program
    {
        public enum TokenList
        {
            WS,
            LeftBracket,
            RightBracket,
            MSG,
            STRING,
            INT,
            BOOL,
            Colon
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
        
        public static IDictionary<char, string> lexTable=new Dictionary<char, string>()
        {
            {' ', "WS"},
            {'[', "LeftBracket"},
            {']', "RightBracket" },
            {'"', "STR" },
            {':', "Colon"}
        };

        static void Main(string[] args)
        {
            string[] _args = Console.ReadLine().Split("\t");

            foreach (string _arg in _args)
            {
                //Console.WriteLine(_arg);
                //Lexer(_arg);
            }

            Lexer(_args);
        }

        static void Lexer(string[] args)
        {
            List<TokenAssign> ta = LexerInit();
            string res;

            Console.WriteLine(args.Length);


            //Lex Values
            foreach (string arg in args)
            {
                res = arg;
                for (int i = 0; i < ta.Count; i++)
                {
                    res = Regex.Replace(res, ta[i].regex.ToString(), ta[i].TokenList.ToString());
                    Console.WriteLine(res);
                }
            }

            
        }

        static List<TokenAssign> LexerInit()
        {
            List<TokenAssign> ta = new List<TokenAssign>();

            ta.Add(new TokenAssign(TokenList.WS, new Regex(@"\s+")));
            ta.Add(new TokenAssign(TokenList.LeftBracket, new Regex("\\[")));
            ta.Add(new TokenAssign(TokenList.RightBracket, new Regex("\\]")));
            ta.Add(new TokenAssign(TokenList.MSG, new Regex('"'.ToString())));
            ta.Add(new TokenAssign(TokenList.STRING, new Regex(@"string|string:")));
            ta.Add(new TokenAssign(TokenList.INT, new Regex(@"int|int:")));
            ta.Add(new TokenAssign(TokenList.BOOL, new Regex(@"bool|bool:")));
            ta.Add(new TokenAssign(TokenList.Colon, new Regex(@":")));

            return ta;
        }

        static void Parser(string[] parsedArgs)
        {
            IDictionary<string, string> variables = new Dictionary<string, string>();

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