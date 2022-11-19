using System.Collections;

namespace Tungsten_Interpreter.Utilities.Variables
{
    public class VariableSetup
    {
        public static Hashtable globalVar = new Hashtable();

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
                for (int j = 0; j < inputList[i].Length; j++)
                {
                    if (inputList[i][j] == '(' || inputList[i][j] == ')')
                    {
                        //Console.WriteLine("Found Bracket");
                        inputList[i] = inputList[i].Remove(j, 1);
                    }
                }
            }

            for(int i = startIndex; i < input.Length; i++)
            {
                if (globalVar.ContainsKey(inputList[i]))
                {
                    input[i] = input[i].Replace(inputList[i], (string)globalVar[inputList[i]]);
                }
            }

            return input;
        }
    }
}