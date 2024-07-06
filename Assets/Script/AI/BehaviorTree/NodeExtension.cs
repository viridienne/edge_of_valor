using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NodeExtension
{
    public sealed class BPrioritySelector : BSelector
    {
        List<BNode> _sortedChildren;
        public List<BNode> SortedChildren => _sortedChildren ?? SortChildren();
        public BPrioritySelector(string name = "PrioritySelectorNode") : base(name)
        {
        }

        public override BNodeState Process()
        {
            foreach (var child in SortedChildren)
            {
                switch (child.Process())
                {
                    case BNodeState.RUNNING:
                        Debug.Log($"BPrioritySelector Processing {child.Name} with priority {child.Priority} state {child.BNodeState}");
                        return BNodeState.RUNNING;
                    case BNodeState.SUCCESS:
                        Debug.Log($"BPrioritySelector Processing {child.Name} with priority {child.Priority} state {child.BNodeState}");
                        Reset();
                        return BNodeState.SUCCESS;
                    default:
                        Debug.Log($"BPrioritySelector Processing {child.Name} with priority {child.Priority} state {child.BNodeState}");
                        continue;
                }
            }

            Reset();
            return BNodeState.FAILURE;
        }
        public List<BNode> SortChildren()
        {
           return Children.OrderByDescending(x => x.Priority).ToList();
        }

        public override void Reset()
        {
            base.Reset();
            _sortedChildren = null;
        }
    }
    public class BSelector : BNode
    {
        public BSelector(string name = "SelectorNode") : base(name)
        {

        }

        public override BNodeState Process()
        {
            if (_currentChild < Children.Count)
            {
                switch (Children[_currentChild].Process())
                {
                    case BNodeState.RUNNING:
                        return BNodeState.RUNNING;
                    case BNodeState.SUCCESS:
                        Reset();
                        return BNodeState.SUCCESS;
                    default:
                        _currentChild++;
                        return BNodeState.RUNNING;
                }
            }

            Reset();
            return BNodeState.FAILURE;
        }
    }
    public class BSequence : BNode
    {
        public BSequence(string name = "SequenceNode", int priority = 0) : base(name , priority)
        {
        }

        public override BNodeState Process()
        {
            if (_currentChild < Children.Count)
            {
                switch (Children[_currentChild].Process())
                {
                    case BNodeState.RUNNING:
                        return BNodeState.RUNNING;
                    case BNodeState.FAILURE:
                        _currentChild = 0;
                        return BNodeState.FAILURE;
                    default:
                        _currentChild++;
                        return _currentChild == Children.Count ? BNodeState.SUCCESS : BNodeState.RUNNING;
                }
            }

            Reset();
            return BNodeState.SUCCESS;
        }
    }
}