using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tungsten_Interpreter.Utilities.Parser.Methods
{
    public class ErrorHandling
    {
        public static void Alert(string message, ConsoleColor colour)
        {
            Console.ForegroundColor = colour;

            Console.WriteLine(message);

            Console.ResetColor();
        }
    }
}
