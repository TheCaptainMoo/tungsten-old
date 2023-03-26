using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tungsten_Interpreter.Utilities.Parser
{
    public interface ILateMethod
    {
        string Name { get; set; }
        //int Position { get; set; }
        void LateExecute(string[] para);
    }
}
