using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable



public class DebateManager : MonoBehaviour {
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
    public int MinAuthenticity => (int)(0.15f * Candidate.MaxAuthenticity);
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
        var _generalQuestions = QuestionLoader.GetRandomQuestions();
        var _playerQuestions = QuestionLoader.GetQuestionsForCandidate(Player);
        var _enemyQuestions = QuestionLoader.GetQuestionsForCandidate(Enemy);
        var _finalQuestion = QuestionLoader.GetFinalQuestion();
        _questions = new (Question, Candidate)[] {
            // round 1
            (_generalQuestions[0], Player), (_generalQuestions[0], Enemy),
            (_playerQuestions[0], Player), (_enemyQuestions[0], Enemy),
            // round2
            (_generalQuestions[1], Player), (_generalQuestions[1], Enemy),
            (_playerQuestions[1], Player), (_enemyQuestions[1], Enemy),
            // round 3
            (_generalQuestions[2], Player), (_generalQuestions[2], Enemy),
            (_playerQuestions[2], Player), (_enemyQuestions[2], Enemy),
            (_generalQuestions[3], Player), (_generalQuestions[3], Enemy),
            // final question 
            (_finalQuestion, Player), (_finalQuestion, Enemy)
        };
        _questionsInTotal = _questions.Length;
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
        string introCS = "Dámy a pánové, vítejte u prezidentské debaty, klíčového okamžiku v historii našeho národa. Dnes večer představí kandidáti " + Player.Name + " a " + Enemy.Name + " různé vize naší budoucnosti. Děkujeme vám, že jste se rozhodli státi svědky tohoto zásadního rozhovoru.";
        string introEN = "Ladies and gentlemen, welcome to the presidential debate, a pivotal moment in our nation's journey. Tonight, our candidates " + Player.Name + " and " + Enemy.Name + " present diverse visions for our future. Thank you for joining this critical conversation.";
        return (_language == "english") ? introEN : introCS;
    }

    public string GetPlayerIntroText() {
        string introCS = Player.Name + ", seriózní kandidát s vtipným odstupem k politice. Vypadá, jako by každou chvíli přednášel důležitou tezi. Jeho oblíbeným heslem je: 'Rozhodnutí je jako dobrý vtip - potřebuje čas a správnou pointu.'";
        string introEN = Player.Name + ", a seasoned politician, has more political baggage than a 10-term senator at an airport carousel. Critics say they navigate issues with all the agility of a sloth in a speed-eating contest.";
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

    public string GetKickOutOfDebateText(Candidate candidate) {
        string candidateLastName = candidate.Name.Split(' ')[1];

        string textEN = "Mr " + candidateLastName + ", in this debate we stress the importance of transparency and the truthful presentation of information. Unfortunately, because of your repeated false and extremely populist statements, we have to exclude you from the debate.";
        string textCS = "Pane " + candidateLastName + ", v rámci této debaty zdůrazňujeme důležitost transparentnosti a pravdivé prezentace informací. Kvůli opakovaným nepravdivým a extrémně populistickým prohlášením vás bohužel musíme vyjmout z debaty.";
        return (_language == "english") ? textEN : textCS;

    }

    public string GetOutroText(bool kickedOut) {
        string textNormalEN = "Ladies and gentlemen, that's all from today's presidential debate. I would like to thank both candidates for their participation and their openness in discussing key issues for our country. We hope that this debate has answered your questions. Thank you for watching.";
        string textNormalCS = "Dámy a pánové, to je pro dnešní prezidentskou debatu vše. Chtěl bych poděkovat oběma kandidátům za jejich účast a otevřenost v diskusi o klíčových otázkách pro naši zemi. Doufáme, že vám tato debata odpověděla na vaše otázky. Děkujeme vám za sledování a přejeme šťastnou volbu.";

        string textKickOutEN = "Ladies and gentlemen, due to persistent misinformation, we regretfully end this debate early to uphold the integrity of our democratic process. We apologize for any inconvenience and urge viewers to seek accurate information for informed decision-making. Thank you for your understanding.";
        string textKickOutCS = "Dámy a pánové, vzhledem k opakovaným dezinformacím bohužel musíme tuto rozpravu ukončit předčasně, abychom zachovali integritu demokratického hlasování. Omlouváme se za způsobené komplikace a děkujeme vám za pochopení.";

        if (kickedOut) return (_language == "english") ? textKickOutEN : textKickOutCS;
        else return (_language == "english") ? textNormalEN : textNormalCS;
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

    // dont show cards when all are used and when this is the final question 
    private int _numCardsUsed = 0;
    public bool ShouldShowCards => _numCardsUsed < CardManager.NumCards && _questionNum < _questionsInTotal - 1;


    public bool DecidePlayerWin(Card card) {
        return ProcessCardAttackLegacy(card);
    }
    public void UpdateAuthenticityAndVoters(Card card, bool playerWon) {
        ProcessCardAttackLegacy(card, playerWon);
        ++_numCardsUsed;
    }

    private bool ProcessCardAttackLegacy(Card card, bool? playerWon = null)
    {
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

        // the first time this is called (with null) this function return who won
        if (playerWon is null) {
            return DecideWin(probabilityMultiplier);
        }
        // it should be called again after that with the information of who won
        // and it will adjust stats accordingly

        int loserDeltaAuth = CalculateResult(card.LoserAuthenticityDelta, powerMultiplier);
        int winnerDeltaVolici = CalculateResult(card.WinnerVoliciDelta, powerMultiplier);
        if ((bool)playerWon)
        {
            Enemy.ChangeAuthenticity(loserDeltaAuth);
            ChangePlayerVoters(winnerDeltaVolici);
        }
        else
        {
            Player.ChangeAuthenticity(loserDeltaAuth);
            ChangeEnemyVoters(winnerDeltaVolici);
        }
        return (bool)playerWon;
    }
}
