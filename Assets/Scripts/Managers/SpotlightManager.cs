using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightManager : MonoBehaviour
{
    public GameObject PlayerMask;
    public GameObject EnemyMask;
    public GameObject ModeratorMask;

    private SoundManager soundManager;

    private void Start() {
        soundManager = GameObject.FindGameObjectWithTag("sound").GetComponent<SoundManager>();
    }

    public void SetSpotlightPlayer(bool state)
    {
        PlayerMask.SetActive(state);
        soundManager.PlaySpotlightSE();
    }
    public void SetSpotlightEnemy(bool state)
    {
        EnemyMask.SetActive(state);
        soundManager.PlaySpotlightSE();
    }
    public void SetSpotlightModerator(bool state)
    {
        ModeratorMask.SetActive(state);
        soundManager.PlaySpotlightSE();
    }
}
