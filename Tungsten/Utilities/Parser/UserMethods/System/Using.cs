using System.Reflection;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods.System
{
    internal class Using : IMethod
    {
        public string Name { get; set; } = "ACTIVATE";

        public static IEnumerable<IUsing> usings = from t in Assembly.GetExecutingAssembly().GetTypes()
                     where t.GetInterfaces().Contains(typeof(IUsing))
                              && t.GetConstructor(Type.EmptyTypes) != null
                     select Activator.CreateInstance(t) as IUsing;

        public void Execute(string[] para)
        {
            string path = TextMethods.CalcString(String.Join(" ", para, 1, para.Length - 1), '<', '>');

            foreach(var u in usings)
            {
                if (u.Path == path)
                {
                    VariableSetup.usingMethods.Add(u.Name);
                }
            }
        }
    }
}
