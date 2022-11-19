namespace Tungsten_Interpreter.Utilities.Parser.Methods
{
    public class FunctionParam
    {
        public FunctionParam(List<string> parameters)
        {
            Parameters = parameters;
        }
        public List<string> Parameters { get; set; }
    }

    public class FunctionBody
    {
        public FunctionBody(Dictionary<int, string[]> body)
        {
            Body = body;
        }

        public Dictionary<int, string[]> Body { get; set; }
    }
}