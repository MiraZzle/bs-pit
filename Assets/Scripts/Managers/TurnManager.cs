using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField]
    private TMP_Text Title;

    public float ModeratorDelay = 0.75f;

    void Start()
    {
        Title.text = "";
        StartCoroutine(ModeratorSpeech());
    }

    IEnumerator ModeratorSpeech()
    {
        yield return new WaitForSeconds(ModeratorDelay);

        spotlight.SetSpotlightModerator(true);

        Title.text = "intro";

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

    void QuestionTurn()
    {
        Title.text = "question phase";

        questionManager.SetQuestion("TOTO JE OTAZKA?");
        questionManager.HandleAnswers = HandleAnswers;
        questionManager.SetAnswers(new List<string> { "ANO", "NE", "NEVIM" });

        questionManager.ShowQuestion(() => questionManager.ShowAnswers());
    }

    void HandleAnswers(AnswerButton answer) { questionManager.HideAnswers(); }

    void AttackTurn() { Title.text = "attack phase"; }
}
