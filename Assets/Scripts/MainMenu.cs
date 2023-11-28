using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    private Image flagImage;
    private Image debateImage;
    private Image languageImage;
    private Image quitImage;

    [SerializeField] private Sprite flagCZ;
    [SerializeField] private Sprite flagEN;
    [SerializeField] private Sprite debateCZ;
    [SerializeField] private Sprite debateEN;
    [SerializeField] private Sprite quitCZ;
    [SerializeField] private Sprite quitEN;
    [SerializeField] private Sprite languageCZ;
    [SerializeField] private Sprite languageEN;

    SoundManager soundManager;

    void Start() {
        soundManager = GameObject.FindGameObjectWithTag("sound").GetComponent<SoundManager>();

        GameObject objImg = GameObject.Find("Language");
        flagImage = objImg.GetComponent<Image>();

        GameObject langImg = GameObject.Find("LanguageButton");
        languageImage = langImg.GetComponent<Image>();
        
        GameObject debateImg = GameObject.Find("DebateButton");
        debateImage = debateImg.GetComponent<Image>();

        GameObject quitImg = GameObject.Find("QuitButton");
        quitImage = quitImg.GetComponent<Image>();

        if (!PlayerPrefs.HasKey("language")) {
            PlayerPrefs.SetString("language", "english");
        }
        DrawIcons();
    }

    public void Debate() {
        soundManager.PlayMouseClickSE();
        SceneManager.LoadScene("MainScene");
    }


    private void DrawIcons() {
        string language = PlayerPrefs.GetString("language");
        flagImage.sprite = (language == "english") ? flagEN : flagCZ;
        debateImage.sprite = (language == "english") ? debateEN : debateCZ;
        languageImage.sprite = (language == "english") ? languageEN : languageCZ;
        quitImage.sprite = (language == "english") ? quitEN : quitCZ;
    }

    public void ChangeLanguage() {
        soundManager.PlayMouseClickSE();

        string newLang = (PlayerPrefs.GetString("language") == "english") ? "czech" : "english";

        PlayerPrefs.SetString("language", newLang);
        DrawIcons();
    }

    public void Quit() {
        soundManager.PlayMouseClickSE();

        Application.Quit();
    }

}
