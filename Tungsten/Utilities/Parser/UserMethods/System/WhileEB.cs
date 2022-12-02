using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods.System
{
    internal class WhileEB : ILineInteractable, IUsing
    {
        public string Name { get; set; } = "WEB";
        public string Path { get; set; } = "System";

        // Returns Back to the Start of the While Loop
        public int lineExecute(string[] para, int startLine)
        {
            return VariableSetup.whileStartPosition[Convert.ToInt32(para[1])] - 2;
        }
    }
}
