using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScaleBarManager : MonoBehaviour {
    [SerializeField] private Image imageSlider;
    [SerializeField] private TMP_Text percentLeft;
    [SerializeField] private TMP_Text percentRight;
    [SerializeField] private Canvas splitLine;

    private float percentVaule = 50f;
    private float imgWidth = 0, imgHeight = 0;

    void Start() {
        RectTransform rectTransform = imageSlider.GetComponent<RectTransform>();
        imgWidth = rectTransform.rect.width;
        imgHeight = rectTransform.rect.height;
    }

    void Update() {
        RectTransform splitTrasnform = splitLine.GetComponent<RectTransform>();
        Vector2 splitPos = splitTrasnform.anchoredPosition;

        float newX = splitPos.x, newY = splitPos.y;
        if (Input.GetMouseButtonDown(0) && percentVaule > 0) {
            percentVaule --;
            if (splitTrasnform.rect.width < splitTrasnform.rect.height)
                newX = splitPos.x - imgWidth/100;
            else
                newY = splitPos.y - imgHeight/100;
        } else if (Input.GetMouseButtonDown(1) && percentVaule < 100) {
            percentVaule++;
            if (splitTrasnform.rect.width < splitTrasnform.rect.height)
                newX = splitPos.x + imgWidth/100;
            else
                newY = splitPos.y + imgHeight/100;
        }

        imageSlider.fillAmount = percentVaule/100;
        splitTrasnform.anchoredPosition = new Vector2(newX, newY);
        percentLeft.text = percentVaule + "%";
        percentRight.text = 100-percentVaule + "%";
    }
}
