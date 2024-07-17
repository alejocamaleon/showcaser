using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ActionManager : MonoBehaviour, IActionLauncher
{
    public int rangeStart = 0;
    [Tooltip("0 or less than Range Start will execute until the last action defined")]
    public int rangeEnd = 0;
    public int preloops = 0;
    public int loops = 0;
    int loopCount = 0;

    //public GameObject[] actions;
    public MasterAction[] actions;
    public bool capture = false;
    [Tooltip("Action number when the capture will start")]
    public int captureIndex = 0;
    public string outputFolder = "";
    public string filename = "";
    public int size = 1;
    public int frameRate = 60;
    public int maxTicksPerAction = 100000;

    //List<IAction> actionList = new List<IAction>();
    int actionIndex = 0;
    int tickCounter = 0;
    bool stopApp = false;

    // Start is called before the first frame update
    void Start()
    {
        loopCount = 0;

        if (capture)
        {
            Time.captureFramerate = frameRate;

            try
            {                
                if(!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

            }
            catch (IOException ex)
            {
                Debug.LogError(ex.Message);
            }

        }

        if (actions.Length == 0)
        {            
            StopApplication("Action list is empty");
            return;
        }

        if (rangeStart >= actions.Length-1 && rangeStart != 0)
        {
            StopApplication("Range Start is out of bounds");
            return;
        }

        if (rangeEnd < rangeStart || rangeEnd == 0)
        {
            rangeEnd = -1;
        }

        for (int pli = 0; pli <= preloops; pli++)
        {
            actions[0].Init(this, 0);

            if (rangeStart > 0)
            {
                actions[0].ForceFinish();
                for (int odx = 1; odx < rangeStart; odx++)
                {
                    actions[odx].Init(this, odx);
                    actions[odx].ForceFinish();
                }
            }

            if ( pli == preloops) { break; }

            for (int odx = rangeStart; odx < Mathf.Max(rangeEnd, actions.Length); odx++)
            {
                actions[odx].Init(this, odx);
                actions[odx].ForceFinish();
            }
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if (stopApp)
        {
            return;
        }
        tickCounter++;
        
        if (actions[actionIndex].useCustomMaxTicks)
        {
            if (tickCounter > actions[actionIndex].customMaxTicks)
            {
                StopApplication("MaxCustomTicksPerAction ("+ actions[actionIndex].customMaxTicks + ") reached on action: " + actionIndex );
                return;
            }
        } else
        {
            if (tickCounter > maxTicksPerAction)
            {
                StopApplication("MaxTicksPerAction reached on action: " + actionIndex);
                return;
            }
        }

        actions[actionIndex].Tick();

        if (capture && actionIndex >= captureIndex)
        {
            //Debug.Log("deltaTime: " + Time.deltaTime + ", captureDeltaTime: " + Time.captureDeltaTime);
            
            string filepath = string.Format("{0}/{1}_{2:D04}.png", outputFolder, filename,Time.frameCount);
            ScreenCapture.CaptureScreenshot(filepath, size);            
        }

    }

    public void ActionFinished(int index)
    {
        if (index == actionIndex)
        {            
            if (actionIndex == actions.Length-1)
            {
                if (keepLooping()) { return; }
                StopApplication("Scene completed");
                return;
            }


            if (actionIndex == rangeEnd)
            {
                if(keepLooping()) { return; }
                StopApplication("testing range completed");
                return;
            }

            actionIndex++;
            actions[actionIndex].Init(this, actionIndex);
            tickCounter = 0;
        }
    }

    bool keepLooping()
    {
        loopCount++;
        if (loopCount < loops + preloops)
        {
            if (rangeStart > 0)
            {
                for (int odx = 0; odx < rangeStart; odx++)
                {
                    actions[odx].Init(this, odx);
                    actions[odx].ForceFinish();
                }
            }

            actionIndex = rangeStart;
            actions[actionIndex].Init(this, actionIndex);
            tickCounter = 0;            
            return true;
        } else
        {            
            return false;
        }
    }

    void StopApplication(string msg)
    {
        Debug.Log("Application stopped: "+msg);
        stopApp = true;
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#else
        
        Application.Quit();
#endif

    }

}
