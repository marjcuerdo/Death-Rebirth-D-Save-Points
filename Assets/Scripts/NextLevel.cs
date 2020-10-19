using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{

	Scene m_Scene;
	string sceneName;

    
    private void Start() {
    	m_Scene = SceneManager.GetActiveScene();
    }

    public void LoadNextScene() {

        if (m_Scene.name == "Pre-Test Survey") {
            Debug.Log("Loading Level1");
            SceneManager.LoadScene("Level1");

            // load different version every time
            
        } else if (m_Scene.name == "Level1") {
            Debug.Log("Loading Level2");
    		SceneManager.LoadScene("Level2");
    	} else if (m_Scene.name == "New Scene") {
            SceneManager.LoadScene("Pre-Test Survey");
        } else if (m_Scene.name == "BegGame_Level1") {
            SceneManager.LoadScene("BegGame_Level2");
        } else
        { 
            Debug.Log("Loading nothing :(");
        
        }
    }
}
