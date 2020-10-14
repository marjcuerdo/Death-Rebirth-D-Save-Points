using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public int score = 0;
    //public int updatedScore;

    public TextMeshProUGUI scoreText;

    void Awake() {
    	score = PlayerPrefs.GetInt("Player Score");
    }

    void Update() {

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
