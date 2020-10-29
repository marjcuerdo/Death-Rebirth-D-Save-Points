using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{

	public SaveGame sgObj;
	//private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
      	sgObj = GetComponent<SaveGame>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset() {
        PlayerPrefs.GetInt("Player Score");
            //Debug.Log("Score: " + sObj.score.ToString());
            //Debug.Log("Setting health");
        PlayerPrefs.GetInt("Player Health"); 

        // set objects back to active/inactive
        for (int i= 0; i < sgObj.boolStates.Count; i++)
         {
         	//Debug.Log(counter);
             sgObj.allObjects[i].SetActive(sgObj.boolStates[i]);
             //Debug.Log(sgObj.boolStates[i]);

             //counter += 1;
                 //child.gameObject.SetActive(false);
         }

         // return moved objects to orig position
         for (int i= 0; i < sgObj.positions.Count; i++)
         {

         	// if an object has a rigidbody
         	if (sgObj.allMovingObjects[i].GetComponent<Rigidbody2D>() != null) {
         		// stop movement
         	 sgObj.allMovingObjects[i].GetComponent<Rigidbody2D>().velocity = Vector3.zero;
         	 //sgObj.allMovingObjects[i].GetComponent<Rigidbody2D>().Sleep();
         	 sgObj.allMovingObjects[i].GetComponent<Rigidbody2D>().angularVelocity = 0;
         	}
         	 //
         	 // place in original position
             sgObj.allMovingObjects[i].transform.position = sgObj.positions[i];
             Debug.Log(sgObj.positions[i]);

             //counter += 1;
                 //child.gameObject.SetActive(false);
         }
        
    }
}
