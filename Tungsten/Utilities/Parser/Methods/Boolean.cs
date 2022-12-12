namespace Tungsten_Interpreter.Utilities.Parser.Methods
{
    internal static class Check
    {
        public static bool Operation<T>(T val1, string op, T val2)
        {
            // Convert the Generic into Strings
            string v1 = val1.ToString();
            string v2 = val2.ToString();

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
                        Console.WriteLine("Cannot Compute String To Integer At {0} {1} {2}", v1, op, v2);
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
                        Console.WriteLine("Cannot Compute String To Integer At {0} {1} {2}", v1, op, v2);
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
                        Console.WriteLine("Cannot Compute String To Integer At {0} {1} {2}", v1, op, v2);
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
