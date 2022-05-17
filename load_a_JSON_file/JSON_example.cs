using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// Attach this script to any game object to try it out. Place the example.json file into a folder called "StreamingAssets". 
public class JSON_example : MonoBehaviour{
	// Public vars
	public string JSON_fileName = "example.json";

    // Each variable that is included in your json file, needs to be included here. 
    // In the example there are two variables a string called var1 and decimal number called var2. 
	[System.Serializable]
    public class JSONDataClass {
    	public string var1;
    	public float var2;
    }

	// Declare the variable into which we save the content of the .json file. 
	private JSONDataClass JSONData;

    // Start is called before the first frame update
    void Start(){
    	// Get the Startup Text from the JSON file
    	GetDataFromJSON(JSON_fileName);

    	// Show that it worked
        Debug.Log(JSONData.var1);
        Debug.Log(JSONData.var2);
    	
    }

    /// <summary>
    /// Method to read in a JSON file that is placed in the StreamingAssets folder. 
    /// </summary>
    void GetDataFromJSON(string fileName){
    	// Get path
        string path2file = Path.GetFullPath(Path.Combine(Application.streamingAssetsPath, fileName));

        // Get JSON input
        var sr = new StreamReader(path2file);
        var fileContents = sr.ReadToEnd();
        sr.Close();

        // Get instruction data profile
        JSONData = JsonUtility.FromJson<JSONDataClass>(fileContents);
    }
}
