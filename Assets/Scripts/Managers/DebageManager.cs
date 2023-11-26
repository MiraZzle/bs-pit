using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

public class DebageManager : MonoBehaviour
{
    private Question[] _generalQuestions;
    private Question[] _playerQuestions;
    private Question[] _enemyQuestions;
    private Question[] _questions;

    private int _questionNum = 0;
    private int _questionsInTotal;

    public int PlayerAuthenticity => _player.Authenticity;
    public int EnemyAuthenticity => _enemy.Authenticity;
    public int PlayerVoters { get; private set; } = 50;

    private void ChangePlayerVoters(int deltaVolici) {
        PlayerVoters += deltaVolici;
        PlayerVoters = Mathf.Clamp(PlayerVoters, 0, 100);
    }
    private void ChangeEnemyVoters(int deltaVolici) {
        PlayerVoters -= deltaVolici;
        PlayerVoters = Mathf.Clamp(PlayerVoters, 0, 100);
    }

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
        _questionsInTotal = _questions.Length;
    }

    
    bool _ready = false;
    public void Update() {
        if (!_ready) {
            SetUpQuestions();
            _ready = true;
        }
    }


    private Question _lastQuestion;
    private Candidate _lastCandidate;

    public Question? AskAnotherQuestion() {
        if (_questionNum >= _questionsInTotal) return null;

        _lastQuestion = _questions[_questionNum++];
        return _lastQuestion;
    }

    public void ProcessAnswer(Answer answer) {
        _lastCandidate.ChangeAuthenticity(answer.DeltaAuthenticity);

        if (_lastCandidate == _player) 
            ChangePlayerVoters(answer.DeltaVolici);
        else 
            ChangeEnemyVoters(answer.DeltaVolici);

    }

    public void ProcessCardAttack(Card card) {
        // if the player attacked, than the last question must have been for the enemy

        int deltaAuth = 0;
        int deltaVolici = 0;
        bool playerWon = false;

        // general question


        // personal question - irrelevant

        // personal question - relevant
        // 1) enemy populist answer

        // 2) enemy neutral answer

        // 3) enemy real answer


        if (playerWon) {
            _enemy.ChangeAuthenticity(deltaAuth);
        }
        else {

        }

    }
}
