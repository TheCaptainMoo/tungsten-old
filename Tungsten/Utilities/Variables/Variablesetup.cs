using System.Collections;
using System.Reflection;
using Tungsten_Interpreter.Utilities.Parser;
using Tungsten_Interpreter.Utilities.Parser.Methods;

namespace Tungsten_Interpreter.Utilities.Variables
{
    public class VariableSetup
    {
        public static Hashtable globalVar = new Hashtable();
        //public static List<string> usingPath = new List<string>();
        public static List<string> usingMethods = new List<string>() { "ACTIVATE" };

        public static List<string[]> lines = new List<string[]>();

        //Function Variables
        public static IDictionary<string, FunctionParam> functionParameters = new Dictionary<string, FunctionParam>();
        public static IDictionary<string, FunctionBody> functionBody = new Dictionary<string, FunctionBody>();

        //While Variables
        public static IDictionary<int, int> whileStartPosition = new Dictionary<int, int>();
        public static IDictionary<int, int> whileEndPosition = new Dictionary<int, int>();

        public static void AddEntry<T>(string name, T value)
        {
            if (globalVar.ContainsKey(name))
            {
                Console.WriteLine("Use 'update' to edit: " + name);
            }
            else
            {
                globalVar.Add(name, value);
            }
        }

        public static void RemoveEntry(string name)
        {
            if (globalVar.ContainsKey(name))
            {
                globalVar.Remove(name);
            }
            else
            {
                Console.WriteLine(name + " Doesn't Exist");
            }
        }

        public static void UpdateEntry<T>(string name, T newValue)
        {
            if (globalVar.ContainsKey(name))
            {
                globalVar[name] = newValue;
            }
            else
            {
                Console.WriteLine(name + " Doesn't Exist");
            }
        }

        public static string[] Convert(string[] input, int startIndex)
        {
            List<string> inputList = input.ToList();
            for (int i = 0; i < inputList.Count; i++)
            {
                if (TextMethods.CalcString(inputList[i], '<', '>') != inputList[i])
                {
                    inputList[i] = inputList[i].Replace("<" + TextMethods.CalcString(inputList[i], '<', '>') + ">", "");
                }

                for (int j = 0; j < inputList[i].Length; j++)
                {
                    if (inputList[i][j] == '(' || inputList[i][j] == ')' || inputList[i][j] == '[' || inputList[i][j] == ']')
                    {
                        //Console.WriteLine("Found Bracket");
                        inputList[i] = inputList[i].Remove(j, 1);
                    }
                }
            }

            for(int i = startIndex; i < input.Length; i++)
            {
                //Console.WriteLine(globalVar[inputList[i].Length]);

                if (globalVar.ContainsKey(inputList[i]))
                {
                    try
                    {
                        // Handle String[]
                        //string[] val = globalVar[inputList[i]] as string[];
                        //input[i] = val[System.Convert.ToInt32(input[i+1].Substring(1, input[i+1].Length-2))];
                        //input[i + 1] = "";
                        string[] val = (string[])globalVar[inputList[i]];
                        input[i] = val[System.Convert.ToInt32(TextMethods.CalcString(input[i], '<', '>'))];
                    }
                    catch
                    {
                        // Handle Non-Array
                        input[i] = input[i].Replace(inputList[i], globalVar[inputList[i]].ToString());
                    }
                }
            }

            return input;
        }
    
        public static string Convert(string input)
        {
            if (globalVar.ContainsKey(input))
            {
                return globalVar[input].ToString();
            }

            return input;
        }

        public static void Clean()
        {
            globalVar = new Hashtable();
            //usingPath = new List<string>();
            usingMethods = new List<string>() { "ACTIVATE" };
            lines = new List<string[]>();

            functionParameters = new Dictionary<string, FunctionParam>();
            functionBody = new Dictionary<string, FunctionBody>();

            whileStartPosition = new Dictionary<int, int>();
            whileEndPosition = new Dictionary<int, int>();
        }
    }
}