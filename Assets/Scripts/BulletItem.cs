using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletItem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;

    public void SetColor(Color color) { text.color = color; }

    public void SetText(string text) { this.text.text = text; }
}
