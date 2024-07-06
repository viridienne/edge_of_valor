public class BehaviorTree : BNode
{
    public BehaviorTree(string name = "BehaviorTree") : base(name)
    {
    }
    public override BNodeState Process()
    {
        while (_currentChild < Children.Count)
        {
            BNodeState = Children[_currentChild].Process();
            if (BNodeState == BNodeState.SUCCESS)
            {
                _currentChild++;
            }
            else
            {
                return BNodeState;
            }
               
        }
        return BNodeState.SUCCESS;
    }
    public override void Reset()
    {
        _currentChild = 0;
        foreach (var child in Children)
        {
            child.Reset();
        }
    }
}