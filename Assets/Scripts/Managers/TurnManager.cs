using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    [SerializeField]
    private QuestionManager questionManager;

    void Start() { ModeratorQuestion(); }

    void ModeratorQuestion()
    {
        questionManager.SetQuestion("Some random ...?");
        questionManager.SetAnswers(new List<string> { "A", "B", "C" });

        questionManager.ShowQuestion(() => questionManager.ShowAnswers());
    }
}
