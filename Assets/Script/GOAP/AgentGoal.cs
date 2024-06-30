using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// an outcome, what the world looks like after the agent has achieved the goal
/// </summary>
public class AgentGoal
{
    public EGoal GoalType;
    public int Priority;
    
    /// <summary>
    /// things that the agent wants to achieve
    /// </summary>
    public HashSet<AgentBelief> DesiredEffects;

    public AgentGoal(EGoal type)
    {
        GoalType = type;
    }

    public class Builder
    {
        readonly AgentGoal _goal;
        public Builder (EGoal type)
        {
            _goal = new AgentGoal(type);
        }
        public Builder WithPriority(int priority)
        {
            _goal.Priority = priority;
            return this;
        }
        public Builder WithDesiredEffects(HashSet<AgentBelief> effects)
        {
            _goal.DesiredEffects = effects;
            return this;
        }
        public Builder WithDesiredEffect(AgentBelief effect)
        {
            _goal.DesiredEffects ??= new HashSet<AgentBelief>();
            _goal.DesiredEffects.Add(effect);
            return this;
        }
        public AgentGoal Build()
        {
            return _goal;
        }
    }
}
