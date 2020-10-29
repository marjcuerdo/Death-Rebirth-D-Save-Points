using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHandler : MonoBehaviour {
    void Awake () {
        string keyName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + gameObject.name;
        if (PlayerPrefs.GetInt(keyName) == 0)
            gameObject.SetActive(false);
    }
    public void SetActiveState (bool state) {
        string keyName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + gameObject.name;
        PlayerPrefs.SetInt(keyName, state ? -1 : 0);
        gameObject.SetActive(state);
    }
}
   