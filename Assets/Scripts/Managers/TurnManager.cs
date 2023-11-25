using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    [SerializeField]
    private QuestionManager questionManager;

    [SerializeField]
    private Dialog moderatorDialog;

    [SerializeField]
    private SpotlightManager spotlight;

    public float ModeratorDelay = 0.75f;

    void Start() { StartCoroutine(ModeratorSpeech()); }

    IEnumerator ModeratorSpeech()
    {
        yield return new WaitForSeconds(ModeratorDelay);

        spotlight.SetSpotlightModerator(true);

        yield return new WaitForSeconds(ModeratorDelay);

        moderatorDialog.Show();
        moderatorDialog.OnTypewriterEnd += ModeratorIntroduction;
    }

    void ModeratorIntroduction() { }
}
