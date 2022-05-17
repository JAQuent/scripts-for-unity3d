# scripts-for-unity3d
This repository contains various C# scripts written for VR experiments in unity3d. Some of scripts (e.g. trackObjects.cs) have much better implementations here: https://github.com/immersivecognition/unity-experiment-framework

# List:
* **_trackObjects.cs_** is designed to track a number of game objects (position and rotation) in unity3d saving the data in .txt file. The script - for instance - can be used to track the player or the VR camera and the controllers. Instruction how to use it are in the script.
* **_csharpFunctions.cs_** is a collection of short functions that can be used for unity3d. Especially it includes functions that reads in a .txt file and then parses that information to a list of float/int. This is useful to give unity input tables. This is not a stand-alone script. 
* **_moveCamera.cs_** is a script that moves and rotates a camera or effectively any other game object through a series of way points and stops a specified amount of time after arriving at each point. 
* **_LookAtPlayer.cs_** is a script that makes a game object look at the player the whole time. 
* **_screenRayTracker_** An experimental script that allows you to track what's on the screen in Unity. Check this folder for more details. 

# Disclaimer: 
Except when otherwise stated in writing the copyright holders and/or other parties provide the programs 'as is' without warranty of any kind, expressed or implied, including, but not limited to, the implied warranties of merchantability and fitness for a particular purpose.
