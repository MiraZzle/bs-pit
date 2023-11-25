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
    private CharacterCard Card;

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
        moderatorDialog.Hide();
        spotlight.SetSpotlightPlayer(true);

        yield return new WaitForSeconds(ModeratorDelay);

        Card.Name.text = "Bohuslav Novák";
        Card.Age.text = "50";
        Card.Mastery.text = "Alkoholik";
        Card.Positives.text = "Charita";
        Card.Negatives.text = "Komunista";

        Card.Show();

        yield return new WaitForSeconds(ModeratorDelay);

        Card.Hide();

        Turn();
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
