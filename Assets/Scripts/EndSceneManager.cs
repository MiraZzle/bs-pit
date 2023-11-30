using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;

public class EndSceneManager : MonoBehaviour {
    private TextMeshProUGUI nameCandidate;
    private Image headCandidate;
    private const float _waitingTimeVideo = 5f;   // if this is change then the fade in animation has to be changed as well

    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Canvas newspaper;

    [SerializeField] private TextMeshProUGUI textUp;
    [SerializeField] private TextMeshProUGUI textMid;
    [SerializeField] private TextMeshProUGUI textDown;
    [SerializeField] private TextMeshProUGUI textTitle;

    [SerializeField] private Sprite playerHead;
    [SerializeField] private Sprite enemyHead;

    [SerializeField] private GameObject redCross;
    [SerializeField]
    TMP_Text pressSpaceToContinue;

    string _language;
    bool _canContinue;

    void Start() {
        GameObject nameObj = GameObject.Find("nameCandidate");
        nameCandidate = nameObj.GetComponent<TextMeshProUGUI>();

        GameObject headObj = GameObject.Find("headCandidate");
        headCandidate = headObj.GetComponent<Image>();

        _language = PlayerPrefs.GetString("language");

        newspaper.enabled = false;
        videoPlayer.playOnAwake = true;

        _canContinue = false;
        videoPlayer.Prepare();
        videoPlayer.Play();
        StartCoroutine(waitForVideoPause());

        DrawImage();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && _canContinue) {
            SceneManager.LoadScene("StartScene");
        }
    }



    IEnumerator waitForVideoPause() {
        yield return new WaitForSeconds(_waitingTimeVideo);  // wait for the video to play
        // stop the rest of the video and show the newspaper
        videoPlayer.Pause();
        videoPlayer.enabled = false;
        newspaper.enabled = true;

        // dont allow the player to skip the newspaper immediately
        yield return new WaitForSeconds(1.5f);
        _canContinue = true;
        pressSpaceToContinue.gameObject.SetActive(true);
    }

    
    private void DrawImage() {
        string playerName = PlayerPrefs.GetString("playerName");
        string enemyName = PlayerPrefs.GetString("enemyName");
        int playerAuthenticity = PlayerPrefs.GetInt("playerAuthenticity");
        int enemyAuthenticity = PlayerPrefs.GetInt("enemyAuthenticity");
        int minAuthenticity = PlayerPrefs.GetInt("minAuthenticity");
        int playerVoters = PlayerPrefs.GetInt("playerVoters");

        bool kickedOutEnding;
        string winnerName;
        string loserName;
        bool winnerPopulist = false;
        bool loserPopulist = false;
        
        Sprite head;

        void SetWinnerAndLoser() {
            // player was kicked out
            if (playerAuthenticity < minAuthenticity) {
                kickedOutEnding = true;
                // player was kicked out
                loserName = playerName;
                winnerName = enemyName;
                head = playerHead;
                return;
            }
            // enemy was kicked out
            if (enemyAuthenticity < minAuthenticity) {
                kickedOutEnding = true;
                // enemy was kicked out
                loserName = enemyName;
                winnerName = playerName;
                head = enemyHead;
                return;
            }
            // normal ending
            // same voters --> decide by authenticity
            if (playerVoters == 50) {
                winnerName = (playerAuthenticity >= enemyAuthenticity) ? playerName : enemyName;
            }
            else {
                winnerName = (playerVoters > 50) ? playerName : enemyName;
            }
            kickedOutEnding = false;
            loserName = (winnerName == playerName) ? enemyName : playerName;
            int winnerAuthenticity = (winnerName == playerName) ? playerAuthenticity : enemyAuthenticity;
            int loserAuthenticity = (winnerName == playerName) ? enemyAuthenticity : playerAuthenticity;
            winnerPopulist = (winnerAuthenticity < 50);
            loserPopulist = (loserAuthenticity < 50);

            head = (winnerName == playerName) ? playerHead : enemyHead;
        }

        SetWinnerAndLoser();
        headCandidate.sprite = head;

        // set title
        SetTitle();

        // kicked out ending
        if (kickedOutEnding) {
            nameCandidate.text = loserName.ToUpper();
            redCross.SetActive(true);
            SetKickedOutText(loserName, winnerName);
            return;
        }

        // normal ending
        nameCandidate.text = winnerName.ToUpper();
        redCross.SetActive(false);
        SetNormalEndingText(winnerName, loserName, winnerPopulist, loserPopulist);
    }

    private void SetTitle() {
        string titleTextEN = "Duel: elections " + Random.Range(2023, 2090);
        string titleTextCS = "Duel: volby " + Random.Range(2023, 2090);
        textTitle.text = (_language == "english") ? titleTextEN.ToUpper() : titleTextCS.ToUpper();
    }

    private void SetKickedOutText(string kickedOutName, string otherName) {
        string kickLastName = kickedOutName.Split(' ')[1];
        string otherFirstName = otherName.Split(' ')[0];
        string otherLastName = otherName.Split(' ')[1];
        string kickLastNameCS = kickLastName[..^1] + "ého";  // Zlobivý --> Zlobivého
        string otherNameCs = otherFirstName + "u " + otherLastName[..^1] + "ému";  // Kazisvětu Zlobivému

        string textUpCS = "Závěrečné kolo prezidentské debaty bylo nečekaně ukončeno, když byl kandidát " + kickedOutName + " vyloučen z debaty kvůli opakovaným nepravdivým a populistickým výrokům.";
        string textMidCS = kickLastNameCS + " neúcta k pravidlům diskuze vedla organizátory k rozhodujícímu kroku, pro zachování integrity debaty. Vzhledem k tomu, že na pódiu byli pouze dva kandidáti, vyloučení " + kickLastNameCS + " ponechalo " + otherNameCs + " možnost debatu uzavřít sólo. Tento incident poukazuje na důležitost zachování pravdivosti v politickém diskurzu a ponechává voličům zásadní rozhodnutí na základě argumentů zbývajícího kandidáta.";
        string textDownCS = "Tento incident znovu rozvířil diskusi o důležitosti ověřování faktů a zodpovědnosti v politických debatách. Tím vyvolal výzvy k větší transparentnosti a kontrole výroků politiků.";
        
        string textUpEN = "In an unprecedented turn of events, the final round of the presidential debate came to an abrupt end as candidate " + kickedOutName + " faced exclusion for repeatedly making false and populistic statements.";
        string textMidEN = kickLastName + "'s disrespect for the rules of the debate led the organizers to take a decisive step to preserve the integrity of the debate.. With only two candidates on the stage, the exclusion of " + kickLastName + " left " + otherName + " to conclude the debate solo. The incident highlights the importance of maintaining truthfulness in political discourse, leaving voters with a crucial decision based on the merits of the other candidates's arguments.";
        string textDownEN = "The incident has reignited discussions on the role of fact-checking and accountability in political debates, prompting calls for increased transparency and scrutiny of politicians' statements.";

        textUp.text   = (_language == "english") ? textUpEN   : textUpCS;
        textMid.text  = (_language == "english") ? textMidEN  : textMidCS;
        textDown.text = (_language == "english") ? textDownEN : textDownCS;
    }

    private void SetNormalEndingText(string winnerName, string loserName, bool winnerPopulist, bool loserPopulist) {
        string textUpCS = "";
        string textUpEN = "";
        string textMidCS = "";
        string textMidEN = "";
        string textDownCS = "";
        string textDownEN = "";

        string winnerLastName = winnerName.Split(' ')[1];
        string loserLastName = loserName.Split(' ')[1];
        string loserLastNameCS = loserLastName[..^1] + "ého";  // Zlobivý --> Zlobivého
        string winnerLastNameCS = winnerLastName[..^1] + "ého";  // Zlobivý --> Zlobivého

        if (winnerPopulist && loserPopulist) {
            textUpCS = "Politický souboji o prezidentský úřad Callibristánu byl dramatický příběh, který skončil vítězstvím populisty " + winnerLastNameCS + ".";
            textMidCS = "Jeho cesta k triumfu byla poznamenána systematickým šířením dezinformací, kultivací nenávisti a vyvoláváním strachu. Tato strategie, i přes její kontroverznost, se ukázala být účinnou v polarizovaném politickém prostředí. " + winnerLastName + " tak získal podporu těch, kteří se nechali ovlivnit emocemi a neověřenými informacemi. „Tak snadno se nevzdám!“ řekl poražený kandidát " + loserName + " reportérce v rozhovoru.";
            textDownCS = "Vítězství " + winnerLastNameCS + " nyní ve společnosti rezonuje jak pozitivně, tak negativně, přičemž někteří v jeho triumfu vidí naději, zatímco jiní se obávají o budoucnost demokracie.";

            textUpEN = "The political contest for the presidency of Callibristan was a dramatic story that ended with the victory of the populist " + winnerLastName + ".";
            textMidEN = "His path to triumph has been marked by the systematic spread of disinformation, the cultivation of hatred and the incitement of fear. This strategy, despite its controversial nature, has proved effective in a polarised political environment. " + winnerLastName + " thus gained the support of those who were swayed by emotions and unverified information. “I'm not giving up that easily!” " + loserName + ", the defeated candidate, said in an interview with a journalist.";
            textDownEN = "The victory of " + winnerLastName + " now resonates both positively and negatively in society, with some seeing hope in his triumph while others fear for the future of democracy.";
        }
        if (winnerPopulist && !loserPopulist) {
            textUpCS = "Souboj dvou finalistů, posledních dvou kandidátů ucházejících se o post prezidenta Callibristánu je u konce.";
            textMidCS = "Volby skončily nečekaným výsledkem. Favorizovaný kandidát, " + loserName + ", totiž navzdory všem odhadům prohrál. Je to člověk, který si svým vystupováním a názory získal srdce predevším v mladší části společnost. Boj o křeslo vedl čestně, reprezentativně, bez urážek a bez lhaní. Volby nakonec vyhrál populista " + winnerName + ".";
            textDownCS = "Jeho vítězství nyní ve společnosti rezonuje jak pozitivně, tak negativně, přičemž někteří v jeho triumfu vidí naději, zatímco jiní se obávají o budoucnost demokracie.";

            textUpEN = "The battle of the two finalists, the last two candidates running for president of Callibristan, is over.";
            textMidEN = "The election ended with an unexpected result. The favored candidate, " + loserName + ", lost against all odds. He is a man who has won the hearts of the younger generation with his demeanour and views. He fought for the seat honestly, representatively, without insults and without lying. In the end, the election was won by the populist " + winnerName + ".";
            textDownEN = "His victory now resonates both positively and negatively in society, with some seeing hope in his triumph while others fear for the future of democracy.";
        }
        if (!winnerPopulist && loserPopulist) {
            textUpCS = "V boji o post prezidenta Callibristánu se odehrál intenzivní souboj, ve kterém však populista " + loserName + " nakonec nezvítězil.";
            textMidCS = "Navzdory jeho snaze šířit lži, dezinformace, podněcovat nenávist a vytvářet atmosféru plnou strachu, voliči ukázali odpor vůči této strategii. Jasné odmítnutí " + loserLastNameCS + " v prezidentských volbách naznačuje, že společnost se staví proti metodám manipulace a hledá jiné hodnotové přístupy v politice. „Tak snadno bych se nevzdal!“, řekl vítěz voleb " + winnerName + " reportérce v rozhovoru.";
            textDownCS = "Tento výsledek může mít významné důsledky pro budoucnost politické kultury a úrovně politického dialogu v zemi.";

            textUpEN = "The battle for the post of president of Callibristan was an intense contest, but the populist " + loserName + " did not win in the end.";
            textMidEN = "Despite his efforts to spread lies, disinformation, incite hatred and create an atmosphere of fear, the electorate has shown resistance to this strategy. The clear rejection of " + loserLastName + " in the presidential election indicates that society is resisting the methods of manipulation and is looking for other value-based approaches in politics. “I wouldn't give up so easily!” told the winner of the election " + winnerName + " a reporter in an interview.";
            textDownEN = "This outcome may have important implications for the future of political culture and the standard of political dialogue in the country.";
        }
        if (!winnerPopulist && !loserPopulist) {
            string winnerNameCS = winnerName.Split(' ')[0] + "a " + winnerLastNameCS;  // Mečislav Zlobivý --> Mečislava Zlobivého
            string loserNameCS = loserName.Split(' ')[0] + "em " + loserName + "m";    // Mečislav Zlobivý   --> Mečislavem Zlobivým

            textUpCS = "Souboj dvou finalistů, posledních dvou kandidátů ucházejících se o post prezidenta Callibristánu je u konce.";
            textMidCS = "Dnes skončily prezidentské volby vítězstvím favorita, " + winnerNameCS + ". Po souboji s " + loserNameCS + " získal " + winnerLastName + " silnou podporu voličů a slibuje jednotu a rozvoj země ve svém vedení. Jeho výhra přináší naději na prosperitu a sjednocenou společnost v Callibristánu. " + loserLastName + " přijal porážku s respektem. Oba kandidáti vedli kampaň slušně a s úctou ke svým voličům.";
            textDownCS = "Tyto volby ukázaly, že hodnoty jako vzájemný respekt a tolerance mají v naší společnosti stále svou váhu.";

            textUpEN = "The battle of the two finalists, the last two candidates running for president of Callibristan, is over.";
            textMidEN = "Today the presidential election has ended with the victory of the favorite, " + winnerName + ". After a duel with " + loserName + ", " + winnerLastName + " received strong support from the voters and promises unity and development of the country under his leadership. His victory brings hope for prosperity and a united society in Callibristan. " + loserLastName + " accepted his defeat with respect. Both candidates conducted their campaigns with civility and respect for their voters.";
            textDownEN = "This election has shown that values such as mutual respect and tolerance still have weight in our society.";
        }

        textUp.text = (_language == "english") ? textUpEN : textUpCS;
        textMid.text = (_language == "english") ? textMidEN : textMidCS;
        textDown.text = (_language == "english") ? textDownEN : textDownCS;
    }
}
