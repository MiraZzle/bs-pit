using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candidate : MonoBehaviour {
    [SerializeField]
    private ProgressBar authenticityBar;

    private int maxAuthenticity = 100;

    public int Authenticity { get => (int)authenticityBar.Value; }

    public void ChangeAuthenticity(int deltaAuthenticity) {
        authenticityBar.Value = Mathf.Clamp(deltaAuthenticity, 0, maxAuthenticity);
        if (Authenticity <= maxAuthenticity / 10) {
            // auto lose game
        }
    }
}
