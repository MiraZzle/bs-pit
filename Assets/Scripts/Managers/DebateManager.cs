using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable





public class DebateManager : MonoBehaviour
{
    private Question[] _generalQuestions;
    private Question[] _playerQuestions;
    private Question[] _enemyQuestions;
    private (Question, Candidate)[] _questions;

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
        PlayerPrefs.SetString("name", _player.Name);
    }

    public void SetUpQuestions() {
        // TOHLE SE NEMUZE VOLAT VE STARTU, PAK JE NULL REFERENCE EXEPTION

        _generalQuestions = QuestionLoader.GetRandomQuestions();
        _playerQuestions = QuestionLoader.GetQuestionsForCandidate(_player);
        _enemyQuestions = QuestionLoader.GetQuestionsForCandidate(_enemy);
        _questions = new (Question, Candidate)[] {
            // round 1
            (_generalQuestions[0], _player),
            (_generalQuestions[0], _enemy),

            (_playerQuestions[0], _player),
            (_enemyQuestions[0], _enemy),
            // round2
            (_generalQuestions[1], _player),
            (_generalQuestions[1], _enemy),

            (_playerQuestions[1], _player),
            (_enemyQuestions[1], _enemy),
            // round 3
            (_generalQuestions[2], _player),
            (_generalQuestions[2], _enemy),

            (_playerQuestions[2], _player),
            (_enemyQuestions[2], _enemy),

            // last question
            (_generalQuestions[3], _player),
            (_generalQuestions[3], _enemy)
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
    private Answer _lastAnswer;

    public (Question?, Candidate?) AskAnotherQuestion() {
        if (_questionNum >= _questionsInTotal) return (null, null);

        (_lastQuestion, _lastCandidate) = _questions[_questionNum++];
        return (_lastQuestion, _lastCandidate);
    }

    public void ProcessAnswer(Answer answer) {
        _lastCandidate.ChangeAuthenticity(answer.DeltaAuthenticity);

        if (_lastCandidate == _player) 
            ChangePlayerVoters(answer.DeltaVolici);
        else 
            ChangeEnemyVoters(answer.DeltaVolici);
        _lastAnswer = answer;
    }

    public void ProcessCardAttack(Card card) {
        // if the player attacked, than the last question must have been for the enemy

        bool DecideWin(float multiplier) {
            float r = Random.Range(0f, 1f);
            return PlayerAuthenticity/Candidate.MaxAuthenticity * multiplier > r;
        }
        int CalculateResult(int number, float multiplier) {
            float result = number * multiplier;
            float roundedResult = (result > 0) ? Mathf.Ceil(result) : Mathf.Floor(result);
            return (int)roundedResult;
        }

        float probabilityMultiplier = 1f;
        float powerMultiplier = 1f;

        void SetProbabilityAndMultiplier() {
            // general question
            if (_lastQuestion.Type == QuestionType.General) {
                probabilityMultiplier = 1f;
                powerMultiplier = 0.65f;
                return;
            }
            // personal question - irrelevant
            if (!card.IsRelevantToProperty((PropertyType)_lastQuestion.AssociatedProperty!)) {
                probabilityMultiplier = 0.5f;
                powerMultiplier = 1f;
                return;
            }

            // personal question - relevant
            switch (_lastAnswer.Type) {
                case AnswerType.Populist:
                    probabilityMultiplier = 1f;
                    powerMultiplier = 1.5f;
                    break;
                case AnswerType.Neutral:
                    probabilityMultiplier = 1f;
                    powerMultiplier = 1f;
                    break;
                case AnswerType.Real:
                    probabilityMultiplier = 3f;
                    powerMultiplier = 0.5f;
                    break;
                default:
                    break;
            }
        }

        SetProbabilityAndMultiplier();

        bool playerWon = DecideWin(probabilityMultiplier);
        int loserDeltaAuth = CalculateResult(card.LoserAuthenticityDelta, powerMultiplier);
        int winnerDeltaVolici = CalculateResult(card.WinnerVoliciDelta, powerMultiplier);
        if (playerWon) {
            _enemy.ChangeAuthenticity(loserDeltaAuth);
            ChangePlayerVoters(winnerDeltaVolici);
        }
        else {
            _player.ChangeAuthenticity(loserDeltaAuth);
            ChangeEnemyVoters(winnerDeltaVolici);
        }
    }
}
