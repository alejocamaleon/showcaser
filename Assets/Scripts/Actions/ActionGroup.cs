using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionGroup : MasterAction, IActionLauncher
{

    public List<MasterAction> actions;
    List<MasterAction> groupedActions;
    List<MasterAction> finishedActions;        

    // Update all actions in the group until all of them are finished
    public override void Init(IActionLauncher actionManager, int index)
    {
        base.Init(actionManager, index);
        groupedActions = new List<MasterAction>();
        finishedActions = new List<MasterAction>();
        for (int idx = 0; idx < actions.Count; idx++)
        {
            actions[idx].Init(this, idx);
            groupedActions.Add(actions[idx]);            
        }
    }

    public override void Tick()
    {
        foreach(MasterAction ta in groupedActions)
        {
            ta.Tick();
        }
        foreach (MasterAction fa in finishedActions)
        {
            groupedActions.Remove(fa);
        }
        finishedActions.Clear();
        if (groupedActions.Count == 0)
        {
            Finish();
        }
    }

    public override void ForceFinish()
    {
        foreach (MasterAction ta in groupedActions)
        {
            ta.ForceFinish();
        }
        Finish();
    }

    public void ActionFinished(int index)
    {
        finishedActions.Add(actions[index]);        
    }
}
