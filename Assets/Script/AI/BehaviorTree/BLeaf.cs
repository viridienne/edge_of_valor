/// <summary>
/// A leaf is a node that has no children
/// </summary>
public class BLeaf : BNode
{
    protected readonly IBehavior Behavior;
    public BLeaf(IBehavior behavior, string name = "Leaf") : base(name)
    {
        Behavior = behavior;
    }
    public override BNodeState Process()
    {
        BNodeState = Behavior.Process();
        return BNodeState;
    }
    public override void Reset()
    {
        Behavior.Reset();
    }
    
}