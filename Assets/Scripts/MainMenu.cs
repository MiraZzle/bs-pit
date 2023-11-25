using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void Debate() {
        SceneManager.LoadSceneAsync(1);
    }

    public void Settings() {
        // TODO: add settings scene
    }

    public void Quit() {
        Application.Quit();
    }

}
