using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public int score = 0;

    public TextMeshProUGUI scoreText;

    public PlayerMovement gObj;

    public Health hObj;

    void Awake() {
        gObj = GameObject.Find("Player").GetComponent<PlayerMovement>();
        hObj = GameObject.Find("Player").GetComponent<Health>();

        if (gObj.isNewGame == false) {
            score = PlayerPrefs.GetInt("Player Score");
            //countToFifty = PlayerPrefs.GetInt("CountFifty"); // running more than once
        }
    }

    void Start() {
        
    }

    void Update() {

    	scoreText.text = score.ToString();
    }

    public void AddPoints(int points) {
    	score += points;

        // add to health every 50 pts 
        if (score % 50 == 0) {
            //hObj.health += 1;
            hObj.AddHealth();
            Debug.Log("HEALTH: " + hObj.health);
        }
    	//updatedScore = score;
    }

    public void OnApplicationQuit(){
         PlayerPrefs.SetInt("Player Score", 0);
         //Debug.Log("Reset score");
    }
}
