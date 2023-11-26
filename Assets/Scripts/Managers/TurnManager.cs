using System.Collections;
using System.Collections.Generic;
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

    IEnumerator ModeratorSpeech()
    {
        _availableCards = cardManager.GetRandomCards();

        int index = 0;
        foreach (var cardBtn in _cardSlots)
        {
            var img = cardBtn.GetComponent<Image>();
            img.sprite = _availableCards[index++].Sprite;
            cardBtn.onClick.AddListener(() =>
                                        { HandleCards(cardBtn, _availableCards[index - 1]); });
        }

        yield return new WaitForSeconds(ModeratorDelay);

        spotlight.SetSpotlightModerator(true);

        Title.text = "intro";

        yield return new WaitForSeconds(ModeratorDelay);

        moderatorDialog.Show();

        yield return new WaitUntil(() => !moderatorDialog.IsActive);

        yield return new WaitForSeconds(ModeratorDelay);
        spotlight.SetSpotlightPlayer(true);

        moderatorDialog.SetText("Candidate A, a seasoned politician, has more political baggage than a 10-term senator at an airport carousel. Critics say they navigate issues with all the agility of a sloth in a speed-eating contest.");
        moderatorDialog.Show();
        yield return new WaitWhile(() => moderatorDialog.IsActive);

        _canContinue = false;
        yield return new WaitUntil(() => _canContinue);
        moderatorDialog.Hide();

        yield return new WaitForSeconds(ModeratorDelay);

        Player.InfoCard.Show();

        yield return new WaitUntil(() => !Player.InfoCard.IsOpen);

        spotlight.SetSpotlightEnemy(true);
        spotlight.SetSpotlightPlayer(false);
        moderatorDialog.SetText("Candidate B, the private sector enthusiast, brings as much political experience as a goldfish in a game of chess – but hey, who needs political know-how when you've got a dynamic PowerPoint presentation?");
        moderatorDialog.Show();

        yield return new WaitUntil(() => !moderatorDialog.IsActive);
        _canContinue = false;
        yield return new WaitUntil(() => _canContinue);

        moderatorDialog.Hide();
        Enemy.InfoCard.Show();

        yield return new WaitUntil(() => !Enemy.InfoCard.IsOpen);

        spotlight.SetSpotlightEnemy(false);

        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {

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

            foreach (var a in answers)
            {
                Debug.Log(a.Text);
            }

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
            c.DialogBox.Show();

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

    public void Skip() {
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
