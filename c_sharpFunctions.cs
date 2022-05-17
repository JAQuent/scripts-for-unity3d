// Functions
// Reading in a .txt fie
string[] readText(string fileName){
	// Loads .txt file and split by \t and \n
	string inputText = System.IO.File.ReadAllText(fileName);
	string[] stringList = inputText.Split('\t', '\n'); //splits by tabs and lines
 	return stringList;
}

// Parsing informatiom from text file converting to list of floats
List<float> parseFloatListFromString(int indexMin, int indexCount, string[] stringList){
	// Selects items, converts strings to floats and then creates a list for floats
	int indexMax = stringList.Length - indexCount + indexMin;
	var index = Enumerable.Repeat(indexMin, (int)((indexMax - indexMin) / indexCount) + 1).Select((tr, ti) => tr + (indexCount * ti)).ToList();
	List<float> floatList = index.Select(x => float.Parse(stringList[x])).ToList();
	return floatList;
}

// Parsing information from text file converting to list of integer
List<int> parseIntListFromString(int indexMin, int indexCount, string[] stringList){
	// Selects items, converts strings to int and then creates a list for ints
	int indexMax = stringList.Length - indexCount + indexMin;
	var index = Enumerable.Repeat(indexMin, (int)((indexMax - indexMin) / indexCount) + 1).Select((tr, ti) => tr + (indexCount * ti)).ToList();
	List<int> intList = index.Select(x => int.Parse(stringList[x])).ToList();
	return intList;
}
///////////////
// Example use
// Loading input file
// This code needs using System.Linq;
string[] wayPointsString = readText(fileName);

// Parsing information
int indexCount = 7; // because there are 7 variables with 1 column for each variable
n = wayPointsString.Length/indexCount;
//Positions
indexMin = 0;
xPos     = parseFloatListFromString(indexMin, indexCount, wayPointsString);
///////////////

/// Check whether something has reached an approximate location
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

/// Check whether something has reached an approximate rotation
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


/// <summary>
/// This function takes a string and splits it into list based on character
/// </summary>
List<string> splitString2list(string string2split, string splitCharacter){
	// Needs using System.Linq;
	List<string> list = new List<string>();
    list = string2split.Split(splitCharacter).ToList();
    return list;
}