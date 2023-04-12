namespace Tungsten_Interpreter.Utilities.Parser.Methods
{
    public class Maths : AST.AbstractSyntaxTree.AstNode
    {
        /*public static double Evaluate(string expression)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add("expression", string.Empty.GetType(), expression);
            System.Data.DataRow row = table.NewRow();
            table.Rows.Add(row);
            return double.Parse((string)row["expression"]);
        }*/

        private string input;
        private int pos;

        public Maths(string input)
        {
            this.input = input;
            pos = 0;
        }

        public int Parse()
        {
            int result = Expr();
            if (pos < input.Length)
            {
                throw new Exception("Unexpected input");
            }
            return result;
        }

        public override object? Execute()
        {
            return Parse();
        }

        private int Expr()
        {
            int result = Term();
            while (pos < input.Length)
            {
                char op = input[pos];
                if (op != '+' && op != '-')
                {
                    break;
                }
                pos++;
                int rhs = Term();
                if (op == '+')
                {
                    result += rhs;
                }
                else
                {
                    result -= rhs;
                }
            }
            return result;
        }

        private int Term()
        {
            int result = Factor();
            while (pos < input.Length)
            {
                char op = input[pos];
                if (op != '*' && op != '/')
                {
                    break;
                }
                pos++;
                int rhs = Factor();
                if (op == '*')
                {
                    result *= rhs;
                }
                else
                {
                    result /= rhs;
                }
            }
            return result;
        }

        private int Factor()
        {
            int result;
            if (input[pos] == '(')
            {
                pos++;
                result = Expr();
                if (input[pos] != ')')
                {
                    throw new Exception("Expected ')'");
                }
                pos++;
            }
            else
            {
                result = Num();
            }
            return result;
        }

        private int Num()
        {
            int result = 0;
            while (pos < input.Length && Char.IsDigit(input[pos]))
            {
                result = result * 10 + (input[pos] - '0');
                pos++;
            }
            return result;
        }
    }
}