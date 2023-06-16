using Tungsten_Interpreter.Utilities.AST;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods.Functions
{
	internal class Delete : ICallable
	{
        public string FunctionName { get; set; } = "delete";
        public void Function(List<AbstractSyntaxTree.AstNode> para) 
        {
            try
            {
                //AbstractSyntaxTree.VariableNode node = (AbstractSyntaxTree.VariableNode)para[0];
                //VariableSetup.RemoveEntry(node.Name);
                for(int i = 0;  i < para.Count; i++)
                {
                    AbstractSyntaxTree.VariableNode node = (AbstractSyntaxTree.VariableNode)para[i];
                    VariableSetup.RemoveEntry(node.Name);
                }
            }
            catch
            {
                ErrorHandling.Alert("Could not delete variable.", ConsoleColor.Red);
            }
        }
    }

    internal class Update : ICallable
    {
        public string FunctionName { get; set; } = "update";
        public void Function(List<AbstractSyntaxTree.AstNode> para)
        {
            try
            {
                AbstractSyntaxTree.ValueNode node = (AbstractSyntaxTree.ValueNode)para[0];
                switch (node.Execute())
                {
                    case "string":
                        break;

                    case "int":
                        break;

                    case "bool":
                        break;
                }
            } 
            catch
            {

            }
        }
    }
}
