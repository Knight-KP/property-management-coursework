namespace PropertyManagementConsole.DataStructures;

public class BstNode<T>
{
    public int Key;
    public T Value;

    public BstNode<T>? Left;
    public BstNode<T>? Right;

    public BstNode(int key, T value)
    {
        Key = key;
        Value = value;
    }
}
