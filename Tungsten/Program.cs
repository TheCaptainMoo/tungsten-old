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

        static List<string[]> lines = new List<string[]>();

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
                lines.Add(/*i, */line[i].Split(lineChars, StringSplitOptions.RemoveEmptyEntries));
            }

            //Console.WriteLine("Lexer: " + lexerOut);

            Parser();

            goto reset;
        }

        public static void Clean()
        {
            lines = new List<string[]>();
        }

        static void Parser()
        {
            IDictionary<string, FunctionParam> functionParameters = new Dictionary<string, FunctionParam>();
            IDictionary<string, FunctionBody> functionBody = new Dictionary<string, FunctionBody>();

            IDictionary<int, int> startLine = new Dictionary<int, int>();

            bool lineCancel = false;
            bool hardCancel = false;

            //int wStartPos = 0;
            //int wEndPos = 0;
            int ifStartPos = 0;
            int ifEndPos = 0;

            for (int i = 0; i < lines.Count; i++)
            {
            zero:
                #region Cleaning & Init
                string[] words = lines[i];

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
                    if (i >= lines.Count || i == lines.Count - 1)
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
                    #region Variable Creation
                    case "STRING":
                        #region String Varibles

                        /*if (Exist(words, variableString, 1))
                            return;

                        variableString.Add(words[1], ParseText(words, 2, '[', ']'));*/

                        //VariableSetup.AddEntry(words[1], TextMethods.ParseText(words, 2, '[', ']'));
                        //Console.WriteLine(VariableSetup.globalVar[words[1]]);

                        #endregion
                        break;

                    case "INT":
                        #region Integer Variables
                        //if (Exist(words, variableInt, 1))
                            //return;

                        /*try
                        {
                            double maths = Maths.Evaluate(TextMethods.CalcString(String.Join(" ", words, 1, words.Length - 1), '(', ')'));
                            //variableInt.Add(words[1], Convert.ToInt32(maths));
                            VariableSetup.AddEntry(words[1], Convert.ToInt32(maths));
                        }
                        catch
                        {
                            //variableInt.Add(words[1], Convert.ToInt32(words[2]));
                            VariableSetup.AddEntry(words[1], Convert.ToInt32(words[2]));
                        }*/
                        #endregion
                        break;

                    case "BOOL":
                        #region Boolean Variables
                        //if (Exist(words, variableBool, 1))
                            //return;

                        /*try
                        {
                            //variableBool.Add(words[1], Convert.ToBoolean(words[2]));
                            VariableSetup.AddEntry(words[1], Convert.ToBoolean(words[2]));
                        }
                        catch
                        {
                            Console.WriteLine("Unsupported Bool Type");
                        }*/
                        #endregion
                        break;
                    #endregion

                    #region Variable Modification
                    case "UPDATE": // REFACTOR AND MODIFY TO SUPPORT VARIABLE INPUT
                        #region Update Variables

                        /*words = VariableSetup.Convert(words, 2);

                        switch (words[2])
                        {
                            case "STRING":
                                VariableSetup.UpdateEntry(words[3], words[4]);
                                break;

                            case "INT":
                                try
                                {
                                    double maths = Maths.Evaluate(TextMethods.CalcString(String.Join(" ", words, 1, words.Length - 1), '(', ')'));
                                    VariableSetup.UpdateEntry(words[3], maths);
                                }
                                catch
                                {
                                    VariableSetup.UpdateEntry(words[3], words[4]);
                                }
                                break;

                            case "BOOL":
                                try
                                {
                                    VariableSetup.UpdateEntry(words[3], Convert.ToBoolean(words[4]));
                                }
                                catch
                                {
                                    Console.WriteLine("Unsupported Bool Type");
                                }
                                break;
                        }*/
                        #endregion
                        break;

                    case "DELETE":
                        #region Delete Variables
                        /*string[] args = TextMethods.CalcStringForward(String.Join(" ", words, 1, words.Length - 1), '(', ')').Replace(",", " ").Split(" ");
                        args = args.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                        foreach (string arg in args)
                        {
                            VariableSetup.RemoveEntry(arg);
                        }*/
                        #endregion
                        break;

                    case "INPUT":
                        #region Input Variables
                        /*switch (words[1])
                        {
                            case "STRING":
                                #region String Input
                                //if (Exist(words, variableString, 2))
                                    //return;

                                string[] inputStr = Console.ReadLine().Split(" ");
                                inputStr[0] = "[" + inputStr[0];
                                inputStr[inputStr.Length - 1] = inputStr[inputStr.Length - 1] + "]";

                                //variableString.Add(words[2], ParseText(inputStr, 0, '[', ']'));
                                VariableSetup.AddEntry(words[2], TextMethods.ParseText(inputStr, 0, '[', ']'));
                                #endregion
                                break;

                            case "INT":
                                #region Int Input
                                //if (Exist(words, variableInt, 2))
                                    //return;

                                string[] inputInt = Console.ReadLine().Split(" ");
                                try
                                {
                                    double maths = Maths.Evaluate(TextMethods.CalcString(String.Join(" ", inputInt, 0, inputInt.Length), '(', ')'));
                                    //variableInt.Add(words[2], Convert.ToInt32(maths));
                                    VariableSetup.AddEntry(words[2], Convert.ToInt32(maths));
                                }
                                catch
                                {
                                    //variableInt.Add(words[2], Convert.ToInt32(inputInt[0]));
                                    VariableSetup.AddEntry(words[2], Convert.ToInt32(inputInt[0]));
                                }
                                #endregion
                                break;

                            case "BOOL":
                                #region Bool Input
                                //if (Exist(words, variableBool, 2))
                                    //return;

                                string inputBool = Console.ReadLine();
                                try
                                {
                                    //variableBool.Add(words[2], Convert.ToBoolean(inputBool));
                                    VariableSetup.AddEntry(words[2], Convert.ToBoolean(inputBool));
                                }
                                catch
                                {
                                    Console.WriteLine("Unsupported Bool Type");
                                }
                                #endregion
                                break;

                            default:
                                Console.WriteLine("Unrecognised Type: {0}", words[1]);
                                break;
                        }*/

                        #endregion 
                        break;
                    #endregion

                    #region System
                    case "MATH":
                        #region Maths Operations
                        /*string compute = "";
                        try
                        {
                            for (int j = 1; j < words.Length; j++)
                            {
                                if (VariableSetup.globalVar.ContainsKey(words[j]))
                                {
                                    compute += VariableSetup.globalVar[words[j]];
                                }
                                else
                                {
                                    compute += words[j];
                                }
                            }
                            Console.WriteLine(Maths.Evaluate(compute));
                        }
                        catch
                        {
                            Console.WriteLine(Maths.Evaluate(TextMethods.CalcString(String.Join(" ", words, 1, words.Length - 1), '(', ')')));
                        }*/
                        #endregion
                        break;

                    case "FUNCT":
                        #region Functions
                        //Dictionary<string[], string[]> parameters = new Dictionary<string[], string[]>();
                        List<string> parameters = new List<string>();

                        Dictionary<int, string[]> body = new Dictionary<int, string[]>();

                        string str = TextMethods.CalcStringForward(String.Join(" ", words, 1, words.Length - 1), '<', '>'); ;
                        string[] para;

                        //List<string> type = new List<string>();
                        List<string> name = new List<string>();

                        para = str.Replace(",", "").Split(" ");
                        para[0] = para[0].ToUpper();

                        for (int j = 0; j < para.Length; j += 2)
                        {
                            //type.Add(para[j]);
                            //name.Add(para[j+1]);
                            name.Add(para[j + 1]);
                        }

                        int startPos = 0;
                        int endPos = 0;
                        int index = 0;

                        for (int j = i; j < lines.Count; j++)
                        {
                            string[] wordsInLine = lines[j];
                            foreach (string word in wordsInLine)
                            {
                                foreach (char c in word)
                                {
                                    if (c == '{')
                                    {
                                        startPos = j;
                                    }

                                    if (c == '}')
                                    {
                                        endPos = j - 1;
                                    }
                                }
                            }
                        }

                        while (startPos < endPos)
                        {
                            body.Add(index++, lines[startPos + 1]);

                            startPos++;
                        }

                        //parameters.Add(type.ToArray(), name.ToArray());

                        //functionDeclarations.Add(words[1].ToUpper(), new FunctionDeclaration(name, body));
                        //functionDeclarations.Add("ILOVETODEBUG", new FunctionDeclaration(name, body));

                        functionParameters.Add(words[1].ToUpper(), new FunctionParam(name));
                        functionBody.Add(words[1].ToUpper(), new FunctionBody(body));

                        i = endPos + 1;
                        #endregion
                        break;

                    case "PRINT":
                        //Console.WriteLine(TextMethods.ParseText(words, 1, '[', ']'));
                        //Print.Execute(words);
                        break;

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
                            for (int j = i; j < lines.Count; j++)
                            {
                                string[] wordsInLine = lines[j];
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

                        for (int j = i; j < lines.Count; j++)
                        {
                            string[] wordsInLine = lines[j];
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

                if (functionParameters.ContainsKey(words[0]))
                {
                    string[] args = TextMethods.ParseText(words, 0, '<', '>').Split(",");
                    List<string[]> outputList = new List<string[]>();
                    lines.RemoveAt(i);

                    for (int k = 0; k < functionBody[words[0]].Body.Count; k++)
                    {
                        outputList.Add(functionBody[words[0]].Body[k]);
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
                                    if (outputList[k][j] == /*functionDeclarations[words[0]].functionParameters[l]*/ functionParameters[words[0]].Parameters[l])
                                    {
                                        outputList[k][j] = arg;
                                        if (k == outputList.Count - 1)
                                        {
                                            functionParameters[words[0]].Parameters[l] = arg;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    for (int j = 0; j < functionBody[words[0]].Body.Count; j++)
                    {
                        lines.Insert(i + j, outputList[j]);
                    }

                    i--;
                    //outputList = new List<string[]>();
                }
                #endregion
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