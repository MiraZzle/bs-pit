using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHoverLogic : MonoBehaviour
{
    Candidate candidate;
    TurnManager turnManager;

    private void Start() {
        candidate = GetComponent<Candidate>();
        turnManager = GameObject.FindGameObjectWithTag("logic").GetComponent<TurnManager>();
    }


    private void OnMouseEnter() {
        if (turnManager.IsDebating) {
            candidate.InfoCard.Show();
        }
    }
    private void OnMouseExit() {
        if (turnManager.IsDebating) {
            candidate.InfoCard.Hide();
        }
    }

    void Update()
    {
        
    }
}
