using System.Text;

namespace Tungsten_Interpreter.Utilities.Parser.Methods
{
    internal static class Check
    {
        public static bool Operation(string val1, string op, string val2)
        {
            string v1 = val1;
            string v2 = val2;

            // Switch Statement For Each Accepted Operation
            switch (op)
            {
                case "==":
                    if (v1 == v2)
                    {
                        return true;
                    }
                    break;

                case "!=":
                    if (v1 != v2)
                    {
                        return true;
                    }
                    break;

                case "<=":
                    try
                    {
                        if (int.Parse(v1) <= int.Parse(v2))
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
                        Console.WriteLine("Cannot Compute Boolean At {0} {1} {2}", v1, op, v2);
                        return false;
                    }

                case ">=":
                    try
                    {
                        if (int.Parse(v1) >= int.Parse(v2))
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
                        Console.WriteLine("Cannot Compute Boolean At {0} {1} {2}", v1, op, v2);
                        return false;
                    }

                case "<":
                    try
                    {
                        if (int.Parse(v1) < int.Parse(v2))
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
                        Console.WriteLine("Cannot Compute Boolean At {0} {1} {2}", v1, op, v2);
                        return false;
                    }

                case ">":
                    try
                    {
                        if (int.Parse(v1) > int.Parse(v2))
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
                        Console.WriteLine("Cannot Compute String To Integer At {0} {1} {2}", v1, op, v2);
                        return false;
                    }

                default:
                    Console.WriteLine("Unknown Operator ({2}) Between {0} and {1}", val1, val2, op);
                    return false;
            }

            return false;
        }
    }
}
