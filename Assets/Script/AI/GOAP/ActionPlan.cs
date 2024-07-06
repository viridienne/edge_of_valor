using System.Collections.Generic;

public interface IPlanner
{
    ActionPlan Plan(AgentGoal goal, HashSet<AgentGoal> goals, AgentGoal mostRecentGoal);
}

public class GPlanner : IPlanner
{
    public ActionPlan Plan(AgentGoal goal, HashSet<AgentGoal> goals, AgentGoal mostRecentGoal)
    {
        // //order goals by priority and not achieved
        // var orderedGoals = goals.Where(g => g.DesiredEffects.Any(e => !e.Condition)).OrderByDescending(g => g == mostRecentGoal? g.Priority - 1 : g.Priority).ToList();
        // //solve goal in order
        // foreach (var g in orderedGoals)
        // {
        //    var node = new GNode(null,null, g.DesiredEffects, 0);
        //    
        // }
        return new ActionPlan(goal, new Stack<AgentAction>(), 0);
    }
    
    // //return true if a path is found
    // public bool FindPath(GNode parentNode, HashSet<AgentAction> actions)
    // {
    //     foreach (var a in actions)
    //     {
    //         var required = parentNode.Required;
    //         required.RemoveWhere(r => r.Condition);
    //         if(required.Count == 0)
    //         {
    //             return true;
    //         }
    //         if(a.Effects.Any(required.Contains))
    //         {
    //             var newRequired = new HashSet<AgentBelief>(required);
    //             newRequired.ExceptWith(a.Effects);
    //             newRequired.UnionWith(a.Preconditions); 
    //         }
    //     }
    //
    // }
}

public class GNode
{
    public GNode Parent;
    public AgentAction Action;
    public HashSet<AgentBelief> Required;
    public List<GNode> Leaves;
    public int Cost;

    public bool IsLeafDead => Leaves.Count == 0 && Action == null;
    public GNode(GNode parent, AgentAction action, HashSet<AgentBelief> required, int cost)
    {
        Parent = parent;
        Action = action;
        Required = required;
        Cost = cost;
        Leaves = new List<GNode>();
    }
}
public class ActionPlan 
{
    //current goal
    public AgentGoal CurrentGoal { get; private set; }
    //actions stack to achieve the goal
    public Stack<AgentAction> Actions { get; private set; }
    // sum cost
    public int TotalCost { get; private set; }
    //costructor
    public ActionPlan(AgentGoal goal, Stack<AgentAction> actions, int totalCost)
    {
        CurrentGoal = goal;
        Actions = actions;
        TotalCost = totalCost;
    }

}
