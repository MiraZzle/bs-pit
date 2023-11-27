﻿using System.Collections;
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

    [SerializeField]
    Button skipButton;

    public float ModeratorDelay = 0.75f;

    private Card[] _availableCards;

    [SerializeField]
    private Button[] _cardSlots;

    bool _hasCard = false;

    bool skipIntro = false;
    void Start()
    {
        Debug.Log("TADY SE VOLA START");

        if (skipIntro) {
            StartCoroutine(GameLoop());
        }
        else {
            StartCoroutine(IntroModeratorSpeech());
        }
    }

    bool PressedContinue() => Input.GetKeyDown(KeyCode.Space);

    IEnumerator IntroModeratorSpeech() {
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
        yield return new WaitForSeconds(2*ModeratorDelay);
        
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        // setup
        _availableCards = cardManager.GetRandomCards();
        debateManager.SetUpQuestions();
        int index = 0;
        foreach (var cardBtn in _cardSlots) {
            var img = cardBtn.GetComponent<Image>();
            img.sprite = _availableCards[index++].Sprite;
            cardBtn.onClick.AddListener(() => { HandleCards(cardBtn, _availableCards[index - 1]); });
        }

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
                if (lastQuestion is not null) spotlight.SetSpotlightModerator(true);
                if (lastQuestion is not null) yield return new WaitForSeconds(ModeratorDelay);

                questionManager.ShowQuestion(question, answers);
                yield return new WaitUntil(() => !questionManager.IsActive);
                yield return new WaitForSeconds(ModeratorDelay);
                spotlight.SetSpotlightModerator(false);
            }

            yield return new WaitForSeconds(ModeratorDelay);
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
            Debug.Log(selectedAnswer.Text);
            selectedAnswer.IsUsed = true;
            debateManager.ProcessAnswer(selectedAnswer);
            candidate.DialogBox.SetText(selectedAnswer.Text);
            candidate.DialogBox.PlayText();

            yield return new WaitUntil(() => !candidate.DialogBox.IsActive);

            if (candidate == debateManager.Enemy && debateManager.ShouldShowCards)
            {
                yield return new WaitForSeconds(ModeratorDelay);
                _hasCard = false;
                ShowCards();
                yield return new WaitUntil(() => _hasCard);
                HideCards();
            }
            else {
                yield return new WaitUntil(() => PressedContinue());
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

        //if (Player.Authenticity <= debateManager.MinAuthenticity || Enemy.Authenticity <= debateManager.MinAuthenticity) {
            // nekoho vyhodime
        //}
        else {
            StartCoroutine(DebateOutro());
        }
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
        if (Input.GetKeyDown(KeyCode.Return)) {
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
