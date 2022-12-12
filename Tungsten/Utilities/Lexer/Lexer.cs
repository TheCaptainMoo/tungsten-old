using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser;

namespace Lexer
{
    public class TungstenLexer
    {
        // Keywords
        public enum TokenList
        {
            WS, //Whitespace
            STRING,
            MATRIX,
            INT,
            BOOL,
            TL, //Typeless Variable
            NL, //New Line
            FUNCT,
            PRINT,
            PRINTIN,
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

        // Template For Creating Tokens
        /*public class TokenAssign
        {
            public TokenAssign(TokenList tokenList, Regex regex)
            {
                TokenList = tokenList;
                this.regex = regex;
            }

            public TokenList TokenList { get; set; }
            public Regex regex { get; set; }
        }*/

        public sealed class TokenAssign
        {
            public TokenAssign(string token, Regex regex) 
            { 
                Token = token;
                this.regex = regex;
            }

            public string Token { get; set; }
            public Regex regex { get; set; }
        }

        static List<TokenAssign> LexerInit()
        {
            // List of Tokens & Relevant Syntax

            // Handle Syntax
            List<TokenAssign> ta = new List<TokenAssign>()
            {
                new TokenAssign("WS", new Regex(@"\s+|\t")),
                new TokenAssign("NL", new Regex(@";|\n+|\r+|[\r\n]+|\*\/")),
                new TokenAssign("SB", new Regex(@"{|WS{")),
                new TokenAssign("EB", new Regex(@"}|WS}"))
            };

            // Handle Keywords
            var tokens = from t in Assembly.GetExecutingAssembly().GetTypes()
                          where t.GetInterfaces().Contains(typeof(ILexer))
                                   && t.GetConstructor(Type.EmptyTypes) != null
                          select Activator.CreateInstance(t) as ILexer;

            foreach (var t in tokens)
            {
                ta.Add(new TokenAssign(t.Name, t.RegexCode));
            }

            return ta;
        }

        public static List<string> Lexer(string[] args)
        {
            List<TokenAssign> ta = LexerInit();
            List<string> output = new List<string>();
            string res;

            int bracketNum = 0;

            //Lex Values
            foreach (string arg in args)
            {
                res = arg;
                for (int i = 0; i < ta.Count; i++)
                {
                    // Replace Syntax With Token
                    res = Regex.Replace(res, ta[i].regex.ToString(), /*ta[i].TokenList.ToString()*/ ta[i].Token);
                    if (res.EndsWith("SB") || res.StartsWith("EB"))
                    {
                        res = res.Insert(res.Length - 2, "NL");
                        break;
                    }
                }

                // Code For Tokenising 
                if (res.EndsWith("NLSB"))
                {
                    res += "WS" + bracketNum + "NL";
                    bracketNum++;
                }
                else if (res.StartsWith("EB") || res.StartsWith("NLEB") || res.StartsWith("WSNL"))
                {
                    bracketNum--;
                    res += "WS" + bracketNum + "NL";
                }

                output.Add(res);
            }

            #if DEBUG
            foreach (string outp in output)
            {
                Console.WriteLine(outp);
            }
            #endif

            return output;
        }
    }
}