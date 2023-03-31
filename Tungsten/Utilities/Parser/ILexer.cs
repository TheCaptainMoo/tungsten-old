using System.Text.RegularExpressions;

namespace Tungsten_Interpreter.Utilities.Parser
{
    public interface ILexer
    {
        public string Name { get; set; }
        public Regex RegexCode { get; set; }
        void AstConstructor(string[] para);
    }
}
