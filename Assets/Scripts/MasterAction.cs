using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAction : MonoBehaviour
{
    [Header("Action")]
    public bool useCustomMaxTicks = false;
    public int customMaxTicks = 1000;

    public IActionLauncher am { get; set; }
    public int myIndex { get; set; }

    public virtual void Init(IActionLauncher actionManager, int index)
    {
        am = actionManager;
        myIndex = index;
    }

    public virtual void Tick()
    {
        
    }

    public virtual void ForceFinish()
    {
        Finish();
    }

    public virtual void Finish()
    {
        am.ActionFinished(myIndex);
    }
}
