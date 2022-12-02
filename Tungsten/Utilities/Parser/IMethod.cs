namespace Tungsten_Interpreter.Utilities.Parser
{
    internal interface IMethod
    {
        public string Name { get; set; }

        void Execute(string[] param);
    }
}
