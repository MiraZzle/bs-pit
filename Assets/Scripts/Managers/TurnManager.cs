using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [SerializeField]
    Button skipButton;

    [SerializeField]
    private TMP_Text Title;

    public float ModeratorDelay = 0.75f;

    private Card[] _availableCards;

    [SerializeField]
    private Button[] _cardSlots;

    bool _hasCard = false;
    bool _canContinue = false;

    void Start()
    {
        Title.text = "";
        StartCoroutine(ModeratorSpeech());
    }

    bool PressedContinue() => Input.GetKeyDown(KeyCode.Space);
    bool _inIntro = true;

    IEnumerator ModeratorSpeech() {
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

        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        _inIntro = false;
        debateManager.ShowBars();

        _availableCards = cardManager.GetRandomCards();
        int index = 0;
        foreach (var cardBtn in _cardSlots) {
            var img = cardBtn.GetComponent<Image>();
            img.sprite = _availableCards[index++].Sprite;
            cardBtn.onClick.AddListener(() => { HandleCards(cardBtn, _availableCards[index - 1]); });
        }

        // ask questions
        while (true)
        {
            Title.text = "question phase";

            (Question q, Candidate c) = debateManager.AskAnotherQuestion();
            if (q is null)
            {
                // end game
                yield return new WaitForSeconds(1);

                PlayerPrefs.SetInt("autenticita", debateManager.PlayerAuthenticity);
                PlayerPrefs.SetInt("volici", debateManager.PlayerVoters);

                SceneManager.LoadScene("EndScene");
                break;
            }

            var answers = q.GetAnswers();
            questionManager.ShowQuestion(q, answers);
            yield return new WaitUntil(() => !questionManager.IsActive);

            Answer selectedAnswer;
            if (c == Player)
            {
                questionManager.ShowAnswers();
                yield return new WaitUntil(() => questionManager.HasAnswer);
                questionManager.HideAnswers();
                selectedAnswer = questionManager.Selected;
            }
            else
            {
                answers.Shuffle();
                selectedAnswer = answers[0];
            }
            selectedAnswer.IsUsed = true;

            debateManager.ProcessAnswer(selectedAnswer);
            c.DialogBox.SetText(selectedAnswer.Text);
            c.DialogBox.PlayText();

            yield return new WaitUntil(() => !c.DialogBox.IsActive);

            if (c != Player && debateManager.numCardsUsed < 4)
            {
                _hasCard = false;
                Title.text = "attack phase";

                ShowCards();

                yield return new WaitUntil(() => _hasCard);
                HideCards();
            }

            c.DialogBox.Hide();
        }
    }

    public void SkipCardAttack() {
        _hasCard = true;
    }

    void HideCards()
    {
        skipButton.gameObject.SetActive(false);

        foreach (var btn in _cardSlots)
        {
            btn.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _canContinue = true;
        }
        if (Input.GetKeyDown(KeyCode.Return) && _inIntro) {
            moderatorDialog.Skip = true;
        }
    }

    void ShowCards()
    {
        skipButton.gameObject.SetActive(true);

        foreach (var btn in _cardSlots)
        {
            btn.gameObject.SetActive(true);

        }

    }

    void HandleCards(Button btn, Card card)
    {
        btn.interactable = false;
        btn.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);

        debateManager.ProcessCardAttack(card);

        _hasCard = true;
    }
}
