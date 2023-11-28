using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHoverLogic : MonoBehaviour
{
    Candidate candidate;

    private void Start() {
        candidate = gameObject.GetComponent<Candidate>();
    }

    private bool _shouldHide = false;

    private void OnMouseEnter() {
        _shouldHide = false;
        if (TurnManager.IsDebating && !PauseLogic.IsPaused) {
            candidate.InfoCard.Show();
        }
    }

    private void OnMouseExit() {
        if (TurnManager.IsDebating) {
            _shouldHide = true;
        }
    }

    void Update()
    {
        if (candidate == null) {
            Debug.Log("candidate je null");
            return;
        }
        if (_shouldHide && !PauseLogic.IsPaused) {
            candidate.InfoCard.Hide();
        }
    }
}
