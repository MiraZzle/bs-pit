using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;
    public Answer Ans;
    public void SetText(string text) { _text.text = text; }
}
