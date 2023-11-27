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

    public event Action OnClose;

    public bool IsOpen { get; private set; }

    public void Show()
    {
        gameObject.SetActive(true);
        IsOpen = true;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        IsOpen = false;
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
