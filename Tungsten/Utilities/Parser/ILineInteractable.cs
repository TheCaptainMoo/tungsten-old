namespace Tungsten_Interpreter.Utilities.Parser
{
    internal interface ILineInteractable
    {
        public string Name { get; set; }
        public int lineExecute(string[] para, int sL);
    }
}
