using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AnswersManager : MonoBehaviour
{
    public GameObject AnswerPrefab;
    public GameObject Parent;

    public Action<Answer> Handler;

    private List<Answer> _answers = new List<Answer>();
    private VerticalLayoutGroup _alignment;

    // Start is called before the first frame update
    void Start() { _alignment = GetComponent<VerticalLayoutGroup>(); }

    public void Show() { Parent.SetActive(true); }
    public void Hide() { Parent.SetActive(false); }

    public void SetAnswers(List<string> answers)
    {
        ClearAnswers();
        for (int i = 0; i < answers.Count; i++)
        {
            _answers.Add(CreateAnswer(answers[i], i));
        }
    }

    private void ClearAnswers()
    {
        foreach (var answer in _answers)
        {
            Destroy(answer);
        }
    }

    private Answer CreateAnswer(string text, int id)
    {
        GameObject gameobj = Instantiate(AnswerPrefab, Parent.transform);
        Answer answer = gameobj.GetComponent<Answer>();

        answer.SetText(text);
        answer.AnswerID = id;
        answer.GetComponent<Button>().onClick.AddListener(() => Handler?.Invoke(answer));

        return answer;
    }
}
