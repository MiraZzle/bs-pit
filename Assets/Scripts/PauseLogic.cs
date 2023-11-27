using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseLogic : MonoBehaviour {
    // this script takes care of the pause menu


    [SerializeField] GameObject pauseMenu;

    void Start() {
        pauseMenu.SetActive(false);  // hide the pause menu at the start
    }

    bool isPaused = false;

    void Pause() {
        // pauses the game

        isPaused = true;
        AudioListener.pause = true;  // pause the music
        pauseMenu.SetActive(true);   // show the pause menu
        Time.timeScale = 0f;         // stop the flow of time 
    }

    void Resume() {
        // resumes the game

        isPaused = false;
        AudioListener.pause = false;  // resume the music
        pauseMenu.SetActive(false);   // hide the pause menu
        Time.timeScale = 1f;          // resume the flow of time
    }


    const string startSceneName = "StartScene";  // name of the starting scene 

    public void ReturnToStartScreen() {
        // returns back to the start screen

        Resume();   // resume the game first
        SceneManager.LoadScene(startSceneName);  // load the actual scene
    }


    void Update() {
        // shows / hides the pause menu upon pressing the space / escape keys 

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) Resume();
            else Pause();
        }
    }
}
