using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMove : MasterAction
{
    public enum MoveType { speed, time };

    [Header("Move")]
    public Transform actor;
    public Vector3 startPos;
    public bool useCurrentPos = false;
    public Vector3 endPos;
    public bool relativePos = false;

    public MoveType type;
    public float speed = 1f;
    public float time = 1f;

    public AnimationCurve easing = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

    Vector3 targetPos;
    float distance;
    float elapsedTime = 0;

    public override void Init(IActionLauncher actionManager, int index)
    {
        base.Init(actionManager, index);

        if (!useCurrentPos)
        {
            actor.position = startPos;
        }
        startPos = actor.position;

        targetPos = endPos;
        if (relativePos)
        {
            targetPos = actor.position + endPos;
        }

        distance = Vector3.Distance(startPos, targetPos);
        elapsedTime = 0;
    }

    public override void Tick()
    {
        elapsedTime += Time.deltaTime;
        switch (type)
        {
            case MoveType.speed:
                actor.position += (targetPos - actor.position).normalized * speed * easing.Evaluate(Vector3.Distance(actor.position, startPos) / distance) * Time.deltaTime;

                if (Vector3.Distance(actor.position, targetPos) < 0.02f || Vector3.Distance(actor.position, startPos) > distance)
                {
                    actor.position = targetPos;
                    Finish();
                }
                break;

            case MoveType.time:
                actor.position = startPos + (targetPos - startPos).normalized * easing.Evaluate(elapsedTime/time) * distance;

                if (elapsedTime >= time)
                {
                    actor.position = targetPos;
                    Finish();
                }
                break;
        }        
    }

    public override void ForceFinish()
    {
        actor.position = targetPos;       
        Finish();
    }
}
