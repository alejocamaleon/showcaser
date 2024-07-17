using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWait : MasterAction
{
    [Header("Wait")]
    public float seconds = 1000;

    float counter = 0;

    public override void Init(IActionLauncher actionManager, int index)
    {
        base.Init(actionManager, index);
        counter = 0;
    }

    public override void Tick()
    {
        counter += Time.deltaTime;
        if (counter > seconds)
        {
            Finish();
        }
        
    }
}
