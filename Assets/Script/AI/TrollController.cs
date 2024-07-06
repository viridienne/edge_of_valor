using System.Collections;
using System.Collections.Generic;
using NodeExtension;
using UnityEngine;

public class TrollController : EnemyController
{
    public override void BuildBehaviorTree()
    {
        base.BuildBehaviorTree();
        
        var patrolAndIdle = new BLeaf(new PatrolBehavior(_transform, _spriteRenderer, _patrolPoints, _stat.Speed, 2f,true),
            "Patrol");
        
        var chase = new BSequence("Chase",2);

        chase.AddChild(new BLeaf(
            new ChaseBehavior(_transform, GameManager.Instance.Player.Tf, false, _stat.Speed, _stat.AttackRange,
                _sensor), "Chasing"));
        
        var selector = new BPrioritySelector("Troll Logic");
        selector.AddChild(chase);
        selector.AddChild(patrolAndIdle);
        
        
        _behaviorTree.AddChild(selector);
    }
}
