using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
// Inspiriation for this script: https://www.youtube.com/watch?v=5Ue0waWtkY4

public class moveCamera : MonoBehaviour{
	// Public variables
    public string fileName;
    public float moveSpeed;
    public float rotateSpeed;

    // Private variables
    private int step = 0;
    private int indexMin;
    private Vector3 targetPos;
    private Quaternion targetRot;
    private int n;
    private List<float> xPos;
    private List<float> yPos;
    private List<float> zPos; 
    private List<float> xRot; 
    private List<float> yRot; 
    private List<float> zRot; 
    private List<float> secs;
    private bool started = false;
    private bool arrived = false;
    private bool rotated = false;
    private bool timeElapsed = true;
    private float moveStep;

    // Use this for initialization
	void Start () {
		// Loading input file
		string[] wayPointsString = readText(fileName);

		// Get movement speed fom PlayerPref
		// This is set in the startMenu
		moveSpeed = PlayerPrefs.GetFloat("moveSpeed");
		rotateSpeed = PlayerPrefs.GetFloat("rotateSpeed");

		// Parsing information
		int indexCount = 7; // because there are 7 variables
		n = wayPointsString.Length/indexCount;

		//Positions
		indexMin = 0;
		xPos     = parseFloatListFromString(indexMin, indexCount, wayPointsString);

		indexMin = indexMin + 1;
		yPos     = parseFloatListFromString(indexMin, indexCount, wayPointsString);

		indexMin = indexMin + 1;
		zPos     = parseFloatListFromString(indexMin, indexCount, wayPointsString);

		// Rotations
		indexMin = indexMin + 1;
		xRot     = parseFloatListFromString(indexMin, indexCount, wayPointsString);

		indexMin = indexMin + 1;
		yRot     = parseFloatListFromString(indexMin, indexCount, wayPointsString);

		indexMin = indexMin + 1;
		zRot     = parseFloatListFromString(indexMin, indexCount, wayPointsString);

		indexMin = indexMin + 1;
		secs     = parseFloatListFromString(indexMin, indexCount, wayPointsString);
	}

    // Update is called once per frame
    void Update(){
    	// Start sequence if space bar press
    	if(Input.GetKeyUp(KeyCode.Space)){
    		started = true;
    	}

    	// Once started
    	if(started & step < n){
    		// Parse target position/rotation of current step
    		targetPos = new Vector3(xPos[step], yPos[step], zPos[step]);
        	targetRot = Quaternion.Euler(xRot[step], yRot[step], zRot[step]);

        	// Change position to position of current step
        	moveStep = moveSpeed * Time.deltaTime;
        	transform.position = Vector3.MoveTowards(transform.position, targetPos, moveStep);
        	//transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);

        	// Change rotation to rotation of current step
        	transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);

        	// Wait at this way point for x secs and then go to next step
        	arrived = isAtLocation(transform.position, targetPos, 4);
        	//rotated = isRotated(transform.rotation, targetRot);
        	if(arrived & timeElapsed){
        		StartCoroutine(waitRoutine());
        	}
        }
    }

    // Functions
    string[] readText(string fileName){
    	// Loads .txt file and split by \t and \n
    	string inputText    = System.IO.File.ReadAllText(fileName);
		string[] stringList = inputText.Split('\t', '\n'); //splits by tabs and lines
     return stringList;
 	}

 	List<float> parseFloatListFromString(int indexMin, int indexCount, string[] stringList){
 		// selects items, converts strings to floats and then creates a list
		int indexMax = stringList.Length - indexCount + indexMin;
		var index = Enumerable.Repeat(indexMin, (int)((indexMax - indexMin) / indexCount) + 1).Select((tr, ti) => tr + (indexCount * ti)).ToList();
		List<float> floatList  = index.Select(x => float.Parse(stringList[x])).ToList();
		return floatList;
 	}

 	IEnumerator waitRoutine(){
 		timeElapsed = false;
 		Debug.Log(Time.time);
 		Debug.Log("Wait " + secs[step] + " sec");
		yield return new WaitForSeconds(secs[step]);
		Debug.Log("Wait time elapsed");
		timeElapsed = true;
		arrived = false;
        rotated = false;
        step++;
	}

	bool isAtLocation(Vector3 currentPos, Vector3 targetPos, int rounder){
		// This function tests whether something is approxiamtely at a target location
		// Start with true and change to false as soon as one value is different
		bool isThere = true;
		
		// Check x position
		if(System.Math.Round(currentPos.x, rounder) != System.Math.Round(targetPos.x, rounder)){
			isThere = false;
		}

		// Check y position
		if(System.Math.Round(currentPos.y, rounder) != System.Math.Round(targetPos.y, rounder)){
			isThere = false;
		}

		// Check z position
		if(System.Math.Round(currentPos.z, rounder) != System.Math.Round(targetPos.z, rounder)){
			isThere = false;
		}
		return isThere;
	}

	bool isRotated(Quaternion currentRot, Quaternion targetRot){
		// This function tests whether something is approxiamtely at a target rotation
		// Start with true and change to false as soon as one value is different
		bool isThere = true;

		// Check x rotation
		if(System.Math.Round(currentRot.eulerAngles.x) != System.Math.Round(targetRot.eulerAngles.x)){
			isThere = false;
		}

		// Check y rotation
		if(System.Math.Round(currentRot.eulerAngles.y) != System.Math.Round(targetRot.eulerAngles.y)){
			isThere = false;
		}

		// Check z rotation
		if(System.Math.Round(currentRot.eulerAngles.z) != System.Math.Round(targetRot.eulerAngles.z)){
			isThere = false;
		}

		return isThere;
	}
}
