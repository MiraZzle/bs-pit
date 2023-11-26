using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebageManager : MonoBehaviour
{
    private Question[] _generalQuestions;
    private Question[] _playerQuestions;
    private Question[] _enemyQuestions;

    public int PlayerAuthenticity => _player.Authenticity;
    public int EnemyAuthenticity => _enemy.Authenticity;
    public int PlayerVoters { get; private set; } = 50;

    [SerializeField]
    private GameObject _playerObject;
    private Candidate _player;


    [SerializeField]
    private GameObject _enemyObject;
    private Candidate _enemy;


    void Start()
    {
        _player = _playerObject.GetComponent<Candidate>();
        _enemy = _enemyObject.GetComponent<Candidate>();
    }

    public void SetUpQuestion() {
        // TOHLE SE NEMUZE VOLAT VE STARTU, PAK JE NULL REFERENCE EXEPTION

        _generalQuestions = QuestionLoader.GetRandomQuestions();
        _playerQuestions = QuestionLoader.GetQuestionsForCandidate(_player);
        _enemyQuestions = QuestionLoader.GetQuestionsForCandidate(_enemy);

        Debug.Log(_playerQuestions[0].Text);
        Debug.Log(_enemyQuestions[0].Text);
        Debug.Log(_generalQuestions[0].Text);
    }

    public void AskAnotherQuestion() {

    }
}
