using Lexer;
using System.Reflection;
using System.Runtime.InteropServices;
using Tungsten_Interpreter.Utilities.Parser;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Parser.UserMethods.System;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter
{
    internal class Program
    {
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
            VariableSetup.Clean();
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

        static void Parser()
        {
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

            //LINQ For Obtaining All Methods
            var methods = from t in Assembly.GetExecutingAssembly().GetTypes()
                          where t.GetInterfaces().Contains(typeof(IMethod))
                                   && t.GetConstructor(Type.EmptyTypes) != null
                          select Activator.CreateInstance(t) as IMethod;

            List<IMethod> methodsOut = new List<IMethod>();

            foreach (IMethod method in methods)
            {
                if (!VariableSetup.usingMethods.Contains(method.Name))
                    continue;
                methodsOut.Add(method);
            }

            //LINQ For Obtaining All Line Interactable Methods
            var linedMethods = from t in Assembly.GetExecutingAssembly().GetTypes()
                               where t.GetInterfaces().Contains(typeof(ILineInteractable))
                                        && t.GetConstructor(Type.EmptyTypes) != null
                               select Activator.CreateInstance(t) as ILineInteractable;

            List<ILineInteractable> linedMethodsOut = new List<ILineInteractable>();

            foreach (ILineInteractable method in linedMethods)
            {
                if (!VariableSetup.usingMethods.Contains(method.Name))
                    continue;
                linedMethodsOut.Add(method);
            }
            #endregion

            Span<IMethod> spanMOut = CollectionsMarshal.AsSpan(methodsOut);
            Span<ILineInteractable> spanLOut = CollectionsMarshal.AsSpan(linedMethodsOut);

            for (int i = index; i < VariableSetup.lines.Count; i++)
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
 
                for(int j = 0; j < spanMOut.Length; j++)
                {
                    if (words[0] == spanMOut[j].Name)
                    {
                        spanMOut[j].Execute(words);
                    }
                }

                for (int j = 0; j < spanLOut.Length; j++)
                {
                    if (words[0] == spanLOut[j].Name)
                    {
                        spanLOut[j].lineExecute(words, i);
                    }
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
                #endregion
            }
        }
    
    }
}