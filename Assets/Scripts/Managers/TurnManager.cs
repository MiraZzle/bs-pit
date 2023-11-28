using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#nullable enable

public class TurnManager : MonoBehaviour
{

    [SerializeField]
    private QuestionManager questionManager;

    [SerializeField]
    private DebateManager debateManager;

    [SerializeField]
    private CardManager cardManager;

    [SerializeField]
    private Dialog moderatorDialog;

    [SerializeField]
    private SpotlightManager spotlight;

    [SerializeField]
    private Candidate Player;
    [SerializeField]
    private Candidate Enemy;

    public float ModeratorDelay = 0.75f;

    static bool _cardHandled = false;
    public static bool CardAnimationPlaying { get; set; } = false;
    public static bool IsDebating { get; private set; } = false;

    bool skipIntro = false;
    void Start()
    {
        if (skipIntro) {
            StartCoroutine(GameLoop());
        }
        else {
            StartCoroutine(IntroModeratorSpeech());
        }
    }

    bool PressedContinue() => Input.GetKeyDown(KeyCode.Space);

    IEnumerator IntroModeratorSpeech() {
        IsDebating = false;

        yield return new WaitForSeconds(ModeratorDelay);
        spotlight.SetSpotlightModerator(true);
        yield return new WaitForSeconds(ModeratorDelay);

        moderatorDialog.SetText(debateManager.GetIntroText());
        moderatorDialog.PlayText();
        yield return new WaitUntil(() => !moderatorDialog.IsActive);
        yield return new WaitUntil(() => PressedContinue());

        moderatorDialog.Hide();

        spotlight.SetSpotlightPlayer(true);
        yield return new WaitForSeconds(ModeratorDelay);

        moderatorDialog.SetText(debateManager.GetPlayerIntroText());
        moderatorDialog.PlayText();
        yield return new WaitWhile(() => moderatorDialog.IsActive);
        yield return new WaitUntil(() => PressedContinue());
        moderatorDialog.Hide();

        yield return new WaitForSeconds(ModeratorDelay);
        Player.InfoCard.Show();
        yield return new WaitUntil(() => !Player.InfoCard.IsOpen);

        yield return new WaitForSeconds(ModeratorDelay);
        spotlight.SetSpotlightPlayer(false);
        yield return new WaitForSeconds(ModeratorDelay);
        spotlight.SetSpotlightEnemy(true);
        yield return new WaitForSeconds(ModeratorDelay);

        moderatorDialog.SetText(debateManager.GetEnemyIntroText());
        moderatorDialog.PlayText();
        yield return new WaitUntil(() => !moderatorDialog.IsActive);
        yield return new WaitUntil(() => PressedContinue());
        moderatorDialog.Hide();

        yield return new WaitForSeconds(ModeratorDelay);
        Enemy.InfoCard.Show();
        yield return new WaitUntil(() => !Enemy.InfoCard.IsOpen);
        yield return new WaitForSeconds(ModeratorDelay);
        spotlight.SetSpotlightEnemy(false);
        yield return new WaitForSeconds(ModeratorDelay);

        moderatorDialog.SetText(debateManager.GetStartQuestionsIntroText());
        moderatorDialog.PlayText();
        yield return new WaitUntil(() => !moderatorDialog.IsActive);
        yield return new WaitUntil(() => PressedContinue());
        moderatorDialog.Hide();
        yield return new WaitForSeconds(1.5f*ModeratorDelay);
        
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        IsDebating = true;

        // setup
        debateManager.SetUpQuestions();
        cardManager.SetUpRandomCards();
        debateManager.ShowBars();

        Question? lastQuestion = null;
        // ask questions
        while (true)
        {
            (Question question, Candidate candidate) = debateManager.AskAnotherQuestion();
            if (question is null)
            {
                // if the debate ended --> ukoncit debatu
                break;
            }

            // show question
            var answers = question.GetAnswers();

            // if this is the second turn of a general question, dont display it again
            if (question != lastQuestion) {
                // if this is not the first question
                if (lastQuestion is not null) {
                    spotlight.SetSpotlightModerator(true);
                    yield return new WaitForSeconds(ModeratorDelay);
                }

                questionManager.ShowQuestion(question, answers);
                yield return new WaitUntil(() => !questionManager.IsActive);
                yield return new WaitForSeconds(ModeratorDelay);
                spotlight.SetSpotlightModerator(false);
            }
            else {
                yield return new WaitForSeconds(ModeratorDelay);
            }

            Answer selectedAnswer;
            if (candidate == Player)
            {
                // if it is for the player, show answers
                spotlight.SetSpotlightPlayer(true);
                yield return new WaitForSeconds(ModeratorDelay);

                questionManager.ShowAnswers();
                yield return new WaitUntil(() => questionManager.HasAnswer);
                questionManager.HideAnswers();
                selectedAnswer = questionManager.Selected;
            }
            else
            {
                spotlight.SetSpotlightEnemy(true);
                yield return new WaitForSeconds(ModeratorDelay);

                // pick a random answer
                answers.Shuffle();
                selectedAnswer = answers[0];
            }

            // process the answer
            selectedAnswer.IsUsed = true;
            candidate.DialogBox.SetText(selectedAnswer.Text);
            candidate.DialogBox.PlayText();

            yield return new WaitUntil(() => !candidate.DialogBox.IsActive);

            if (candidate == debateManager.Enemy && debateManager.ShouldShowCards)
            {
                yield return new WaitForSeconds(ModeratorDelay);
                debateManager.ProcessAnswer(selectedAnswer);

                _cardHandled = false;
                cardManager.ShowCards();
                yield return new WaitUntil(() => _cardHandled);
                cardManager.HideCards();
            }
            else {
                yield return new WaitUntil(() => PressedContinue());
                debateManager.ProcessAnswer(selectedAnswer);
            }


            candidate.DialogBox.Hide();
            spotlight.SetSpotlightEnemy(false);
            spotlight.SetSpotlightPlayer(false);
            lastQuestion = question;
        }

        questionManager.HideAnswers();
        questionManager.HideQuestion();
        yield return new WaitForSeconds(ModeratorDelay);
        spotlight.SetSpotlightModerator(true);
        yield return new WaitForSeconds(ModeratorDelay);

        IsDebating = false;

        //if (Player.Authenticity <= debateManager.MinAuthenticity || Enemy.Authenticity <= debateManager.MinAuthenticity) {
        // nekoho vyhodime
        //}
        //else {
            StartCoroutine(DebateOutro());
        //}
    }

    IEnumerator DebateOutro() {
        moderatorDialog.SetText(debateManager.GetOutroText());
        moderatorDialog.PlayText();
        yield return new WaitUntil(() => !moderatorDialog.IsActive);
        yield return new WaitUntil(() => PressedContinue());
        moderatorDialog.Hide();

        // save results and load end scene
        PlayerPrefs.SetInt("autenticita", debateManager.PlayerAuthenticity);
        PlayerPrefs.SetInt("volici", debateManager.PlayerVoters);
        SceneManager.LoadScene("EndScene");
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKey(KeyCode.E)) {
            moderatorDialog.Skip = true;
            Player.DialogBox.Skip = true;
            Enemy.DialogBox.Skip = true;
            questionManager.SkipTyping();
        }
        if (Input.GetKeyDown(KeyCode.Space) && !CardAnimationPlaying) {
            _cardHandled = true;
        }
    }

    public static void CardHandled() {
        _cardHandled = true;
    }
}
