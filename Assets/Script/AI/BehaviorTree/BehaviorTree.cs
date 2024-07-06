using UnityEngine;

public class BehaviorTree : BNode
{
    public BehaviorTree(string name = "BehaviorTree") : base(name)
    {
    }
    public override BNodeState Process()
    {
        BNodeState = Children[_currentChild].Process();
        _currentChild = (_currentChild + 1) % Children.Count;
        return BNodeState.RUNNING;
    }
}