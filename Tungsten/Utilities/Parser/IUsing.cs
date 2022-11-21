using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tungsten_Interpreter.Utilities.Parser
{
    internal interface IUsing
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
