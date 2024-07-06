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
    public bool IsLoop { get;}
    public BNodeState Process();
    public void Reset();
}

public class Condition : IBehavior
{
    protected readonly System.Func<bool> _condition;
    public Condition(System.Func<bool> condition)
    {
        _condition = condition;
    }
    public BNodeState Process()
    {
        return _condition() ? BNodeState.SUCCESS : BNodeState.FAILURE;
    }
    public bool IsLoop => false;
    public void Reset()
    {
    }
}

public class BNode 
{
    public BNodeState BNodeState { get; protected set; }
    public List<BNode> Children { get; }
    public string Name;
    public int Priority;
    protected int _currentChild = 0;
    public BNode(string name = "Node" , int priority = 0)
    {
        Name = name;
        Priority = priority;
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
