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

    void Awake() {
        gObj = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void Start() {
        
    }

    void Update() {
        if (gObj.isNewGame == false || gObj.isDead == true) {
            //Debug.Log("getting player score: " + PlayerPrefs.GetInt("Player Score").ToString());
            score = PlayerPrefs.GetInt("Player Score");
            //Debug.Log("again player score: " + PlayerPrefs.GetInt("Player Score").ToString());
        }
    	scoreText.text = "Score: " + score.ToString();
    }

    public void AddPoints(int points) {
    	score += points;
    	//updatedScore = score;
    }

    public void OnApplicationQuit(){
         PlayerPrefs.SetInt("Player Score", 0);
         //Debug.Log("Reset score");
    }
}
