# showcaser
Using Unity as a video generation tool by linking actions and exporting a video
(This project is not in development anymore)

Showcaser
by AlejoLab
Made in Unity 2020.3.4f1

---------------------------
This is a little experiment based on the idea of using Unity as a video generation tool.
Inspired by https://github.com/Helpsypoo/PrimerUnity

TUTORIAL
--------
How to quickly see this project in action:

1. Open any scene in the Examples folder
2. Find the object ActionManager
3. Check the following settings
   Range start, range end, preloops = 0
   loops = 0 or 1 (same result)
   Actions: There are some elements
   Capture - Uncheck!
   (The rest is ignored when not capturing)
   MaxTicksPerAction: A high number like 10K or more
4. Run the scene
5. Things will happen, whenever the actions are completed the editor will stop running by itself and output a message on the console tab.
6. Now, to understand something interesting about Unity capturing process change the following:
   Capture: checked
   Capture index: Anything higher than the number of actions ie: 1000 (to avoid doing the actual capturing process)
   FrameRate: Try anything between 2 and 60 (try both)
7. Run the scene
8. When captureFrameRate is set but there is no capture happening Unity will process everything as fast as possible.
9. Play around with the parameters and run the scene, a summary of what they do:

Range Start: First action element to show (all the previous steps are forcedFinished)
Range End: Last action element to show. 0 or any high number will run the actions till the end.
Preloops: precompute this many loops of the whole list (considering the range defined) before actually showing the scene
Loops: Run the list this many times (0 a 1 have the same effect, running the action list one once)

Capture: Will capture images if checked
Capture index: Action number in which the capture will start
Output Folder: Rendered images will go here
Filename: name of the images, will have appended _XXXX.png to create a sequence
Size: Supersize, 1 is the original resolution set in the game view. Numbers above will output higher resolutions (can export in 8K!)
FrameRate: Framerate of the image sequence (2-10 is useful for timelapses, 60-120 for slowmotion)
Max Ticks Per Action: Safety measure, Will stop the scene if any action takes more that many ticks (frames) to simulate.


SYSTEM OVERVIEW
---------------

ActionManager
Manages the action list and the capturing process

The actions need to be part of a gameobject so we can set object references right in the editor.
Drag and drop the object containing the actions into the Action array in ActionManager.
(In the example scenes the actions are all stored in the Actions GameObject)


ActionMaster (base class)
Simulate/execute a custom action

Init() - Setup and reset (for when reusing actions) all variables and starting conditions
Tick() - Do the stuff, called on Update by ActionManager (call Finish when its done)
ForceFinish() - Defines a final state and finishes the action, useful for fastforwarding when testing
Finish() - Tells ActionManager the action has finished and stops being updated


Subclases of ActionMaster:

* AMove - Move a transform in a straight line (optional relative positions, animation based on speed or time, easing curve)

* AScale - Scale one trasnform (optional relative size, animation based on speed or time, easing curve)

* ASetActive - Activate or deactivate a GameObject

* Await - Wait the defined amount in seconds

* ADiceThrow - Custom made to throw a dice using a physical sim and send the result to a graph object.

Custom actions can be made using ActionMaster as a base class.


POTENTIAL FEATURES
------------------
Things I havent tried or checked yet, or that I think could be possible with this system:

I plan on doing ActionManager an action itself, so actions can be nested or grouped to be run in parallel.
Also opens the possibility to create grids of scenes/sims running in parallel (like the multiple blobs throwing dice/winning coins)

Still have to check how it handles unity animations.

A way to define actions within the code itself to define actions during runtime

Multiple cameras

Export directly to mp4 video using ffmpeg
