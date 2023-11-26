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

    [SerializeField]
    private TMP_Text help;

    public void Show() { gameObject.SetActive(true); }
    public void Hide() { gameObject.SetActive(false); }

    void Update()
    {
        if (isActiveAndEnabled && Input.GetKeyDown(KeyCode.Space))
        {
            Hide();
        }
    }
}
