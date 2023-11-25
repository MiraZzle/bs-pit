using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightManager : MonoBehaviour
{
    public GameObject PlayerMask;
    public GameObject EnemyMask;
    public GameObject ModeratorMask;

    public void SetSpotlightPlayer(bool state) { PlayerMask.SetActive(state); }
    public void SetSpotlightEnemy(bool state) { EnemyMask.SetActive(state); }
    public void SetSpotlightModerator(bool state) { ModeratorMask.SetActive(state); }

    public void SetSpotlight(bool player, bool moderator, bool enemy)
    {
        SetSpotlightPlayer(player);
        SetSpotlightModerator(moderator);
        SetSpotlightEnemy(enemy);
    }
}
