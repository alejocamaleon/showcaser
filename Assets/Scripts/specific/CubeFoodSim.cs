using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFoodSim : MasterAction, IActionLauncher
{

    [Header("Cubes Play")]
    [SerializeField]
    List<Transform> cubes = new List<Transform>();
    [SerializeField]
    float cubeSpeed;
    [SerializeField]
    Transform food;
    [SerializeField]
    int maxPoints;
    [SerializeField]
    float foodArea;

    List<MasterAction> turn = new List<MasterAction>();
    List<Vector3> startPositions = new List<Vector3>();
    List<int> points = new List<int>();

    public override void Init(IActionLauncher actionManager, int index)
    {
        base.Init(actionManager, index);
        for (int idx = 0; idx < cubes.Count; idx++)
        {
            startPositions.Add(cubes[idx].position);
            points.Add(0);
        }
        createNewTurn();
    }

    public override void Tick()
    {
        for (int idx = 0; idx < turn.Count; idx++)
        {
            turn[idx].Tick();
        }
    }

    void createNewTurn()
    {
        //loop goes here
        turn.Clear();

        //spawn food
        food.position = new Vector3(Random.Range(-foodArea, foodArea), food.localScale.y/2, Random.Range(-foodArea, foodArea));
        AScale asc = new AScale();
        asc.actor = food;
        asc.startScale = Vector3.zero;
        asc.useCurrentScale = false;
        asc.endScale = Vector3.one;
        asc.relativeScale = false;
        asc.type = AScale.MoveType.time;
        asc.time = 0.5f;
        asc.easing = AnimationCurve.EaseInOut(0, 0, 1, 1);
        turn.Add(asc);


        //actions to move all cubes to the food
        foreach (Transform tt in cubes)
        {
            AMove am = new AMove();
            am.actor = tt;
            am.useCurrentPos = false;
            am.startPos = startPositions[cubes.FindIndex(x => x == tt )];
            am.endPos = new Vector3(food.position.x, 0, food.position.z);
            am.type = AMove.MoveType.speed;
            am.speed = cubeSpeed;
            am.easing = AnimationCurve.Linear(0, 0.5f, 1, 1);
            turn.Add(am);
        }

        for(int idx=0; idx < turn.Count; idx++)
        {
            turn[idx].Init(this, idx);
        }
    }



    void gameFinished(int winnerIndex)
    {
        //tell someone about the winner
        Debug.Log("Winner is " + winnerIndex);
        //deactivate or shrink the losers?
        Finish();
    }

    public void ActionFinished(int myIndex)
    {
        //nothing
        if (myIndex > 0)
        {
            //a cube has reached the food
            food.localScale = Vector3.zero;
            //first cube to reach the food gets a point and grows in height
            //add points
            points[myIndex - 1]++;
            cubes[myIndex - 1].localScale += Vector3.up;
            if(points[myIndex - 1] >= maxPoints)
            {
                gameFinished(myIndex - 1);
                return;
            }

            /*//resetplayfield
            foreach (Transform tt in cubes)
            {
                AMove am = new AMove();
                am.actor = tt;
                am.useCurrentPos = true;
                am.endPos = food.position;
                am.type = AMove.MoveType.speed;
                am.speed = cubeSpeed;
                am.easing = AnimationCurve.Linear(0, 0.5f, 1, 1);
                turn.Add(am);
            }*/

            //the a new loop
            createNewTurn();
        }
    }    

    public override void ForceFinish()
    {
        gameFinished(Random.Range(0, cubes.Count-1));        
    }

}
