namespace Tungsten_Interpreter.Utilities.Parser
{
    internal interface IMethod
    {
        public string Name { get; set; }
        //public bool lineInteract { get; set; }

        void Execute(string[] param);
        //int lineInteractExecute(string[] param);
    }
}
