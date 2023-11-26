using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AnswersManager : MonoBehaviour
{
    public GameObject AnswerPrefab;
    public GameObject Parent;

    public SoundManager ButtonSoundManager;
    public AudioClip Clip;

    public Action<Answer> Handler;

    private List<AnswerButton> _answers = new List<AnswerButton>();
    private VerticalLayoutGroup _alignment;

    // Start is called before the first frame update
    void Start() { _alignment = GetComponent<VerticalLayoutGroup>(); }

    public void Show() { Parent.SetActive(true); }
    public void Hide() { Parent.SetActive(false); }

    public void SetAnswers(Answer[] answers)
    {
        ClearAnswers();
        for (int i = 0; i < answers.Length; i++)
        {
            _answers.Add(CreateAnswer(answers[i]));
        }
    }

    public void ClearAnswers()
    {
        foreach (var answer in _answers)
        {
            Destroy(answer);
        }
    }

    public void DestroyExcept(Answer answer)
    {
        foreach (var ans in _answers)
        {
            if (ans.Ans.Text != answer.Text)
            {
                Destroy(ans.gameObject);
            }
            else
            {
                var but = ans.GetComponent<Button>();
                but.interactable = false;
            }
        }
    }

    private AnswerButton CreateAnswer(Answer ans)
    {
        GameObject gameobj = Instantiate(AnswerPrefab, Parent.transform);
        AnswerButton answer = gameobj.GetComponent<AnswerButton>();

        answer.SetText(ans.Text);
        answer.Ans = ans;
        answer.GetComponent<Button>().onClick.AddListener(() =>
                                                          {
                                                              ButtonSoundManager.PlaySound(Clip);
                                                              Handler?.Invoke(ans);
                                                          });

        return answer;
    }
}
