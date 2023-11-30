using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AnswersManager : MonoBehaviour
{
    public GameObject AnswerPrefab;
    public GameObject Parent;

    public Action<Answer> Handler;

    private List<AnswerButton> _answers = new List<AnswerButton>();
    private SoundManager soundManager;

    // Start is called before the first frame update
    void Start() { 
        soundManager = GameObject.FindGameObjectWithTag("sound").GetComponent<SoundManager>();
    }

    public void Show() { Parent.SetActive(true); }
    public void Hide() { Parent.SetActive(false); }

    public void SetAnswers(List<Answer> answers)
    {
        ClearAnswers();
        for (int i = 0; i < answers.Count; i++)
        {
            _answers.Add(CreateAnswer(answers[i]));
        }
    }

    public void ClearAnswers()
    {
        foreach (var answer in _answers)
        {
            Destroy(answer.gameObject);
        }

        _answers.Clear();
    }

    private AnswerButton CreateAnswer(Answer ans)
    {
        GameObject gameobj = Instantiate(AnswerPrefab, Parent.transform);
        AnswerButton answer = gameobj.GetComponent<AnswerButton>();

        answer.SetText(ans.Text);
        answer.Ans = ans;
        answer.GetComponent<Button>().onClick.AddListener(() =>
                                                          {
                                                              soundManager.PlayMouseClickSE();
                                                              Handler?.Invoke(ans);
                                                          });

        return answer;
    }
}
