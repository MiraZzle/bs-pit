using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candidate : MonoBehaviour
{
    [SerializeField]
    private ProgressBar authenticityBar;

    public int Authenticity
    {
        get => (int)authenticityBar.Value;
    }

    public void IncreaseAuthenticity(int value) { authenticityBar.Value += value; }
    public void DecreaseAuthenticity(int value) { authenticityBar.Value -= value; }
}
