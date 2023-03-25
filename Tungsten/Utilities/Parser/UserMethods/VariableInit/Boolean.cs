using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class BooleanCreation : IMethod, IUsing, ILexer
    {
        public string Name { get; set; } = "BOOL";
        public string Path { get; set; } = "Variables";
        public Regex RegexCode { get; set; } = new Regex(@"^bool$|^bool:$|WSbool");

        // Creates a Boolean in Memory 
        public void Execute(string[] para)
        {
            try
            {
                //VariableSetup.AddEntry(para[1], Convert.ToBoolean(para[2]));
                VariableSetup.AddEntry(para[1], VariableSetup.VariableTypes.Boolean ,BitConverter.GetBytes(Convert.ToBoolean(para[2])));
            }
            catch
            {
                Console.WriteLine("Unsupported Bool Type");
            }
        }
    }
}