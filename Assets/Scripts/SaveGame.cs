using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

   

public class SaveGame : MonoBehaviour
{
	public List<bool> boolStates = new List<bool>(); // store active/inactive states of gameobjects
	public GameObject[] allObjects; // store all gameobjects
	public List<Vector3> positions = new List<Vector3>(); // store positions of objects that may have moved
	public List<Quaternion> rotations = new List<Quaternion>(); // store rotations of objects that moved
	public GameObject[] allMovingObjects; // store all gameobjects that move
	
	public Vector3 spawnPoint1; 

	public Health hObj;
    public Score sObj;
    public PlayerMovement gObj;

    void Awake() {
    	// initialize spawn point at beg of level
    	spawnPoint1 = GameObject.Find("Player").transform.position; 
    }
    void Start() {
    	// initialize spawn point at beg of level
    	spawnPoint1 = GameObject.Find("Player").transform.position; 
    	
    	allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>(); // all gameobjects in scene
    	allMovingObjects = GameObject.FindGameObjectsWithTag("Enemy"); // enemy objects that moved
    	sObj = GameObject.Find("Player").GetComponent<Score>();
		hObj = GameObject.Find("Player").GetComponent<Health>();
    	gObj = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }


    public void Save() {
    	// remember that a save point exists for this level (for PlayerMovement.cs)
    	gObj.lvlSavePointExists = true;

   		// clear arrays for a new save
   		if (boolStates.Count > 1) {
   			Debug.Log("clearing");
   			boolStates.Clear();
   			positions.Clear();
   		}

   		// save score and health
        PlayerPrefs.SetInt("Player Score", sObj.score);
        PlayerPrefs.SetInt("Player Health", hObj.health); 

        // save player position
        spawnPoint1 = GameObject.Find("Player").transform.position; 

        // find all game objects
        foreach (GameObject go in allObjects)
         {
             boolStates.Add(go.activeInHierarchy); // add active/inactive status
         }

        // find gameobjects that may have moved to return to orig position
        foreach (GameObject go in allMovingObjects)
         {
             positions.Add(go.transform.position);
             rotations.Add(go.transform.rotation);
         }

        Debug.Log("Saved!");
    }
}
