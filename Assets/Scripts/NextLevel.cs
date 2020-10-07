using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{

	Scene m_Scene;
	string sceneName;

	public Score sObj;
	public Health hObj;

    
    private void Start() {
    	sObj = GetComponent<Score>();
    	hObj = GetComponent<Health>();
    	m_Scene = SceneManager.GetActiveScene();
    }

    public void LoadNextScene() {
    	if (m_Scene.name == "Level1") {

    		// Save score
    		PlayerPrefs.SetInt("Player Score", sObj.score);
    		PlayerPrefs.SetInt("Player Health", hObj.health);
    		Debug.Log("Current score is " + sObj.score.ToString());

    		SceneManager.LoadScene("Level2");
    	}
    }
}
