using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{

	Scene m_Scene;
	string sceneName;

    int gameVersion;
    int [] versions = new int [] {1, 2, 3, 4}; // scene numbers for each game version

    
    private void Start() {
    	m_Scene = SceneManager.GetActiveScene();
    }

    public void LoadNextScene() {

        if (m_Scene.name == "Pre-Test Survey") {
            // FIX ORDERING HERE
            //gameVersion = Random.Range(0,5);
            //Debug.Log(gameVersion);

            SceneManager.LoadScene("Level1");
            // load random version every time
        } else if (m_Scene.name == "BegLevel_A") {
            SceneManager.LoadScene("Level1");
        } else if (m_Scene.name == "BegGame_B") {
            SceneManager.LoadScene("BegGame_Level1");
        } else if (m_Scene.name == "Check_C") {
            SceneManager.LoadScene("Check_Level1");
        } else if (m_Scene.name == "Save_D") {
            SceneManager.LoadScene("Save_Level1");
        } else if (m_Scene.name == "Level1") {
            Debug.Log("Loading Level2");
    		SceneManager.LoadScene("Level2");
    	} else if (m_Scene.name == "BegGame_Level1") {
            SceneManager.LoadScene("BegGame_Level2");
        } else
        { 
            Debug.Log("Loading nothing :(");
        
        }
    }
}
