using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebageManager : MonoBehaviour
{
    private Question[] _generalQuestions;
    private Question[] _playerQuestions;
    private Question[] _enemyQuestions;
    private Question[] _questions;

    private int _questionNum = 0;

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

    public void SetUpQuestions() {
        // TOHLE SE NEMUZE VOLAT VE STARTU, PAK JE NULL REFERENCE EXEPTION

        _generalQuestions = QuestionLoader.GetRandomQuestions();
        _playerQuestions = QuestionLoader.GetQuestionsForCandidate(_player);
        _enemyQuestions = QuestionLoader.GetQuestionsForCandidate(_enemy);
        _questions = new Question[] {
            _generalQuestions[0],
            _playerQuestions[0],
            _enemyQuestions[0],

            _generalQuestions[1],
            _playerQuestions[1],
            _enemyQuestions[1],
            
            _generalQuestions[2],
            _playerQuestions[2],
            _enemyQuestions[2],
            
            _generalQuestions[3],
        };
    }

    /*
    bool ready = false;
    public void Update() {
        if (!ready) {
            SetUpQuestions();
            ready = true;
        }
    }
    */

    public void AskAnotherQuestion() {
        
    }
}
