namespace WonkyChip8.Interpreter.Commands
{
    public class NullCommand : Command
    {
        public NullCommand(int? address) : base(address, null) { }
        public override int? NextCommandAddress { get { return null; } }
    }
}