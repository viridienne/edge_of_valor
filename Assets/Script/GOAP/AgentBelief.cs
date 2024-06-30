using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeliefFactory
{
    private readonly GOAPAgent _agent;
    private readonly Dictionary<EBelief,AgentBelief> _beliefs;

    public BeliefFactory(GOAPAgent agent, Dictionary<EBelief, AgentBelief> dict)
    {
        _agent = agent;
        _beliefs = dict;
    }
    public void AddBelief(EBelief type, bool condition)
    {
        if (_beliefs.ContainsKey(type))
        {
            _beliefs[type] = new AgentBelief.Builder(type).WithCondition(condition).Build();
        }
        else
        {
            _beliefs.Add(type, new AgentBelief.Builder(type).WithCondition(condition).Build());
        }
    }

    public void AddLocationBelief(EBelief type, float distance, Vector3 location)
    {
        if (_beliefs.ContainsKey(type))
        {
            _beliefs[type] = new AgentBelief.Builder(type).WithCondition(IsInRange(location, distance))
                .WithLocation(location).Build();
        }
        else
        {
            _beliefs.Add(type,
                new AgentBelief.Builder(type).WithCondition(IsInRange(location, distance)).WithLocation(location)
                    .Build());
        }
    }
    
    private bool IsInRange(Vector3 target, float range)
    {
        return Vector3.Distance(_agent.transform.position, target) <= range;
    }
    
}
public class AgentBelief
{
    public EBelief Type;
    private bool _condition = false;
    public bool Condition => _condition;

    private Vector3 _location;
    public Vector3 Location => _location;

    public AgentBelief(EBelief type)
    {
        Type = type;
    }

    public class Builder
    {
        private readonly AgentBelief _belief;

        public Builder(EBelief type)
        {
            _belief = new AgentBelief(type);
        }

        public Builder WithCondition(bool condition)
        {
            _belief._condition = condition;
            return this;
        }

        public Builder WithLocation(Vector3 location)
        {
            _belief._location = location;
            return this;
        }

        public AgentBelief Build()
        {
            return _belief;
        }
    }
}

/// <summary>
/// preconditions, effects are all beliefs
/// </summary>
public enum EBelief
{
    AgentIdle,
    AgentMoving,
    AgentJumping,
    AgentAttacking,
    AgentDead,
}
public enum EAction
{
    Attack,
    Move,
    Idle,
    Dead,
    Jump,
}