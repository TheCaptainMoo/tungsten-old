using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tungsten_Interpreter.Utilities.Parser.Methods
{
    internal static class ByteManipulation
    {
        public static byte[] CharsToInt(byte[] str) 
        {
            return BitConverter.GetBytes(Convert.ToInt32(Encoding.UTF8.GetString(str)));
        }

        public static byte[] IntToChars(byte[] num)
        {
            return Encoding.UTF8.GetBytes(BitConverter.ToInt32(num).ToString());
        }

        /*byte[] value;
        try
        {
            value = (byte[])Parameters[i].Execute();
        }
        catch
        {
            Memory<byte> memory = (Memory<byte>)Parameters[i].Execute();
            value = memory.Span.ToArray();
        }*/
        public static byte[] GetValue(AST.AbstractSyntaxTree.AstNode para)
        {
            byte[] value;

            if(para is AST.AbstractSyntaxTree.ValueNode val)
            {
                value = (byte[])val.Execute();
            }
            else if (para is AST.AbstractSyntaxTree.VariableNode var)
            {
                value = ((Memory<byte>)var.Execute()).Span.ToArray();
            }
            else if (para is AST.AbstractSyntaxTree.VariableNodedAssignNode varNode)
            {
                value = ((Memory<byte>)varNode.Execute()).Span.ToArray();
            }
            else
            {
                ErrorHandling.Alert("Cannot get bytes from: " + para.ToString(), ConsoleColor.Red);
                value = new byte[0];
            }
            return value;
        }
    }
}
