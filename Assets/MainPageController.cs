using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPageController : MonoBehaviour {

    public GameObject PauseMenu;
    private bool isPaused = false;


    // Start is called before the first frame update
    void Start() {
        PauseMenu.SetActive(false);
        
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                ResumeGame();
            } else {
                PauseGame();
            }

        }
    }

    public void PauseGame() {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame() {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene("StartScene");
    }
}
