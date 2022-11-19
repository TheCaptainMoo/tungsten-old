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
            var methods = from t in Assembly.GetExecutingAssembly().GetTypes()
                          where t.GetInterfaces().Contains(typeof(IMethod))
                                   && t.GetConstructor(Type.EmptyTypes) != null
                          select Activator.CreateInstance(t) as IMethod;

            //LINQ For Obtaining All Line Interactable Methods
            var linedMethods = from t in Assembly.GetExecutingAssembly().GetTypes()
                               where t.GetInterfaces().Contains(typeof(ILineInteractable))
                                        && t.GetConstructor(Type.EmptyTypes) != null
                               select Activator.CreateInstance(t) as ILineInteractable;

            return 0;
        }
    }
}