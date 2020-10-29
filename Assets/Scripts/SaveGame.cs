using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

   

public class SaveGame : MonoBehaviour
{
	public List<bool> boolStates = new List<bool>();
	public GameObject[] allObjects;
	public List<Vector3> positions = new List<Vector3>();
	public GameObject[] allMovingObjects;

	private int counter = 0;
	
	public Vector3 spawnPoint1;

	public Health hObj;
    public Score sObj;
    public PlayerMovement gObj;

    void Start() {
    	allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
    	allMovingObjects = GameObject.FindGameObjectsWithTag("Enemy");
    	sObj = GameObject.Find("Player").GetComponent<Score>();
		hObj = GameObject.Find("Player").GetComponent<Health>();
    	gObj = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

	void Update() {
		
	}

   public void Save() {
   		// clear save array
   		if (boolStates.Count > 1) {
   			Debug.Log("clearing");
   			boolStates.Clear();
   			positions.Clear();
   		}

        PlayerPrefs.SetInt("Player Score", sObj.score);
        PlayerPrefs.SetInt("Player Health", hObj.health); 

        // save player position
        spawnPoint1 = GameObject.Find("Player").transform.position; 

        // find all game objects
        foreach (GameObject go in allObjects)
         {
         	//Debug.Log(counter);
             boolStates.Add(go.activeInHierarchy);
             //Debug.Log(go.activeInHierarchy);
                 //child.gameObject.SetActive(false);
         }

        // find gameobjects that may have moved to return to orig position
        foreach (GameObject go in allMovingObjects)
         {
         	//Debug.Log(counter);
             positions.Add(go.transform.position);
             //Debug.Log(go.activeInHierarchy);
                 //child.gameObject.SetActive(false);
         }


        //saved = true;
        Debug.Log("save");

        //rObj.Respawn();  
        //this.transform.position = spawnPoint1.transform.position;    // move player 
    }
}
