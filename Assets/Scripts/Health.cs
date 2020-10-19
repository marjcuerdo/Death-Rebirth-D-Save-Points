using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health = 5;
    public int numOfHearts = 5;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public PlayerMovement gObj;


    void Awake() {
        // not available on first load
        /*Debug.Log("getting player health");
        health = PlayerPrefs.GetInt("Player Health");*/
        gObj = GameObject.Find("Player").GetComponent<PlayerMovement>();
        //Debug.Log("isNewGame 2: " + gObj.isNewGame.ToString());
        /*if (gObj.isNewGame == true)
        {
                Debug.Log("This is a new level");
                
        } */
    }

    void Update() {

        if (gObj.isNewGame == false) {
           // Debug.Log("getting player health: " + PlayerPrefs.GetInt("Player Health").ToString());
            health = PlayerPrefs.GetInt("Player Health");
            //Debug.Log("again player health: " + PlayerPrefs.GetInt("Player Health").ToString());
            gObj.isNewGame = true;
        }

    	if (health > numOfHearts) {
    		health = numOfHearts;
    	}

    	for (int i=0; i < hearts.Length; i++) {

    		if (i < health) {
    			hearts[i].sprite = fullHeart;
    		} else {
    			hearts[i].sprite = emptyHeart;
    		}

    		if (i < numOfHearts) {
    			hearts[i].enabled = true;
    		} else {
    			hearts[i].enabled = false;
    		}
    	}


    }

    void Start() {    

    }

    public void TakeDamage(int damage) {
    	health -= damage;
    }

    public void AddHealth() {
        health += 1;

    }

    public void OnApplicationQuit(){
         PlayerPrefs.SetInt("Player Health", 5);

         //Debug.Log("Reset health");
    }

}
