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

    [SerializeField]
    ScaleBarManager votingBar;

    public int PlayerAuthenticity => Player.Authenticity;
    public int EnemyAuthenticity => Enemy.Authenticity;
    public int PlayerVoters { get; private set; } = 50;

    private void ChangePlayerVoters(int deltaVolici)
    {
        PlayerVoters += deltaVolici;
        PlayerVoters = Mathf.Clamp(PlayerVoters, 0, 100);
        votingBar.UpdateSlider(PlayerVoters);
    }
    private void ChangeEnemyVoters(int deltaVolici)
    {
        PlayerVoters -= deltaVolici;
        PlayerVoters = Mathf.Clamp(PlayerVoters, 0, 100);
        votingBar.UpdateSlider(PlayerVoters);
    }

    [SerializeField]
    private GameObject _playerObject;
    public Candidate Player;

    [SerializeField]
    private GameObject _enemyObject;
    public Candidate Enemy;

    void Start()
    {
        Player = _playerObject.GetComponent<Candidate>();
        Enemy = _enemyObject.GetComponent<Candidate>();
        PlayerPrefs.SetString("name", Player.Name);
    }



    public void SetUpQuestions()
    {
        // TOHLE SE NEMUZE VOLAT VE STARTU, PAK JE NULL REFERENCE EXEPTION

        _generalQuestions = QuestionLoader.GetRandomQuestions();
        _playerQuestions = QuestionLoader.GetQuestionsForCandidate(Player);
        _enemyQuestions = QuestionLoader.GetQuestionsForCandidate(Enemy);
        _questions = new (Question, Candidate)[] { // round 1
                                                  (_generalQuestions[0], Player), (_generalQuestions[0], Enemy),

                                                  (_playerQuestions[0], Player), (_enemyQuestions[0], Enemy),
                                                  // round2
                                                  (_generalQuestions[1], Player), (_generalQuestions[1], Enemy),

                                                  (_playerQuestions[1], Player), (_enemyQuestions[1], Enemy),
                                                  // round 3
                                                  (_generalQuestions[2], Player), (_generalQuestions[2], Enemy),

                                                  (_playerQuestions[2], Player), (_enemyQuestions[2], Enemy),

                                                  // last question
                                                  (_generalQuestions[3], Player), (_generalQuestions[3], Enemy)
        };
        _questionsInTotal = _questions.Length;
    }

    bool _ready = false;
    public void Update()
    {
        if (!_ready)
        {
            SetUpQuestions();
            _ready = true;
        }
    }

    private Question _lastQuestion;
    private Candidate _lastCandidate;
    private Answer _lastAnswer;

    public (Question?, Candidate?) AskAnotherQuestion()
    {
        if (_questionNum >= _questionsInTotal)
            return (null, null);

        (_lastQuestion, _lastCandidate) = _questions[_questionNum++];
        return (_lastQuestion, _lastCandidate);
    }

    public void ProcessAnswer(Answer answer)
    {
        _lastCandidate.ChangeAuthenticity(answer.DeltaAuthenticity);

        if (_lastCandidate == Player)
            ChangePlayerVoters(answer.DeltaVolici);
        else
            ChangeEnemyVoters(answer.DeltaVolici);
        _lastAnswer = answer;
    }

    public int numCardsUsed = 0;
    public void ProcessCardAttack(Card card)
    {
        ++numCardsUsed;
        // if the player attacked, than the last question must have been for the enemy

        bool DecideWin(float multiplier)
        {
            float r = Random.Range(0f, 1f);
            return PlayerAuthenticity / Candidate.MaxAuthenticity * multiplier > r;
        }
        int CalculateResult(int number, float multiplier)
        {
            float result = number * multiplier;
            float roundedResult = (result > 0) ? Mathf.Ceil(result) : Mathf.Floor(result);
            return (int)roundedResult;
        }

        float probabilityMultiplier = 1f;
        float powerMultiplier = 1f;

        void SetProbabilityAndMultiplier()
        {
            // general question
            if (_lastQuestion.Type == QuestionType.General)
            {
                probabilityMultiplier = 1f;
                powerMultiplier = 0.65f;
                return;
            }
            // personal question - irrelevant
            if (!card.IsRelevantToProperty((PropertyType)_lastQuestion.AssociatedProperty!))
            {
                probabilityMultiplier = 0.5f;
                powerMultiplier = 1f;
                return;
            }

            // personal question - relevant
            switch (_lastAnswer.Type)
            {
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
        if (playerWon)
        {
            Enemy.ChangeAuthenticity(loserDeltaAuth);
            ChangePlayerVoters(winnerDeltaVolici);
        }
        else
        {
            Player.ChangeAuthenticity(loserDeltaAuth);
            ChangeEnemyVoters(winnerDeltaVolici);
        }
    }
}
