namespace VComm.Core.Objects
{
    internal class VRequest
    {
        public string[] phrases { get; set; }
        public Macro macro { get; set; } = new Macro();
    }
}
