using Lexer;
using System.Reflection;
using Tungsten_Interpreter.Utilities.Parser;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Parser.UserMethods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter
{
    internal class Program
    {
        public class WhileLoop
        {
            public WhileLoop(int startPos, int endPos)
            {
                this.startPos = startPos;
                this.endPos = endPos;
            }

            public int startPos;
            public int endPos;
        }

        //static List<string[]> lines = new List<string[]>();

        // Loop Variables
        static IDictionary<int, WhileLoop> whileLoopInfo = new Dictionary<int, WhileLoop>();

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
            string[] lexerArr = TungstenLexer.Lexer(_args).ToArray();
            string lexerOut = "";

            foreach (string lexer in lexerArr)
            {
                lexerOut += lexer + "WS";
            }

            string[] line = lexerOut.Split("NL");

            for (int i = 0; i < line.Length; i++)
            {
                VariableSetup.lines.Add(/*i, */line[i].Split(lineChars, StringSplitOptions.RemoveEmptyEntries));
            }

            //Console.WriteLine("Lexer: " + lexerOut);

            Parser();

            goto reset;
        }

        public static void Clean()
        {
            VariableSetup.lines = new List<string[]>();
        }

        static void Parser()
        {
            IDictionary<int, int> startLine = new Dictionary<int, int>();

            bool lineCancel = false;
            bool hardCancel = false;

            //int wStartPos = 0;
            //int wEndPos = 0;
            int ifStartPos = 0;
            int ifEndPos = 0;

            for (int i = 0; i < VariableSetup.lines.Count; i++)
            {
            zero:
                #region Cleaning & Init
                string[] words = VariableSetup.lines[i];

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

                if (words.Length == 0 || words[0].StartsWith("/*"))
                {
                    if (i >= VariableSetup.lines.Count || i == VariableSetup.lines.Count - 1)
                    {
                        break;
                    }
                    else
                    {
                        i++;
                    }
                    goto zero;
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
                        words[2] = words[2].Replace(":", "");
                    }
                    catch { }
                }
                #endregion

                #region Parsing
                //LINQ For Obtaining All Methods
                var methods = from t in Assembly.GetExecutingAssembly().GetTypes()
                                where t.GetInterfaces().Contains(typeof(IMethod))
                                         && t.GetConstructor(Type.EmptyTypes) != null
                                select Activator.CreateInstance(t) as IMethod;

                //LINQ For Obtaining All Line Interactable Methods
                var linedMethods = from t in Assembly.GetExecutingAssembly().GetTypes()
                                   where t.GetInterfaces().Contains(typeof(ILineInteractable))
                                            && t.GetConstructor(Type.EmptyTypes) != null
                                   select Activator.CreateInstance(t) as ILineInteractable;

                foreach (var method in methods)
                {
                    //Console.WriteLine("INSTANCE DETECTED: " + instance);
                    if (words[0] == method.Name)
                    {
                        method.Execute(words);
                    }
                }

                foreach (var method in linedMethods)
                {
                    if (words[0] == method.Name)
                    {
                        i = method.lineExecute(words, i);
                    }
                }

                switch (words[0])
                {
                    case "WHILE":
                        #region While Loops

                        string[] whileStr = TextMethods.CalcStringForward(String.Join(" ", words, 1, words.Length - 1), '<', '>').Split(" ");
                        List<string> modifier = VariableSetup.Convert(whileStr, 0).ToList(); /*whileStr.ToList<string>()*/
                        /*for (int j = 0; j < whileStr.Length; j++)//---------------------------------------------------------------------------------- REFACTORING
                        {
                            if (variableString.ContainsKey(whileStr[j]))
                            {
                                modifier[j] = variableString[whileStr[j]];
                            }
                            else if (variableInt.ContainsKey(whileStr[j]))
                            {
                                modifier[j] = variableInt[whileStr[j]].ToString();
                            }
                            else if (variableBool.ContainsKey(whileStr[j]))
                            {
                                modifier[j] = variableBool[whileStr[j]].ToString();
                            }
                        }*/

                        //int whileIndex = 0;
                        //Console.WriteLine(Operation(modifier[0], modifier[1], modifier[2]));

                        if (Check.Operation(modifier[0], modifier[1], modifier[2]))
                        {
                            for (int j = i; j < VariableSetup.lines.Count; j++)
                            {
                                string[] wordsInLine = VariableSetup.lines[j];
                                for (int k = 0; k < wordsInLine.Length; k++)//(string word in wordsInLine)
                                {
                                    /*if (word == "RWHILE")
                                    {
                                        //bracketNum--;
                                        //Console.WriteLine("BN:" + bracketNum);
                                        goto whileLoopRedirect;
                                    }*/
                                    if(wordsInLine[k] == "SB")
                                    {
                                        if (startLine.ContainsKey(Convert.ToInt32(wordsInLine[k + 1])))
                                        {
                                            startLine[Convert.ToInt32(wordsInLine[k + 1])] = i;
                                        }
                                        else
                                        {
                                            startLine.Add(Convert.ToInt32(wordsInLine[k + 1]), i);
                                        }
                                        break;
                                    }
                                    if (wordsInLine[k] == "EB")
                                    {
                                        if (!startLine.ContainsKey(Convert.ToInt32(wordsInLine[k + 1])))
                                        {
                                            Console.WriteLine("Unreferenced Opening Bracket On Line {0}", i);
                                        }
                                        else
                                        {
                                            //i = startLine[Convert.ToInt32(wordsInLine[k + 1])];

                                            wordsInLine[k] = "WEB";
                                            //whileIndex = startLine[Convert.ToInt32(wordsInLine[k + 1])];
                                            goto whileLoopRedirect;
                                        }
                                        break;
                                    }
                                    /*foreach (char c in word)
                                    {
                                        if (c == '{')
                                        {
                                            //bracketNum++;
                                            //Console.WriteLine("BN:" + bracketNum);
                                            wStartPos = j;
                                        }

                                        if (c == '}')
                                        {
                                            //bracketNum--;
                                            //Console.WriteLine("BN: " + bracketNum);
                                            wEndPos = j;
                                            goto whileLoopRedirect;
                                        }
                                    }*/
                                }
                            }
                        }

                        whileLoopRedirect:
                        /*if (Operation(modifier[0], modifier[1], modifier[2]))
                        {
                            lines[wEndPos][0] = lines[wEndPos][0].Replace("}", "RWHILE");
                        }
                        else*/ if(!Check.Operation(modifier[0], modifier[1], modifier[2]))
                        {
                            //Console.WriteLine("Skip To End");
                            //lineCancel[whileIndex] = true;
                            lineCancel = true;
                        }

                        //Console.WriteLine();

                        #endregion
                        break;

                    /*case "RWHILE":
                        i = wStartPos - 1;
                        break;*/

                    case "WEB":
                        if (lineCancel != true)
                        {
                            i = startLine[Convert.ToInt32(words[1])] - 1;
                        }
                        else
                        {
                            words[0] = "EB"; 
                            Console.WriteLine("Close While Loop");
                            //i = startLine[Convert.ToInt32(words[1])] - 1;
                        }
                        lineCancel = false;
                        break;

                   /* case "SB":
                        #region Start Bracket

                        if (startLine.ContainsKey(Convert.ToInt32(words[1])))
                        {
                            startLine[Convert.ToInt32(words[1])] = i;
                        }
                        else
                        {
                            startLine.Add(Convert.ToInt32(words[1]), i);
                        } // Move into While Loop

                        #endregion
                        break;*/

                    /*case "EB":
                        #region End Bracket

                        if (!startLine.ContainsKey(Convert.ToInt32(words[1])))
                        {
                            Console.WriteLine("Unreferenced Opening Bracket On Line {0}", i);
                        }
                        else
                        {
                            i = startLine[Convert.ToInt32(words[1])] - 1;
                        }

                        #endregion
                        break;*/
                    
                    case "IF":
                        string[] ifStr = TextMethods.CalcStringForward(String.Join(" ", words, 1, words.Length - 1), '<', '>').Split(" ");
                        List<string> ifModifier = VariableSetup.Convert(ifStr, 0).ToList(); /*whileStr.ToList<string>()*/;

                        for (int j = i; j < VariableSetup.lines.Count; j++)
                        {
                            string[] wordsInLine = VariableSetup.lines[j];
                            foreach (string word in wordsInLine)
                            {
                                foreach (char c in word)
                                {
                                    if (c == '{')
                                    {
                                        ifStartPos = j;
                                    }

                                    if (c == '}')
                                    {
                                        ifEndPos = j;
                                    }
                                }
                            }
                        }

                        if (Check.Operation(ifModifier[0], ifModifier[1], ifModifier[2]))
                        {

                        }

                        break;
                        #endregion
                }

                if (VariableSetup.functionParameters.ContainsKey(words[0]))
                {
                    string[] args = TextMethods.ParseText(words, 0, '<', '>').Split(",");
                    List<string[]> outputList = new List<string[]>();
                    VariableSetup.lines.RemoveAt(i);

                    for (int k = 0; k < VariableSetup.functionBody[words[0]].Body.Count; k++)
                    {
                        outputList.Add(VariableSetup.functionBody[words[0]].Body[k]);
                    }

                    for (int l = 0; l < args.Length; l++)
                    {
                        string arg = args[l].Trim();
                        for (int k = 0; k < outputList.Count; k++)
                        {
                            for (int j = 0; j < outputList[k].Length; j++)
                            {
                                for (int h = 0; h < outputList[k][j].Length; h++)
                                {
                                    if (outputList[k][j] == /*functionDeclarations[words[0]].functionParameters[l]*/ VariableSetup.functionParameters[words[0]].Parameters[l])
                                    {
                                        outputList[k][j] = arg;
                                        if (k == outputList.Count - 1)
                                        {
                                            VariableSetup.functionParameters[words[0]].Parameters[l] = arg;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    for (int j = 0; j < VariableSetup.functionBody[words[0]].Body.Count; j++)
                    {
                        VariableSetup.lines.Insert(i + j, outputList[j]);
                    }

                    i--;
                    //outputList = new List<string[]>();
                }
            }
        } 

        /*public static double Evaluate(string expression)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add("expression", string.Empty.GetType(), expression);
            System.Data.DataRow row = table.NewRow();
            table.Rows.Add(row);
            return double.Parse((string)row["expression"]);
        }*/
    }
}