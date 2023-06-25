using System.Text;
using System.Text.RegularExpressions;
using static Tungsten_Interpreter.Utilities.AST.AbstractSyntaxTree;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;
using Tungsten_Interpreter.Utilities.AST;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods.System
{
    internal class Return : ILexer
    {
        public string Name { get; set; } = "RETURN";
        public Regex RegexCode { get; set; } = new Regex(@"^return$|WSreturn");
        public AstNode AstConstructor(string[] para)
        {
            switch (para[1])
            {
                case "STRING":
                    //return new ReturnNode(new StringAnalysisNode(TextMethods.StringAstParse(para, 1)), VariableSetup.VariableTypes.String);
                    return new ReturnNode(new StringAnalysisNode(TextMethods.AstParse(para, 2, VariableSetup.VariableTypes.String)), VariableSetup.VariableTypes.String);

                case "INT":
                    return new ReturnNode(new Container(TextMethods.AstParse(para, 2, VariableSetup.VariableTypes.Int)), VariableSetup.VariableTypes.Int);

                case "BOOL":
                    break;
            }

            return null;
        }

        public class ReturnNode : AstNode 
        {
            public ReturnNode(AstNode value, VariableSetup.VariableTypes type) 
            {
                Value = value;
                Type = type;
            }

            public override object? Execute()
            {
                switch (Type)
                {
                    case VariableSetup.VariableTypes.String:
                        return Encoding.UTF8.GetBytes((string)Value.Execute());

                    case VariableSetup.VariableTypes.Int:
                        StringBuilder sb = new StringBuilder();
                        
                        if(Value is Container val)
                        {
                            for(int i = 0;  i < val.Children.Count; i++)
                            {
                                if (val.Children[i] is VariableNode)
                                {
                                    sb.Append(Encoding.UTF8.GetString(ByteManipulation.IntToChars(ByteManipulation.GetValue(val.Children[i]))));
                                }
                                else
                                {
                                    sb.Append(Encoding.UTF8.GetString(ByteManipulation.GetValue(val.Children[i])));
                                }
                            }
                        }

                        return Encoding.UTF8.GetBytes(sb.ToString());

                    case VariableSetup.VariableTypes.Boolean: 
                        break;
                }

                return null;
            }

            public AstNode Value { get; set; }
            public VariableSetup.VariableTypes Type { get; set; }
        }

        public class Container : AstNode
        {
            public Container(List<AstNode> children)
            {
                Children = children;
            }

            public override object? Execute() { return null; }

            public List<AstNode> Children { get; set;}
        }
    }
}
