using System;
using System.Text.RegularExpressions;

namespace Lexer
{
    class TungstenLexer
    {
        public enum TokenList
        {
            WS, //Whitespace
            STRING,
            INT,
            BOOL,
            TL, //Typeless Variable
            NL, //New Line
            FUNCT,
            PRINT,
            MATH,
            UPDATE,
            DELETE,
            INPUT,
            WHILE,
            IF,
            SB, //Start Bracket
            EB, //End Bracket
            ACTIVATE
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

        static List<TokenAssign> LexerInit()
        {
            List<TokenAssign> ta = new List<TokenAssign>();

            ta.Add(new TokenAssign(TokenList.WS, new Regex(@"\s+|\t")));
            ta.Add(new TokenAssign(TokenList.STRING, new Regex(@"^string$|^string:$|WSstring")));
            ta.Add(new TokenAssign(TokenList.INT, new Regex(@"^int$|^int:$|WSint")));
            ta.Add(new TokenAssign(TokenList.BOOL, new Regex(@"^bool$|^bool:$|WSbool")));
            ta.Add(new TokenAssign(TokenList.TL, new Regex(@"^var$|^var:$|WSvar|#")));
            ta.Add(new TokenAssign(TokenList.NL, new Regex(@";|\n+|\r+|[\r\n]+|\*\/")));
            ta.Add(new TokenAssign(TokenList.FUNCT, new Regex(@"^funct$|WSfunct")));
            ta.Add(new TokenAssign(TokenList.PRINT, new Regex(@"^print$|^print:$|WSprint")));
            ta.Add(new TokenAssign(TokenList.MATH, new Regex(@"^math$|^math:$|WSmath")));
            ta.Add(new TokenAssign(TokenList.UPDATE, new Regex(@"^update$|WSupdate")));
            ta.Add(new TokenAssign(TokenList.DELETE, new Regex(@"^delete$|WSdelete")));
            ta.Add(new TokenAssign(TokenList.INPUT, new Regex(@"^input$|WSinput|=>")));
            ta.Add(new TokenAssign(TokenList.WHILE, new Regex(@"^while$|WSwhile")));
            ta.Add(new TokenAssign(TokenList.IF, new Regex(@"^if$|WSif")));
            ta.Add(new TokenAssign(TokenList.SB, new Regex(@"{|WS{")));
            ta.Add(new TokenAssign(TokenList.EB, new Regex(@"}|WS}")));
            ta.Add(new TokenAssign(TokenList.ACTIVATE, new Regex(@"activate|\$")));

            return ta;
        }

        public static List<string> Lexer(string[] args)
        {
            List<TokenAssign> ta = LexerInit();
            List<string> output = new List<string>();
            string res;

            int bracketNum = 0;

            //Console.WriteLine(args.Length);


            //Lex Values
            foreach (string arg in args)
            {
                res = arg;
                for (int i = 0; i < ta.Count; i++)
                {
                    res = Regex.Replace(res, ta[i].regex.ToString(), ta[i].TokenList.ToString());
                    //Console.WriteLine(res);
                    if (res.EndsWith("SB") || res.StartsWith("EB"))
                    {
                        //bracketNum++;
                        //Console.WriteLine("BN: " + bracketNum);
                        res = res.Insert(res.Length - 2, "NL");
                        break;
                    }
                }
                if (res.EndsWith("NLSB"))
                {
                    res += "WS" + bracketNum + "NL";
                    Console.WriteLine("LINE BRACKET: " + bracketNum);
                    bracketNum++;
                }
                else if (res.StartsWith("EB") || res.StartsWith("NLEB") || res.StartsWith("WSNL"))
                {
                    bracketNum--;
                    Console.WriteLine("LINE BRACKET: " + bracketNum);
                    res += "WS" + bracketNum + "NL";
                }

                output.Add(res);
            }

            /*foreach (string outp in output)
            {
                Console.WriteLine(outp);
            }*/

            return output;
        }
    }
}