using Tungsten_Interpreter.Utilities.Parser.UserMethods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser
{
    internal interface ICallable
    {
        public string FunctionName { get; set; }
        public AST.AbstractSyntaxTree.AstNode Function(List<AST.AbstractSyntaxTree.AstNode> para);
    } 
}