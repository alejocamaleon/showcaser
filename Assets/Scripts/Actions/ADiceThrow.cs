using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADiceThrow : MasterAction
{

    [Header("Dice")]
    public GameObject dice;
    public Vector3 startPosition;
    public Vector3 throwDirection;
    public float throwForce;
    public float throwRandomness; //Applied to direction and force
    public GraphStatsDice graph;


    Rigidbody rb;
    bool firstTick = true;

    public override void Init(IActionLauncher actionManager, int index)
    {
        base.Init(actionManager, index);
        rb = dice.GetComponent<Rigidbody>();
        firstTick = true;
    }

    public override void Tick()
    {
        if (firstTick)
        {
            dice.transform.position = startPosition;
            rb.AddForceAtPosition(throwDirection.normalized * (throwForce + Random.Range(-throwRandomness, throwRandomness)), startPosition + (dice.transform.lossyScale / 2 * Random.Range(0.5f, 1f)));
            dice.transform.rotation = Random.rotation;
            firstTick = false;
        }
        
        if (rb.IsSleeping())
        {
            //send result to stats graph
            int result = 0;

            if (Vector3.up == dice.transform.up) { result = 1; } //ok
            if (Vector3.up == -dice.transform.up) { result = 2; } //ok
            if (Vector3.up == dice.transform.right) { result = 4; } //ok
            if (Vector3.up == -dice.transform.right) { result = 5; } //ok
            if (Vector3.up == dice.transform.forward) { result = 6; } //ok
            if (Vector3.up == -dice.transform.forward) { result = 3; } //ok

            if (result == 0)
            {
                rb.AddForceAtPosition(throwDirection.normalized * (throwForce + Random.Range(-throwRandomness, throwRandomness)), startPosition + (dice.transform.lossyScale / 2 * Random.Range(0.5f, 1f)));
                return;
            }

            //Debug.Log(result);
            graph.InsertValue(result);
            Finish();
        }
    }


    public override void ForceFinish()
    {
        //chose random number 1-6 and send to referenced graph
        graph.InsertValue(Random.Range(1, 7));
        Finish();
    }

}
