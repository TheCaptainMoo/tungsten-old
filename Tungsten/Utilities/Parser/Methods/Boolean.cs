using System.Text;

namespace Tungsten_Interpreter.Utilities.Parser.Methods
{
    internal static class Check
    {
        public static bool Operation(byte[] val1, string op, byte[] val2)
        {
            string? v1 = null;
            string? v2 = null;

            int? iv1 = null;
            int? iv2 = null;
            if (op == "==" || op == "!=")
            {
                v1 = Encoding.UTF8.GetString(val1);
                v2 = Encoding.UTF8.GetString(val2);

                switch (op)
                {
                    case "==":
                        if (v1 == v2)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case "!=":
                        if (v1 != v2)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                }
            }
            else
            {
                iv1 = BitConverter.ToInt32(val1);
                iv2 = BitConverter.ToInt32(val2);

                switch (op)
                {
                    case "<=":
                        try
                        {
                            if (iv1 <= iv2)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Cannot Compute Boolean At {0} {1} {2}", iv1, op, iv2);
                            return false;
                        }

                    case ">=":
                        try
                        {
                            if (iv1 >= iv2)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Cannot Compute Boolean At {0} {1} {2}", iv1, op, iv2);
                            return false;
                        }

                    case "<":
                        try
                        {
                            if (iv1 < iv2)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Cannot Compute Boolean At {0} {1} {2}", iv1, op, iv2);
                            return false;
                        }

                    case ">":
                        try
                        {
                            if (iv1 > iv2)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Cannot Compute String To Integer At {0} {1} {2}", iv1, op, iv2);
                            return false;
                        }

                    default:
                        Console.WriteLine("Unknown Operator ({2}) Between {0} and {1}", val1, val2, op);
                        return false;
                }
            }

            return false;
        }

        public static bool Comparsion(bool val1, string op, bool val2)
        {
            switch (op)
            {
                // AND
                case "&&":
                    if (val1 == true && val2 == true)
                        return true;
                    else
                        return false;

                // OR
                case "||":
                    if (val1 == true || val2 == true)
                        return true;
                    else
                        return false;

                // eXclusive OR
                case "?|":
                    if ((val1 == true || val2 == true) && !(val1 == true && val2 == true))
                        return true;
                    else
                        return false;
            }

            return false;
        }
    }
}
