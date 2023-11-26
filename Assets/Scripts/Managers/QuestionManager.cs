using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    public Dialog Question;
    public AnswersManager AnswersManager;

    public Action<Answer> Handler;
    public Answer Selected { get; private set; }
    public bool IsActive { get; private set; }
    public bool HasAnswer { get; private set; }

    void Start() { AnswersManager.Handler += HandleAnswer; }

    public void ShowQuestion(Question question, Answer[] answers)
    {
        AnswersManager.SetAnswers(answers);
        Selected = null;
        Question.SetText(question.Text);
        IsActive = true;
        HasAnswer = false;
        StartCoroutine(ShowAll());
    }

    private IEnumerator ShowAll()
    {
        Question.Show();
        yield return new WaitUntil(() => !Question.IsActive);

        IsActive = false;
    }

    public void ShowAnswers() { AnswersManager.Show(); }

    public void HideQuestion()
    {
        Question.Hide();
        AnswersManager.Hide();
    }

    public void HideAnswers() { AnswersManager.Hide(); }
    private void HandleAnswer(Answer answer)
    {
        AnswersManager.ClearAnswers();
        Selected = answer;
        HasAnswer = true;
        Handler?.Invoke(answer);
    }
}
