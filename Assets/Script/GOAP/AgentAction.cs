using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// wrap up a strategy, with preconditions, effects afterwards
/// </summary>
public class AgentAction
{
    public EAction ActionType;
    public int Cost;

    /// <summary>
    /// precondition for the action to be executed
    /// </summary>
    public HashSet<AgentBelief> Preconditions;

    /// <summary>
    /// effects that take place after the action is executed
    /// </summary>
    public HashSet<AgentBelief> Effects;
    
    public IAction Action;
    
    public bool IsDone => Action.IsDone();
    public void Start() => Action.Start();

    public AgentAction(EAction type)
    {
        ActionType = type;
    }

    public void Update(float deltaTime)
    {
        if (Action.CanExecute())
        {
            Action.Update(deltaTime);
        }

        if (!Action.IsDone()) return;
        foreach (var effect in Effects)
        {
            effect.Evaluate();
        }
    }
    public void Stop() => Action.Stop();
    public bool CanExecute() => Action.CanExecute();

    public class Builder
    {
        private readonly AgentAction _action;

        public Builder(EAction type)
        {
            _action = new AgentAction(type);
        }
        public Builder WithCost(int cost)
        {
            _action.Cost = cost;
            return this;
        }
        public Builder WithPreconditions(HashSet<AgentBelief> preconditions)
        {
            _action.Preconditions = preconditions;
            return this;
        }
        public Builder WithPrecondition(AgentBelief precondition)
        {
            _action.Preconditions ??= new HashSet<AgentBelief>();
            _action.Preconditions.Add(precondition);
            return this;
        }
        public Builder WithEffects(HashSet<AgentBelief> effects)
        {
            _action.Effects = effects;
            return this;
        }
        public Builder WithEffect(AgentBelief effect)
        {
            _action.Preconditions ??= new HashSet<AgentBelief>();
            _action.Effects.Add(effect);
            return this;
        }
        public Builder WithAction(IAction action)
        {
            _action.Action = action;
            return this;
        }
        public AgentAction Build()
        {
            return _action;
        }
    }
}

public interface IAction
{
    public void Start();
    public void Update(float deltaTime);
    
    public void Stop();
    
    public bool IsDone();
    public bool CanExecute();
}