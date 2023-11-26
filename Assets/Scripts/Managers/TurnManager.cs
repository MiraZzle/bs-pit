using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    [SerializeField]
    private QuestionManager questionManager;
    [SerializeField]
    private DebateManager debateManager;

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

        yield return new WaitUntil(() => !moderatorDialog.IsActive);

        moderatorDialog.SetText("Player dasdsa das d asd asd as");
        moderatorDialog.Show();
        yield return new WaitWhile(() => moderatorDialog.IsActive);

        moderatorDialog.Hide();
        spotlight.SetSpotlightPlayer(true);

        yield return new WaitForSeconds(ModeratorDelay);

        Player.InfoCard.Show();

        yield return new WaitUntil(() => !Player.InfoCard.IsOpen);
        
        spotlight.SetSpotlightPlayer(false);
        moderatorDialog.SetText("Enemy dasdsa das d asd asd as");
        moderatorDialog.Show();
        spotlight.SetSpotlightEnemy(true);

        yield return new WaitUntil(() => !moderatorDialog.IsActive);

        moderatorDialog.Hide();
        Enemy.InfoCard.Show();

        yield return new WaitUntil(() => !Enemy.InfoCard.IsOpen);

        spotlight.SetSpotlightEnemy(false);

        StartCoroutine(QuestionTurn());
    }

    IEnumerator QuestionTurn()
    {
        Title.text = "question phase";

        (Question q, Candidate c) = debateManager.AskAnotherQuestion();
        if (q is null) {
            // konec otazek --> konec hry
        }

        questionManager.ShowQuestion(q);

        yield return new WaitUntil(() => questionManager.HasAnswer);

        Player.DialogBox.SetText(questionManager.Selected.Text);
        Player.DialogBox.Show();

        questionManager.HideQuestion();

        yield return new WaitUntil(() => !Player.DialogBox.IsActive);

        yield return new WaitForSeconds(ModeratorDelay);

        Player.DialogBox.Hide();
    }

    IEnumerator AttackTurn()
    {
        Title.text = "attack phase";

        yield break;
    }
}
