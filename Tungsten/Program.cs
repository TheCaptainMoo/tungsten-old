using Lexer;
using System.Reflection;
using Tungsten_Interpreter.Utilities.Parser;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Parser.UserMethods.System;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter
{
    internal class Program
    {
        public static readonly string[] splitChars = 
        {
            " ",
            //"\n",
            "\r",
            "\t",
            //";"
        };

        /*public static readonly string[] lineChars =
        {
            "WS",
            "\0",
            "NL"
        };*/

        public static Dictionary<string, IMethod> methods = new Dictionary<string, IMethod>();
        public static Dictionary<string, ILineInteractable> linedMethods = new Dictionary<string, ILineInteractable>();
        public static Dictionary<string, ILateMethod> lateMethods = new Dictionary<string, ILateMethod>();

        // Program Entry Point | Executes Lexer & Parser
        static void Main(string[] args)
        {
            // Get Methods
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.GetInterfaces().Contains(typeof(IMethod)) && type.GetConstructor(Type.EmptyTypes) != null)
                {
                    IMethod method = (IMethod)Activator.CreateInstance(type);
                    methods.Add(method.Name, method);
                }

                if (type.GetInterfaces().Contains(typeof(ILineInteractable)) && type.GetConstructor(Type.EmptyTypes) != null)
                {
                    ILineInteractable linedMethod = (ILineInteractable)Activator.CreateInstance(type);
                    linedMethods.Add(linedMethod.Name, linedMethod);
                }

                if (type.GetInterfaces().Contains(typeof(ILateMethod)) && type.GetConstructor(Type.EmptyTypes) != null)
                {
                    ILateMethod lateMethod = (ILateMethod)Activator.CreateInstance(type);
                    lateMethods.Add(lateMethod.Name, lateMethod);
                }
            }

            // Loop For Each Script
            while (true)
            {
                VariableSetup.Clean();
                string path = Console.ReadLine().Replace("\"", "");
                StreamReader sr = new StreamReader(path);

                string[] _args = sr.ReadToEnd().Split(splitChars, StringSplitOptions.RemoveEmptyEntries);

                sr.Close();

                TungstenLexer.ConstructLines(TungstenLexer.Lexer(_args));

                Parser();
            }
        }

        static void Parser()
        {
            // Generates Valid Using Statements
            #region Preprocessing
            Using u = new Using();
            int index = 0;

            for(int i = 0; i < VariableSetup.lines.Count; i++)
            {
                Span<string> words = VariableSetup.lines[i];
                if(words != null && words.Length != 0 && !words[0].StartsWith("/*"))
                {
                    if (words[0] == "ACTIVATE")
                    {
                        u.Execute(words.ToArray());
                    }
                    else
                    {
                        index = i;
                        break;
                    }
                }
            }

            #endregion

            // Loops Through Each Line
            for (int i = index; i < VariableSetup.lines.Count; i++)
            {
            zero:
                // Cleans Lexer Input
                #region Cleaning & Init
                string[] words = VariableSetup.lines[i];

                words = words.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                /*if (words != null)
                {
                    try
                    {
                        words[0] = words[0].ToUpper();
                    }
                    catch { }
                }*/

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

                /*if ((words[0] == "STRING" || words[0] == "INT" || words[0] == "BOOL") && !words[1].EndsWith(':'))
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
                }*/
                #endregion

                // Runs Code
                #region Parsing

                if (methods.ContainsKey(words[0]))
                {
                    methods[words[0]].Execute(words);
                } 
                else if (linedMethods.ContainsKey(words[0]))
                {
                    i = linedMethods[words[0]].lineExecute(words, i);
                }
                else if (lateMethods.ContainsKey(words[1]))
                {
                    lateMethods[words[1]].LateExecute(words);
                }
                else if (VariableSetup.functionParameters.ContainsKey(words[0]))
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
                                if (outputList[k][j] == VariableSetup.functionParameters[words[0]].Parameters[l])
                                {
                                    outputList[k][j] = arg;
                                }
                            }

                            if (k == outputList.Count - 1)
                            {
                                VariableSetup.functionParameters[words[0]].Parameters[l] = arg;
                            }
                        }
                    }

                    for (int j = 0; j < VariableSetup.functionBody[words[0]].Body.Count; j++)
                    {
                        VariableSetup.lines.Insert(i + j, outputList[j]);
                    }

                    i--;
                }
               
                #endregion
            }

        }
    
    }
}