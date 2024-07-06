using System.Collections;
using System.Collections.Generic;

public enum  BNodeState
{
    RUNNING,
    SUCCESS,
    FAILURE
}
public interface IBehavior
{
    public BNodeState Process();
    public void Reset();
}

public class RepeatedNode : BNode
{
    protected readonly IBehavior Behavior;
    public RepeatedNode(IBehavior behavior, string name = "RepeatedNode") : base(name)
    {
        Behavior = behavior;
    }
    public override BNodeState Process()
    {
        BNodeState = Behavior.Process();
        if (BNodeState == BNodeState.SUCCESS)
        {
            Behavior.Reset();
            BNodeState = BNodeState.RUNNING;
        }
        return BNodeState;
    }
}
public class BNode 
{
    public BNodeState BNodeState { get; protected set; }
    public List<BNode> Children { get; protected set; }
    protected int _currentChild = 0;
    protected string _name;
    
    public BNode(string name = "Node")
    {
        _name = name;
        BNodeState = BNodeState.RUNNING;
        Children = new List<BNode>();
    }
    public virtual BNodeState Process()
    {
        return Children[_currentChild].Process();
    }
    public void AddChild(BNode child)
    {
        Children.Add(child);
    }
    public void RemoveChild(BNode child)
    {
        Children.Remove(child);
    }
    public virtual void Reset()
    {
        _currentChild = 0;
        foreach (var child in Children)
        {
            child.Reset();
        }
    }
}
