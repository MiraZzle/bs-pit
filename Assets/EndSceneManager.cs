using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;

public class EndSceneManager : MonoBehaviour {
    private Image textCandidate;
    private TextMeshProUGUI nameCandidate;
    private TextMeshProUGUI textContinue;
    public VideoPlayer player;
    public Canvas canvas;

    private float waitingTimeVideo = 4f;
    private bool videoPaused = false;


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

        canvas.enabled = false;
        player.playOnAwake = true;

        player.Prepare();
        player.Play();
        StartCoroutine(waitForVideoPause());

        textContinue.enabled = false;

        DrawImage();
    }

    void Update() {
        if (Input.GetKeyDown("space"))
            SceneManager.LoadScene("StartScene");
        
        Invoke("enableContinue", 5f);
    }



    IEnumerator waitForVideoPause() {
        Debug.Log("Čekám 4 sekundy");  
        yield return new WaitForSeconds(waitingTimeVideo);  // ceka 4 sekundy
        videoPaused = true;                                 // pausne video
        player.Pause();
        player.enabled = false;
        canvas.enabled = true;
        Debug.Log("Dočkal jsem se konce videa.");
    }

    private void enableContinue() {
        textContinue.enabled = true;
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

        PlayerPrefs.SetString("name", "Dobroděj Zlomocny");
        PlayerPrefs.SetInt("autenticita", 50);
        PlayerPrefs.SetInt("volici", 60);
    }
}
