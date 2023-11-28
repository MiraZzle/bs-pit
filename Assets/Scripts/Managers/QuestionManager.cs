using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    public Dialog QuestionDialog;
    public AnswersManager AnswersManager;

    public Action<Answer> Handler;
    public Answer Selected { get; private set; }
    public bool IsActive { get; private set; }
    public bool HasAnswer { get; private set; }

    void Start() { AnswersManager.Handler += HandleAnswer; }

    public void ShowQuestion(Question question, List<Answer> answers)
    {
        AnswersManager.SetAnswers(answers);
        Selected = null;
        QuestionDialog.SetText(question.Text);
        IsActive = true;
        HasAnswer = false;
        StartCoroutine(ShowAll());
    }

    public void SkipTyping() {
        QuestionDialog.Skip = true;
    }

    private IEnumerator ShowAll()
    {
        QuestionDialog.PlayText();
        yield return new WaitUntil(() => !QuestionDialog.IsActive);

        IsActive = false;
    }

    public void ShowAnswers() { AnswersManager.Show(); }

    public void HideQuestion()
    {
        QuestionDialog.Hide();
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
