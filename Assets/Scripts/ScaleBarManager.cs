using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScaleBarManager : MonoBehaviour {
    [SerializeField] private Image imageSlider;
    [SerializeField] private TMP_Text percentLeft;
    [SerializeField] private TMP_Text percentRight;

    int _desiredPercentage;

    private void Start() {
        UpdateSlider(50);
    }

    public void UpdateSlider(int percentValue) {
        _desiredPercentage = percentValue;
    }

    private void Update() {
        float updateSpeed = 4f / 100f;  // 4 % per second

        float currentFillAmount = imageSlider.fillAmount;
        float desiredFillAmount = (float)_desiredPercentage / 100f;
        float dist = updateSpeed * Time.deltaTime;

        float newFillAmount = currentFillAmount;
        if (Mathf.Abs(desiredFillAmount - currentFillAmount) <= dist) {
            newFillAmount = desiredFillAmount;
        }
        else {
            newFillAmount += (desiredFillAmount > currentFillAmount) ? dist : -dist;
        }

        imageSlider.fillAmount = newFillAmount;

        int currentPercentage = (int)Mathf.Round(100 * newFillAmount);

        percentLeft.text = currentPercentage + "%";
        percentRight.text = 100 - currentPercentage + "%";
    }
}
