using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndSceneManager : MonoBehaviour {
    private Image textCandidate;
    private TextMeshProUGUI nameCandidate;
    private TextMeshProUGUI textContinue;

    private float timer = 0f;

    // ve formatu auth 50% plus/minus a hlasu 50% plus/minus
    [SerializeField] private Sprite plusplusCz;
    [SerializeField] private Sprite plusplusEn;
    [SerializeField] private Sprite plusminusCz;
    [SerializeField] private Sprite plusminusEn;
    [SerializeField] private Sprite minusplusCz;
    [SerializeField] private Sprite minusplusEn;
    [SerializeField] private Sprite minusminusCz;
    [SerializeField] private Sprite minusminusEn;

    void Start() {
        changeTest();
        GameObject textObj = GameObject.Find("textCandidate");
        textCandidate = textObj.GetComponent<Image>();

        GameObject nameObj = GameObject.Find("nameCandidate");
        nameCandidate = nameObj.GetComponent<TextMeshProUGUI>();

        GameObject contObj = GameObject.Find("textContinue");
        textContinue = contObj.GetComponent<TextMeshProUGUI>();

        textContinue.enabled = false;

        DrawImage();
    }

    void Update() {
        if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey ("enter")) {
            SceneManager.LoadSceneAsync(0);
        }

        if (timer < 2.3f) {
            timer += 0.01f;
        } else {
            textContinue.enabled = true;
        }

        Debug.Log(timer);

    }
    
    private void DrawImage() {
        string language = PlayerPrefs.GetString("language");
        string name = PlayerPrefs.GetString("name");
        int autenticita = PlayerPrefs.GetInt("autenticita");
        int volici = PlayerPrefs.GetInt("volici");
        
        nameCandidate.text = name.ToUpper();

        if ((autenticita >= 50) && (volici >= 50)) textCandidate.sprite = (language == "english") ? plusplusEn : plusplusEn;
        if ((autenticita >= 50) && (volici < 50)) textCandidate.sprite = (language == "english") ? plusminusEn : plusminusCz;
        if ((autenticita < 50) && (volici >= 50)) textCandidate.sprite = (language == "english") ? minusplusEn : minusplusCz;
        if ((autenticita < 50) && (volici < 50)) textCandidate.sprite = (language == "english") ? minusminusEn : minusminusCz;
    }

    private void changeTest() {
        PlayerPrefs.SetString("language", "english");

        PlayerPrefs.SetString("name", "Honimír Obrovsky");
        PlayerPrefs.SetInt("autenticita", 50);
        PlayerPrefs.SetInt("volici", 60);
    }
}
