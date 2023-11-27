using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable



public class DebateManager : MonoBehaviour {
    private Question[] _generalQuestions;
    private Question[] _playerQuestions;
    private Question[] _enemyQuestions;
    private (Question, Candidate)[] _questions;

    private int _questionNum = 0;
    private int _questionsInTotal;

    string _language;


    [SerializeField]
    private ScaleBarManager _votingBar;

    private void ShowVotingBar() {
        _votingBar.gameObject.SetActive(true);
    }
    private void HideVotingBar() {
        _votingBar.gameObject.SetActive(false);
    }

    public int PlayerAuthenticity => Player.Authenticity;
    public int EnemyAuthenticity => Enemy.Authenticity;
    public int PlayerVoters { get; private set; } = 50;

    private void ChangePlayerVoters(int deltaVolici) {
        PlayerVoters += deltaVolici;
        PlayerVoters = Mathf.Clamp(PlayerVoters, 0, 100);
        _votingBar.UpdateSlider(PlayerVoters);
    }
    private void ChangeEnemyVoters(int deltaVolici) {
        PlayerVoters -= deltaVolici;
        PlayerVoters = Mathf.Clamp(PlayerVoters, 0, 100);
        _votingBar.UpdateSlider(PlayerVoters);
    }

    [SerializeField]
    private GameObject _playerObject;
    public Candidate Player { get; private set; }

    [SerializeField]
    private GameObject _enemyObject;
    public Candidate Enemy { get; private set; }

    void Start() {
        Player = _playerObject.GetComponent<Candidate>();
        Enemy = _enemyObject.GetComponent<Candidate>();
        PlayerPrefs.SetString("name", Player.Name);
        _language = PlayerPrefs.GetString("language"); 
        HideVotingBar();
    }

    public void ShowBars() {
        ShowVotingBar();
        Player.ShowAuthenticityBar();
        Enemy.ShowAuthenticityBar();
    }


    public void SetUpQuestions() {
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
        if (_questionNum >= _questionsInTotal)
            return (null, null);

        (_lastQuestion, _lastCandidate) = _questions[_questionNum++];
        return (_lastQuestion, _lastCandidate);
    }

     public string GetIntroText() {
        string introCS = "Dámy a pánové, vítejte u prezidentské debaty, která je klíčovým okamžikem v historii našeho národa. Dnes večer představí kandidáti " + Player.Name + " a " + Enemy.Name + " různé vize naší budoucnosti. Děkujeme vám, že jste se rozhodli státi svědky tohoto zásadního rozhovoru.";
        string introEN = "Ladies and gentlemen, welcome to the presidential debate, a pivotal moment in our nation's journey. Tonight, our candidates " + Player.Name + " and " + Enemy.Name + " present diverse visions for our future. Thank you for joining this critical conversation.";
        return (_language == "english") ? introEN : introCS;
    }

    public string GetPlayerIntroText() {
        string introEN = Player.Name + ", a seasoned politician, has more political baggage than a 10-term senator at an airport carousel. Critics say they navigate issues with all the agility of a sloth in a speed-eating contest.";
        string introCS = Player.Name + ", seriózní kandidát s vtipným odstupem k politice. Vypadá, jako by každou chvíli přednášel důležitou tezi. Jeho oblíbeným heslem je: 'Rozhodnutí je jako dobrý vtip - potřebuje čas a správnou pointu.'";
        return (_language == "english") ? introEN : introCS;
    }

    public string GetEnemyIntroText() {
        string introCS = Enemy.Name + ", charismatický kandidát schopný prodat lednici Eskymákovi. Jeho kampaň je plná energie a humoru a jeho politické návrhy mají tendenci obsahovat smích, ale občas je těžké rozeznat, zda chce zlepšit stát nebo natočit sitcom.";
        string introEN = Enemy.Name + ", a private sector enthusiast, brings as much political experience as a goldfish in a game of chess – but hey, who needs political know-how when you've got a dynamic PowerPoint presentation?";
        return (_language == "english") ? introEN : introCS;
    }

    public string GetStartQuestionsIntroText() {
        string introEN = "Well, let's stop stalling and get down to what everyone is waiting for - the questions.";
        string introCS = "Přestaňme otálet a přejděme k tomu, na co všichni čekají - k otázkám.";
        return (_language == "english") ? introEN : introCS;
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
            return (float)PlayerAuthenticity / (float)Candidate.MaxAuthenticity * multiplier > r;
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
