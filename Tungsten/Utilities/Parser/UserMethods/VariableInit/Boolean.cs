using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class BooleanCreation : IMethod, IUsing
    {
        public string Name { get; set; } = "BOOL";
        public string Path { get; set; } = "Variables";

        public void Execute(string[] para)
        {
            try
            {
                VariableSetup.AddEntry(para[1], Convert.ToBoolean(para[2]));
            }
            catch
            {
                Console.WriteLine("Unsupported Bool Type");
            }
        }
    }
}