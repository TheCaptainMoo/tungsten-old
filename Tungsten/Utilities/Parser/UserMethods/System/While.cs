using System.Collections.Immutable;
using System.Reflection;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class While : ILineInteractable
    {
        public string Name { get; set; } = "WHILE";

        public int lineExecute(string[] para, int lineNumber)
        {
            string[] whileStr = TextMethods.CalcStringForward(String.Join(" ", para, 1, para.Length - 1), '<', '>').Split(" ");
            List<string> modifier = VariableSetup.Convert(whileStr, 0).ToList();        

            //var methods = from t in Assembly.GetExecutingAssembly().GetTypes()
            //              where t.GetInterfaces().Contains(typeof(IMethod))
            //                       && t.GetConstructor(Type.EmptyTypes) != null
            //              select Activator.CreateInstance(t) as IMethod;

            //LINQ For Obtaining All Line Interactable Methods
            //var linedMethods = from t in Assembly.GetExecutingAssembly().GetTypes()
            //                   where t.GetInterfaces().Contains(typeof(ILineInteractable))
            //                            && t.GetConstructor(Type.EmptyTypes) != null
            //                   select Activator.CreateInstance(t) as ILineInteractable;

            int startPos = 0;
            int endPos = 0;

            int startIndex = -1;

            

            for(int i = lineNumber; i < VariableSetup.lines.Count; i++)
            {
                
                string[] wordsInLine = VariableSetup.lines[i];
                for(int j = 0; j < wordsInLine.Length; j++)
                {
                    if(wordsInLine[j] == "SB")
                    {
                        startPos = i;
                        try
                        {
                            VariableSetup.whileStartPosition.Add(Convert.ToInt32(wordsInLine[j + 1]), startPos);
                        }
                        catch
                        {
                            //VariableSetup.whileStartPosition[j] = startPos;
                        }

                        if(startIndex == -1)
                        {
                            startIndex = Convert.ToInt32(wordsInLine[j + 1]);
                        }

                        else if (!Check.Operation(modifier[0], modifier[1], modifier[2]))
                        {
                            Console.WriteLine("KILL LOOP");
                            return VariableSetup.whileEndPosition[startIndex] + 2;
                        }
                    }
                    else if(wordsInLine[j] == "EB" || wordsInLine[j] == "WEB")
                    {
                        endPos = i-1;
                        try
                        {
                            VariableSetup.whileEndPosition.Add(Convert.ToInt32(wordsInLine[j + 1]), endPos);
                        }
                        catch
                        {
                            //VariableSetup.whileEndPosition[j] = endPos;
                        }

                        if(Check.Operation(modifier[0], modifier[1], modifier[2]))
                        {
                            wordsInLine[j] = "WEB";
                            if (Convert.ToInt32(wordsInLine[j+1]) == startIndex)
                            {
                                return startPos;
                            }
                        }
                        else
                        {
                            Console.WriteLine("EXIT");
                            //return endPos + 1;
                            return Convert.ToInt32(VariableSetup.whileEndPosition[Convert.ToInt32(wordsInLine[j + 1])]) + 1;
                        }
                    } 
                }
            }

            if(Check.Operation(modifier[0], modifier[1], modifier[2]))
            {
                return startPos;
            }
            /*if (Check.Operation(modifier[0], modifier[1], modifier[2]))
            {
                modifier = VariableSetup.Convert(TextMethods.CalcStringForward(String.Join(" ", para, 1, para.Length - 1), '<', '>').Split(" "), 0).ToList();


                for (int i = 0; i < body.Count; i++)
                {
                    if (body[i] == null || body[i].Length <= 0)
                    {
                        continue;
                    }

                    if ((body[i][0] == "STRING" || body[i][0] == "INT" || body[i][0] == "BOOL") && !body[i][1].EndsWith(':'))
                    {
                        Console.WriteLine("Relevant Assigner ':' Expected At: '" + body[i][1] + "'");
                        return endPos;
                    }
                    else
                    {
                        try
                        {
                            body[i][1] = body[i][1].Replace(":", "");
                            body[i][2] = body[i][2].Replace(":", "");
                        }
                        catch { }
                    }//---------------------------------------------------------------------------------REFACTOR

                    foreach (var method in methods)
                    {
                        //Console.WriteLine("INSTANCE DETECTED: " + instance);
                        if (body[i][0] == method.Name)
                        {
                            method.Execute(body[i]);
                        }
                    }

                    foreach (var method in linedMethods)
                    {
                        if (body[i][0] == method.Name)
                        {
                            i = method.lineExecute(body[i], i);
                        }
                    }
                }
                goto cycle;
                //return startPos - 1;
            }*/
            else
            {
                return endPos + 1;
            }
        }
    }
}