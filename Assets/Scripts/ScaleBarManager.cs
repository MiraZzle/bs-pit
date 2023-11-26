using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScaleBarManager : MonoBehaviour {
    [SerializeField] private Image imageSlider;
    [SerializeField] private TMP_Text percentLeft;
    [SerializeField] private TMP_Text percentRight;


    private void Start() {
        UpdateSlider(50f);
    }

    public void UpdateSlider(float percentValue) {
        imageSlider.fillAmount = percentValue / 100f;

        if (percentLeft is not null) {
            percentLeft.text = percentValue + "%";
            percentRight.text = 100 - percentValue + "%";
        }
    }

    private void Update() {
        imageSlider.fillAmount = 0.4f;
    }
}
