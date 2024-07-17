using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARotate : MasterAction
{

    public enum MoveType { speed, time };

    [Header("Rotate")]
    public Transform actor;
    public Vector3 startRot;
    public bool useCurrentRot = false;
    public Vector3 endRot;
    public bool relativeRot = false;

    public MoveType type;
    public float speed = 1f;
    public float time = 1f;

    public AnimationCurve easing = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

    Vector3 targetRot;
    float distance;
    float elapsedTime = 0;

    public override void Init(IActionLauncher actionManager, int index)
    {
        base.Init(actionManager, index);

        if (!useCurrentRot)
        {
            actor.localEulerAngles = startRot;
        }
        startRot = actor.localEulerAngles;

        targetRot = endRot;
        if (relativeRot)
        {
            targetRot = actor.localEulerAngles + endRot;
        }

        distance = Vector3.Distance(startRot, targetRot);
        elapsedTime = 0;
    }

    public override void Tick()
    {
        elapsedTime += Time.deltaTime;
        switch (type)
        {
            case MoveType.speed:
                actor.localEulerAngles += (targetRot - actor.localEulerAngles).normalized * speed * easing.Evaluate(Vector3.Distance(actor.localEulerAngles, startRot) / distance) * Time.deltaTime;

                if (Vector3.Distance(actor.localEulerAngles, targetRot) < 0.02f || Vector3.Distance(actor.localEulerAngles, startRot) > distance)
                {
                    actor.localEulerAngles = targetRot;
                    Finish();
                }
                break;

            case MoveType.time:
                actor.localEulerAngles = startRot + (targetRot - startRot).normalized * easing.Evaluate(elapsedTime / time) * distance;

                if (elapsedTime >= time)
                {
                    actor.localEulerAngles = targetRot;
                    Finish();
                }
                break;
        }
    }

    public override void ForceFinish()
    {
        actor.localEulerAngles = targetRot;
        Finish();
    }
}
