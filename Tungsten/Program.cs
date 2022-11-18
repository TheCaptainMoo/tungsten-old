using System.Text;
using System.Text.RegularExpressions;

namespace Tungsten_Interpreter
{
    internal class Program
    {
        public enum TokenList
        {
            WS, //Whitespace
            STRING,
            INT,
            BOOL,
            NL, //New Line
            FUNCT,
            PRINT,
            MATH,
            UPDATE,
            DELETE,
            INPUT,
            WHILE,
            RWHILE, //Repeat While 
            IF,
            SB, //Start Bracket
            EB //End Bracket
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

        public class FunctionParam
        {
            public FunctionParam(List<string> parameters)
            {
                Parameters = parameters;
            }
            public List<string> Parameters { get; set; }
        }

        public class FunctionBody
        {
            public FunctionBody(Dictionary<int, string[]> body)
            {
                Body = body;
            }

            public Dictionary<int, string[]> Body { get; set; }
        }

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
        /*public class FunctionDeclaration
        {
            public FunctionDeclaration(List<string> functionP, Dictionary<int, string[]> functionB)
            {
                functionBody = functionB;
                functionParameters = functionP;
            }

            public List<string> functionParameters { get; set; }
            public Dictionary<int, string[]> functionBody { get; set; }
        }*/

        //static IDictionary<int, string[]> lines = new Dictionary<int, string[]>();
        static List<string[]> lines = new List<string[]>();

        // Runtime Parsed Variables
        static IDictionary<string, string> variableString = new Dictionary<string, string>();
        static IDictionary<string, int> variableInt = new Dictionary<string, int>();
        static IDictionary<string, bool> variableBool = new Dictionary<string, bool>();
        //static IDictionary<string, FunctionDeclaration> functionDeclarations = new Dictionary<string, FunctionDeclaration>();

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
            string[] lexerArr = Lexer(_args).ToArray();
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
            //lines = new Dictionary<int, string[]>();
            lines = new List<string[]>();
            variableString = new Dictionary<string, string>();
            variableInt = new Dictionary<string, int>();
            variableBool = new Dictionary<string, bool>();
            //functionDeclarations = new Dictionary<string, FunctionDeclaration>();
        }

        static List<string> Lexer(string[] args)
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
                    bracketNum++;
                }
                else if (res.StartsWith("EB") || res.StartsWith("NLEB") || res.StartsWith("WSNL"))
                {
                    bracketNum--;
                    res += "WS" + bracketNum + "NL";
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

            return ta;
        }

        static void Parser()
        {
            IDictionary<string, FunctionParam> functionParameters = new Dictionary<string, FunctionParam>();
            IDictionary<string, FunctionBody> functionBody = new Dictionary<string, FunctionBody>();

            IDictionary<int, int> startLine = new Dictionary<int, int>();

            bool lineCancel = false;

            int wStartPos = 0;
            int wEndPos = 0;
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
                    }
                    catch { }
                }
                #endregion

                #region Parsing
                switch (words[0])
                {
                    #region Variable Creation
                    case "STRING":
                        #region String Varibles
                        if (Exist(words, variableString, 1))
                            return;

                        variableString.Add(words[1], ParseText(words, 2, '[', ']'));
                        #endregion
                        break;

                    case "INT":
                        #region Integer Variables
                        if (Exist(words, variableInt, 1))
                            return;

                        try
                        {
                            double maths = Evaluate(CalcString(String.Join(" ", words, 1, words.Length - 1), '(', ')'));
                            variableInt.Add(words[1], Convert.ToInt32(maths));
                        }
                        catch
                        {
                            variableInt.Add(words[1], Convert.ToInt32(words[2]));
                        }
                        #endregion
                        break;

                    case "BOOL":
                        #region Boolean Variables
                        if (Exist(words, variableBool, 1))
                            return;

                        try
                        {
                            variableBool.Add(words[1], Convert.ToBoolean(words[2]));
                        }
                        catch
                        {
                            Console.WriteLine("Unsupported Bool Type");
                        }
                        #endregion
                        break;
                    #endregion

                    #region Variable Modification
                    case "UPDATE": // REFACTOR AND MODIFY TO SUPPORT VARIABLE INPUT
                        #region Update Variables

                        words = VariableConversion(words, 2);

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
                            variableBool.Remove(words[1]);
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
                        #endregion
                        break;

                    case "DELETE":
                        #region Delete Variables
                        string[] args = CalcStringForward(String.Join(" ", words, 1, words.Length - 1), '(', ')').Replace(",", " ").Split(" ");
                        args = args.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                        foreach (string arg in args)
                        {
                            if (variableString.ContainsKey(arg))
                            {
                                variableString.Remove(arg);
                            }
                            else if (variableInt.ContainsKey(arg))
                            {
                                variableInt.Remove(arg);
                            }
                            else if (variableBool.ContainsKey(arg))
                            {
                                variableBool.Remove(arg);
                            }
                            else
                            {
                                Console.WriteLine("{0} Doesn't Exist", arg);
                            }
                        }
                        #endregion
                        break;

                    case "INPUT":
                        #region Input Variables
                        switch (words[1])
                        {
                            case "STRING":
                                #region String Input
                                if (Exist(words, variableString, 2))
                                    return;

                                string[] inputStr = Console.ReadLine().Split(" ");
                                inputStr[0] = "[" + inputStr[0];
                                inputStr[inputStr.Length - 1] = inputStr[inputStr.Length - 1] + "]";

                                variableString.Add(words[2], ParseText(inputStr, 0, '[', ']'));
                                #endregion
                                break;

                            case "INT":
                                #region Int Input
                                if (Exist(words, variableInt, 2))
                                    return;

                                string[] inputInt = Console.ReadLine().Split(" ");
                                try
                                {
                                    double maths = Evaluate(CalcString(String.Join(" ", inputInt, 0, inputInt.Length), '(', ')'));
                                    variableInt.Add(words[2], Convert.ToInt32(maths));
                                }
                                catch
                                {
                                    variableInt.Add(words[2], Convert.ToInt32(inputInt[0]));
                                }
                                #endregion
                                break;

                            case "BOOL":
                                #region Bool Input
                                if (Exist(words, variableBool, 2))
                                    return;

                                string inputBool = Console.ReadLine();
                                try
                                {
                                    variableBool.Add(words[2], Convert.ToBoolean(inputBool));
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
                        }

                        /*
                        if (words[1] == "STRING")
                        {
                            if (Exist(words, variableString, 2))
                                return;

                            string[] input = Console.ReadLine().Split(" ");
                            input[0] = "[" + input[0];
                            input[input.Length - 1] = input[input.Length - 1] + "]";

                            variableString.Add(words[2], ParseText(input, 0, '[', ']'));
                        }
                        else if (words[1] == "INT")
                        {
                            if (Exist(words, variableInt, 2))
                                return;

                            string[] input = Console.ReadLine().Split(" ");
                            try
                            {
                                double maths = Evaluate(CalcString(String.Join(" ", input, 0, input.Length), '(', ')'));
                                variableInt.Add(words[2], Convert.ToInt32(maths));
                            }
                            catch
                            {
                                variableInt.Add(words[2], Convert.ToInt32(input[0]));
                            }
                        }
                        else if (words[1] == "BOOL")
                        {
                            if (Exist(words, variableBool, 2))
                                return;

                            string input = Console.ReadLine();
                            try
                            {
                                variableBool.Add(words[2], Convert.ToBoolean(input));
                            }
                            catch
                            {
                                Console.WriteLine("Unsupported Bool Type");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Unrecognised Type: {0}", words[1]);
                        }*/
                        #endregion 
                        break;
                    #endregion

                    #region System
                    case "MATH":
                        #region Maths Operations
                        string compute = "";
                        try
                        {
                            for (int j = 1; j < words.Length; j++)
                            {
                                if (variableInt.ContainsKey(words[j]))
                                {
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
                        #endregion
                        break;

                    case "FUNCT":
                        #region Functions
                        //Dictionary<string[], string[]> parameters = new Dictionary<string[], string[]>();
                        List<string> parameters = new List<string>();

                        Dictionary<int, string[]> body = new Dictionary<int, string[]>();

                        string str = CalcStringForward(String.Join(" ", words, 1, words.Length - 1), '<', '>'); ;
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
                        Console.WriteLine(ParseText(words, 1, '[', ']'));
                        break;

                    case "WHILE":
                        #region While Loops

                        string[] whileStr = CalcStringForward(String.Join(" ", words, 1, words.Length - 1), '<', '>').Split(" ");
                        List<string> modifier = VariableConversion(whileStr, 0).ToList(); /*whileStr.ToList<string>()*/;

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

                        //Console.WriteLine(Operation(modifier[0], modifier[1], modifier[2]));
                        if (Operation(modifier[0], modifier[1], modifier[2]))
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
                                            //goto whileLoopRedirect;
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

                        //whileLoopRedirect:
                        /*if (Operation(modifier[0], modifier[1], modifier[2]))
                        {
                            lines[wEndPos][0] = lines[wEndPos][0].Replace("}", "RWHILE");
                        }
                        else*/ if(!Operation(modifier[0], modifier[1], modifier[2]))
                        {
                            //Console.WriteLine("Skip To End");
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
                        string[] ifStr = CalcStringForward(String.Join(" ", words, 1, words.Length - 1), '<', '>').Split(" ");
                        List<string> ifModifier = VariableConversion(ifStr, 0).ToList(); /*whileStr.ToList<string>()*/;

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

                        if (Operation(ifModifier[0], ifModifier[1], ifModifier[2]))
                        {

                        }

                        break;
                        #endregion
                }

                if (functionParameters.ContainsKey(words[0]))
                {
                    string[] args = ParseText(words, 0, '<', '>').Split(",");
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

        public static bool Exist<T>(string[] content, IDictionary<string, T> value, int checkIndex)
        {
            if (value.ContainsKey(content[checkIndex]))
            {
                Console.WriteLine("Please Use The 'update' Keyword To Reassign: " + content[checkIndex]);
                return true;
            }
            return false;
        }

        public static bool Operation<T>(T val1, string op, T val2)
        {
            string v1 = val1.ToString();
            string v2 = val2.ToString();

            switch (op)
            {
                case "==":
                    if (v1 == v2)
                    {
                        return true;
                    }
                    break;

                case "!=":
                    if (v1 != v2)
                    {
                        return true;
                    }
                    break;

                case "<=":
                    try
                    {
                        if (int.Parse(v1) + 1 <= int.Parse(v2))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Cannot Compute String To Integer At {0} {1} {2}", v1, op, v2);
                        return false;
                    }

                case ">=":
                    try
                    {
                        if (int.Parse(v1) + 1 >= int.Parse(v2))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Cannot Compute String To Integer At {0} {1} {2}", v1, op, v2);
                        return false;
                    }

                case "<":
                    try
                    {
                        if (int.Parse(v1) + 1 < int.Parse(v2))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Cannot Compute String To Integer At {0} {1} {2}", v1, op, v2);
                        return false;
                    }

                case ">":
                    try
                    {
                        if (int.Parse(v1) + 1 > int.Parse(v2))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Cannot Compute String To Integer At {0} {1} {2}", v1, op, v2);
                        return false;
                    }

                default:
                    Console.WriteLine("Unknown Operator ({2}) Between {0} and {1}", val1, val2, op);
                    return false;
            }

            return false;
        }

        public static string[] VariableConversion(string[] input, int startIndex)
        {
            List<string> inputList = input.ToList();
            for (int i = 0; i < inputList.Count; i++)
            {
                for (int j = 0; j < inputList[i].Length; j++)
                {
                    if (inputList[i][j] == '(' || inputList[i][j] == ')')
                    {
                        //Console.WriteLine("Found Bracket");
                        inputList[i] = inputList[i].Remove(j, 1);
                    }
                }
            }

            for (int i = startIndex; i < input.Length; i++)
            {
                if (variableString.ContainsKey(inputList[i]))
                {
                    input[i] = input[i].Replace(inputList[i], variableString[inputList[i]]);                     
                    //variableString[inputList[i]];
                }
                else if (variableInt.ContainsKey(inputList[i]))
                {
                    //input[i] = variableInt[input[i]].ToString();
                    input[i] = input[i].Replace(inputList[i], variableInt[inputList[i]].ToString());
                    //input[i] = variableInt[inputList[i]].ToString();
                }
                else if (variableBool.ContainsKey(inputList[i]))
                {
                    //input[i] = variableBool[input[i]].ToString();
                    input[i] = input[i].Replace(inputList[i], variableBool[inputList[i]].ToString());
                    //input[i] = variableBool[inputList[i]].ToString();
                }
            }
            return input;
        }
    }
}