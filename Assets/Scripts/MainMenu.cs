using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    private bool cz = false;        // jazyk
    private Image languageImage;
    private Image debateImage;
    private Image langImage;
    private Image quitImage;

    [SerializeField] private Sprite czImg;
    [SerializeField] private Sprite enImg;
    [SerializeField] private Sprite debateCz;
    [SerializeField] private Sprite debateEn;
    [SerializeField] private Sprite quitCz;
    [SerializeField] private Sprite quitEn;
    [SerializeField] private Sprite langCz;
    [SerializeField] private Sprite langEn;

    void Start() {
        GameObject objImg = GameObject.Find("Language");
        languageImage = objImg.GetComponent<Image>();

        GameObject langImg = GameObject.Find("LanguageButton");
        langImage = langImg.GetComponent<Image>();
        
        GameObject debateImg = GameObject.Find("DebateButton");
        debateImage = debateImg.GetComponent<Image>();

        GameObject quitImg = GameObject.Find("QuitButton");
        quitImage = quitImg.GetComponent<Image>();
    }

    public void Debate() {
        SceneManager.LoadSceneAsync(1);
        Debug.Log("CZ: " + cz);
    }

    public void LanguageChange() {
        if (languageImage.sprite == enImg){
            cz = true;
            languageImage.sprite = czImg;

            debateImage.sprite = debateCz;
            langImage.sprite = langCz;
            quitImage.sprite = quitCz;
        } else {
            cz = false;
            languageImage.sprite = enImg;

            debateImage.sprite = debateEn;
            langImage.sprite = langEn;
            quitImage.sprite = quitEn;
        }
    }

    public void Quit() {
        Application.Quit();
    }

}
