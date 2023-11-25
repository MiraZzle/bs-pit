using UnityEngine;
using TMPro;

public class Answer : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;
    public int AnswerID;

    public void SetText(string text) { _text.text = text; }
}
