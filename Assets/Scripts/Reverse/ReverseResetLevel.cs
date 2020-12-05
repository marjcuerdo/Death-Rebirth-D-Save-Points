using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReverseResetLevel : MonoBehaviour
{

    public ReverseSaveGame sgObj;
    public ReverseHealth hObj;
    public ReverseScore sObj;
    public ReversePlayerMovement gObj;

    public ReverseTimer tObj;
    public bool restartedLevel = false;

    // Start is called before the first frame update
    void Start()
    {
        sgObj = GetComponent<ReverseSaveGame>();
        gObj = GameObject.Find("Player").GetComponent<ReversePlayerMovement>();
        tObj = GameObject.Find("Player").GetComponent<ReverseTimer>();
        hObj = GameObject.Find("Player").GetComponent<ReverseHealth>();
        sObj = GameObject.Find("Player").GetComponent<ReverseScore>();
    }

    public void RestartLevel() {
        // continue timer 
        PlayerPrefs.SetFloat("TimeRem", tObj.timeRemaining); 
        PlayerPrefs.SetFloat("TimeInc", tObj.timeInc);
        //PlayerPrefs.SetInt("Player Health", 5);
        //PlayerPrefs.SetInt("Player Score", 0);
        //PlayerPrefs.SetInt("Extra Hearts", 0);
        restartedLevel = true; // used to keep timer running after level restarts
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Reset() {

        if (gObj.lvlSavePointExists)  {
            sObj.score = PlayerPrefs.GetInt("Score");
            hObj.currentExtraHearts = PlayerPrefs.GetInt("Hearts");
            hObj.health = PlayerPrefs.GetInt("Health"); 
            hObj.j = 0;
            hObj.k = 0;
            hObj.tookDamage = (PlayerPrefs.GetInt("Damage") != 0); 
        } else {
            // retrieve score and health from beg of level
            sObj.score = PlayerPrefs.GetInt("Player Score");
            hObj.currentExtraHearts = PlayerPrefs.GetInt("Extra Hearts");
            hObj.health = PlayerPrefs.GetInt("Player Health"); 
            hObj.j = 0;
            hObj.k = 0;
            hObj.tookDamage = (PlayerPrefs.GetInt("Took Damage") != 0);           
        }


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
             sgObj.allMovingObjects[i].GetComponent<Rigidbody2D>().angularVelocity = 0; 
             // return to original rotation
             sgObj.allMovingObjects[i].transform.rotation = sgObj.rotations[i]; 
             //sgObj.allMovingObjects[i].GetComponent<Rigidbody2D>().Sleep();
             //sgObj.allMovingObjects[i].GetComponent<Rigidbody2D>().angularVelocity = Vector3.zero;
            }
             // place in original position
             sgObj.allMovingObjects[i].transform.position = sgObj.positions[i];
             //Debug.Log(sgObj.positions[i]);

         }
        
    }
}
