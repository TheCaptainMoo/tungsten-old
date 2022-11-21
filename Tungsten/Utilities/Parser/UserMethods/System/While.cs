using System.Collections.Immutable;
using System.Reflection;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class While : ILineInteractable, IUsing
    {
        public string Name { get; set; } = "WHILE";
        public string Path { get; set; } = "System";

        public int lineExecute(string[] para, int lineNumber)
        {
            string[] whileStr = TextMethods.CalcString(String.Join(" ", para, 1, para.Length - 1), '<', '>').Split(" ");
            List<string> modifier = VariableSetup.Convert(whileStr, 0).ToList();        

            int startPos = 0;
            int endPos = 0;

            int startIndex = -1;

            for(int i = lineNumber; i < VariableSetup.lines.Count; i++)
            {
                
                string[] wordsInLine = VariableSetup.lines[i];
                for(int j = 0; j < wordsInLine.Length; j++)
                {
                    if (wordsInLine[j] == "SB" && startIndex <= -1)
                    {
                        startIndex = Convert.ToInt32(wordsInLine[j + 1]);
                    }
                    if (wordsInLine[j] == "SB" && Convert.ToInt32(wordsInLine[j + 1]) == startIndex)
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
                        if (!Check.Operation(modifier[0], modifier[1], modifier[2]))
                        {
                            //Console.WriteLine("KILL LOOP");
                            return VariableSetup.whileEndPosition[startIndex] + 2;
                        }
                    }
                    else if((wordsInLine[j] == "EB" || wordsInLine[j] == "WEB") && Convert.ToInt32(wordsInLine[j+1]) == startIndex)
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
                            //Console.WriteLine("EXIT");
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