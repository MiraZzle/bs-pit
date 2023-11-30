using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoCard : MonoBehaviour
{
    public Image Icon;
    public TMP_Text Name;
    public TMP_Text Age;
    public TMP_Text Bio;
    public TMP_Text Mastery;
    public TMP_Text MasteryName;
    public TMP_Text Positives;
    public TMP_Text Negatives;
    [SerializeField] TMP_Text pressSpaceToContinue;

    public event Action OnClose;

    public bool IsOpen { get; private set; }

    public void Show()
    {
        _showHelp = true;
        Invoke(nameof(ShowHelp), 1f);
        gameObject.SetActive(true);
        IsOpen = true;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        IsOpen = false;
    }

    bool _showHelp = false;
    void ShowHelp() {
        if (_showHelp) pressSpaceToContinue.gameObject.SetActive(true);
    }

    public void HidePressSpaceToContinue() {
        _showHelp = false;
        pressSpaceToContinue.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isActiveAndEnabled && Input.GetKeyDown(KeyCode.Space))
        {
            Hide();
            OnClose?.Invoke();
        }
    }
}
