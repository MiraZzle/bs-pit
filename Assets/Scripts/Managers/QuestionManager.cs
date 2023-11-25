using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    public Dialog Question;
    public AnswersManager AnswersManager;
    public Action<AnswerButton> HandleAnswers
    {
        get => AnswersManager.Handler;
        set => AnswersManager.Handler = value;
    }

    // Start is called before the first frame update
    void Start() { AnswersManager.Handler = HandleAnswers; }

    public void ShowQuestion(Action onAnimationEnd)
    {
        Question.OnTypewriterEnd += onAnimationEnd;
        Question.Show();
    }
    public void HideQuestion() { Question.Hide(); }

    public void ShowAnswers() { AnswersManager.Show(); }
    public void HideAnswers() { AnswersManager.Hide(); }

    public void SetAnswers(List<string> answers) { AnswersManager.SetAnswers(answers); }
    public void SetQuestion(string question) { Question.SetText(question); }
}
