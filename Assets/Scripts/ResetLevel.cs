using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{

	public SaveGame sgObj;
	public Timer tObj;
	public bool restartedLevel = false;

    // Start is called before the first frame update
    void Start()
    {
      	sgObj = GetComponent<SaveGame>();
      	tObj = GameObject.Find("Player").GetComponent<Timer>();
    }

    public void RestartLevel() {
    	// continue timer when player's health runs out
        PlayerPrefs.SetFloat("TimeRem", tObj.timeRemaining); 
        PlayerPrefs.SetFloat("TimeInc", tObj.timeInc);
        restartedLevel = true; // used to keep timer running after level restarts
    	SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Reset() {
    	// retrieve score and health when game was saved
        PlayerPrefs.GetInt("Player Score");
        PlayerPrefs.GetInt("Player Health"); 

        // set objects back to active/inactive
        for (int i= 0; i < sgObj.boolStates.Count; i++)
         {
             sgObj.allObjects[i].SetActive(sgObj.boolStates[i]);
         }

         // return moved objects to orig position
         for (int i= 0; i < sgObj.positions.Count; i++)
         {

         	// if an object has a rigidbody
         	if (sgObj.allMovingObjects[i].GetComponent<Rigidbody2D>() != null) {
         	// stop movement/remove forces on objects
         	 sgObj.allMovingObjects[i].GetComponent<Rigidbody2D>().velocity = Vector3.zero; 
         	 // return to original rotation
         	 sgObj.allMovingObjects[i].transform.rotation = sgObj.rotations[i]; 
         	 //sgObj.allMovingObjects[i].GetComponent<Rigidbody2D>().Sleep();
         	 //sgObj.allMovingObjects[i].GetComponent<Rigidbody2D>().angularVelocity = Vector3.zero;
         	}
         	 // place in original position
             sgObj.allMovingObjects[i].transform.position = sgObj.positions[i];
             Debug.Log(sgObj.positions[i]);

         }
        
    }
}
