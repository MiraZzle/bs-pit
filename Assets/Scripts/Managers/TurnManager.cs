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

    [SerializeField]
    private Candidate Player;
    [SerializeField]
    private Candidate Enemy;

    public float ModeratorDelay = 0.75f;

    void Start() { StartCoroutine(ModeratorSpeech()); }

    IEnumerator ModeratorSpeech()
    {
        yield return new WaitForSeconds(ModeratorDelay);

        spotlight.SetSpotlightModerator(true);

        yield return new WaitForSeconds(ModeratorDelay);

        moderatorDialog.Show();
        moderatorDialog.OnTypewriterEnd += () =>
        { StartCoroutine(ModeratorIntroduction()); };
    }

    IEnumerator ModeratorIntroduction()
    {
        var infos = Candidate.GetRandomInfo();

        Player.SetInfo(infos[0, 0], infos[0, 1], infos[0, 2]);
        Enemy.SetInfo(infos[1, 0], infos[1, 1], infos[1, 2]);

        moderatorDialog.Hide();
        spotlight.SetSpotlightPlayer(true);

        yield return new WaitForSeconds(ModeratorDelay);

        Player.InfoCard.Show();
    }

    void Turn()
    {

        questionManager.SetQuestion("TOTO JE OTAZKA?");
        questionManager.HandleAnswers = HandleAnswers;
        questionManager.SetAnswers(new List<string> { "ANO", "NE", "NEVIM" });

        questionManager.ShowQuestion(() => questionManager.ShowAnswers());
    }

    void HandleAnswers(AnswerButton answer) { questionManager.HideAnswers(); }
}
