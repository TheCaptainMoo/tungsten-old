﻿using System.Text.RegularExpressions;
using Tungsten_Interpreter.Utilities.Parser.Methods;
using Tungsten_Interpreter.Utilities.Variables;

namespace Tungsten_Interpreter.Utilities.Parser.UserMethods
{
    public class BooleanCreation : ILexer
    {
        public string Name { get; set; } = "BOOL";
        public Regex RegexCode { get; set; } = new Regex(@"^bool$|WSbool");

        public AST.AbstractSyntaxTree.AstNode AstConstructor(string[] para)
        {
            try
            {
                //VariableSetup.nodes.Add(new AST.AbstractSyntaxTree.VariableAssignNode(VariableSetup.VariableTypes.Boolean, para[1], BitConverter.GetBytes(Convert.ToBoolean(para[3]))));
                return new AST.AbstractSyntaxTree.VariableAssignNode(VariableSetup.VariableTypes.Boolean, para[1], BitConverter.GetBytes(Convert.ToBoolean(para[3])));
            }
            catch
            {
                // Print Error
                ErrorHandling.Alert("Boolean value could not be generated at: " + String.Join(" ", para), ConsoleColor.Red);
                return null;
            }
        }
    }
}