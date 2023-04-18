using Lexer;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;
using Tungsten_Interpreter.Utilities.AST;
using Tungsten_Interpreter.Utilities.Parser;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Parser.UserMethods;
using Tungsten_Interpreter.Utilities.Parser.UserMethods;//.System;
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

        public static Dictionary<string, ILexer> methods = new Dictionary<string, ILexer>();
        public static Dictionary<string, INestedLexer> nestedMethods = new Dictionary<string, INestedLexer>();

        // Program Entry Point | Executes Lexer & Parser
        static void Main(string[] args)
        {
            // Get Methods
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.GetInterfaces().Contains(typeof(ILexer)) && type.GetConstructor(Type.EmptyTypes) != null)
                {
                    ILexer lexer = (ILexer)Activator.CreateInstance(type);
                    methods.Add(lexer.Name, lexer);
                }

                if (type.GetInterfaces().Contains(typeof(INestedLexer)) && type.GetConstructor(Type.EmptyTypes) != null)
                {
                    INestedLexer nestedLexer = (INestedLexer)Activator.CreateInstance(type);
                    nestedMethods.Add(nestedLexer.Name, nestedLexer);
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

                TungstenLexer.CreateNodes(TungstenLexer.ConstructLines(TungstenLexer.Lexer(_args)));

                Parser();
            }
        }

        static void Parser()
        {
            for(int i = 0; i < VariableSetup.nodes.Count; i++)
            {
                VariableSetup.nodes[i].Execute();
            }
        }
    }
}