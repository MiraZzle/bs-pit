using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightManager : MonoBehaviour
{
    public GameObject PlayerMask;
    public GameObject EnemyMask;
    public GameObject ModeratorMask;

    [SerializeField]
    private SoundManager sound;
    [SerializeField]
    private AudioClip turnOn;

    public void SetSpotlightPlayer(bool state)
    {
        PlayerMask.SetActive(state);
        sound.PlaySound(turnOn);
    }
    public void SetSpotlightEnemy(bool state)
    {
        EnemyMask.SetActive(state);
        sound.PlaySound(turnOn);
    }
    public void SetSpotlightModerator(bool state)
    {
        ModeratorMask.SetActive(state);
        sound.PlaySound(turnOn);
    }

    public void SetSpotlight(bool player, bool moderator, bool enemy)
    {
        SetSpotlightPlayer(player);
        SetSpotlightModerator(moderator);
        SetSpotlightEnemy(enemy);
    }
}
