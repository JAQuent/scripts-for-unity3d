// Script to track position and view rotation of game objects in Unity3D
// Author: Joern Alexander Quent
// E-mail: Alex.Quent at mrc-cbu.cam.ac.uk
// Licence: GNU General Public License v2.0
// Version history:
// 					1.0 - 2018/09/12 - first Version.
// 					1.1 - 2018/09/13 - Actually saving milliseconds and not float values in seconds.
//
// How to use:
// 1. Attach script a game object.
// 2. Indicate how many objects should be tracked by setting size of 
// 	  public variable Tracked Obj in Unity panel. 
// 3. Drag the game objects in the element boxes. 
// 4. Specifying the file name stem and number. For instance, you
//    can use data/trackingFile as the stem to save the file in the 
//    folder data and 1 as the number for the current run. 
//
// Control: Press S for starting, P for pausing and E for ending tracking.    
//
// Output: The example above will save two files in the folder data
//    (trackingFile_info_1.txt and trackingFile_1.txt). The first file 
//    contains general information about the tracking (File name of tracking 
//    file,system time at start, number of tracked objects and their ID 
//    and names). The second files contains the actual tacking data. 
//    Each row is a data point. The tab seperated columns are object name, object ID,
//    time in milliseconds (starting at zero), position (x, y, z) and rotation (x, y, z).
//
// Important: The tracking of the game object is relative to transform position, 
//    which is relative to any parent objects. Keep that in mind when using the script
//    to track position. Objects' positions are parsed in a loop. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.IO;

public class trackObjects : MonoBehaviour {

	// Public variables
	public List<GameObject> trackedObj;
	public string fileNamStem;
	public string fileNamNum;

	// Private variables
	private StreamWriter trackFile;
	private StreamWriter infoFile;
	private string startTimeNormal;
	private int numTrackedObj;
	private bool track;

	// Initialization
	void Start () {
		// Parsing information
		startTimeNormal       = System.DateTime.Now.ToString("yyyy, MM, dd, hh, mm, ss");
		numTrackedObj         = trackedObj.Count;

		// Writing info file
		infoFile              = new StreamWriter(fileNamStem + "_info_" + fileNamNum + ".txt");
		infoFile.WriteLine("File name:\t" + fileNamStem + "_" + fileNamNum + ".txt" +
						   "\nStart system time:\t" + startTimeNormal +
						   "\nNumber of tracked objects:\t" + numTrackedObj +
						   "\nList of tracked objects:");
		// Listing all tracked objects at the end of the file
		for(int i = 0; i < numTrackedObj; i++){
			infoFile.WriteLine("" + i + "\t" + trackedObj[i]);
		}

		// Close and save file
		infoFile.Close();

		// Opening tracking file
		trackFile = new StreamWriter(fileNamStem + "_" + fileNamNum + ".txt");
	}
	
	// Update is called once per frame
	void Update () {
		// Writing position and rotation of tracked object to text file as often as possible
		if(track){
			for (int i = 0; i < numTrackedObj; i++){
				trackFile.WriteLine("" + 
						  trackedObj[i] + "\t" +
						  i + "\t" + 
						  Time.time * 1000 + "\t" +
						  trackedObj[i].transform.position.x + "\t" + 
						  trackedObj[i].transform.position.y + "\t" +
						  trackedObj[i].transform.position.z + "\t" +
						  trackedObj[i].transform.rotation.x + "\t" +
						  trackedObj[i].transform.rotation.y + "\t" +
						  trackedObj[i].transform.rotation.z);
			}
		}

		// Start tracking
		if(Input.GetKeyDown(KeyCode.S)){
			track = true;
			Debug.Log("Started tracking");
		}

		// Pause tracking
		if(Input.GetKeyDown(KeyCode.P)){
			track = false;
			Debug.Log("Stopped tracking");
		}

		// CLose and end tracking
		if(Input.GetKeyDown(KeyCode.E)){
			// Close and save file
			trackFile.Close();
			track = false;
			Debug.Log("Ended tracking");
		}
	}
}