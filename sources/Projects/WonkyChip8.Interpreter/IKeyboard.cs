namespace WonkyChip8.Interpreter
{
    public interface IKeyboard
    {
        bool IsKeyPressed(byte keyIndex);
        byte KeysCount { get; }
    }
}