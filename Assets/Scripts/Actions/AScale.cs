using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AScale : MasterAction
{    
    public enum MoveType { speed, time };

    [Header("Scale")]
    public Transform actor;
    public Vector3 startScale;
    public bool useCurrentScale = false;
    public Vector3 endScale;
    public bool relativeScale = false;

    public MoveType type;
    public float speed = 1f;
    public float time = 1f;

    public AnimationCurve easing = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

    Vector3 targetScale;
    float distance;
    float elapsedTime = 0;

    public override void Init(IActionLauncher actionManager, int index)
    {
        base.Init(actionManager, index);

        if (!useCurrentScale)
        {
            actor.localScale = startScale;
        }
        startScale = actor.localScale;

        targetScale = endScale;
        if (relativeScale)
        {
            targetScale = actor.localScale + endScale;
        }

        distance = Vector3.Distance(startScale, targetScale);
        elapsedTime = 0;
    }

    public override void Tick()
    {        
        elapsedTime += Time.deltaTime;
        switch (type)
        {
            case MoveType.speed:
                actor.localScale += (targetScale - actor.localScale).normalized * speed * easing.Evaluate(Vector3.Distance(actor.localScale, startScale) / distance) * Time.deltaTime;

                if (Vector3.Distance(actor.localScale, targetScale) < 0.01f || Vector3.Distance(actor.localScale, startScale) > distance)
                {
                    actor.localScale = targetScale;
                    Finish();
                }
                break;

            case MoveType.time:
                actor.localScale = startScale + (targetScale - startScale).normalized * easing.Evaluate(elapsedTime / time) * distance;

                if (elapsedTime >= time)
                {
                    actor.localScale = targetScale;
                    Finish();
                }
                break;
        }
    }

    public override void ForceFinish()
    {
        actor.localScale = targetScale;
        Finish();
    }
}
