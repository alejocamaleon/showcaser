using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASetActive : MasterAction
{
    [Header("Set Active GameObject")]
    public GameObject actorGO;
    public bool setActive = true;

    public override void Init(IActionLauncher actionManager, int index)
    {
        base.Init(actionManager, index);
        actorGO.SetActive(setActive);
        Finish();
    }
}
