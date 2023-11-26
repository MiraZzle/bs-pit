using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;

public class EndSceneManager : MonoBehaviour {
    private TextMeshProUGUI nameCandidate;
    private TextMeshProUGUI textContinue;

    private float waitingTimeVideo = 4f;

    [SerializeField] private TextMeshProUGUI textUp;
    [SerializeField] private TextMeshProUGUI textMid;
    [SerializeField] private TextMeshProUGUI textDown;
    [SerializeField] private TextMeshProUGUI textTitle;

    public VideoPlayer player;
    public Canvas canvas;

    void Start() {
        GameObject nameObj = GameObject.Find("nameCandidate");
        nameCandidate = nameObj.GetComponent<TextMeshProUGUI>();

        GameObject contObj = GameObject.Find("textContinue");
        textContinue = contObj.GetComponent<TextMeshProUGUI>();

        canvas.enabled = false;
        player.playOnAwake = true;

        player.Prepare();
        player.Play();
        StartCoroutine(waitForVideoPause());

        textContinue.enabled = false;

        DrawImage();
    }

    void Update() {
        if (Input.GetKeyDown("space"))
            SceneManager.LoadScene("StartScene");
        
        Invoke("enableContinue", 5f);
    }



    IEnumerator waitForVideoPause() {
        yield return new WaitForSeconds(waitingTimeVideo);  // ceka 4 sekundy
        player.Pause();
        player.enabled = false;
        canvas.enabled = true;
    }

    private void enableContinue() {
        textContinue.enabled = true;
    }
    
    private void DrawImage() {
        string language = PlayerPrefs.GetString("language");
        string name = PlayerPrefs.GetString("name");
        int autenticita = PlayerPrefs.GetInt("autenticita");
        int volici = PlayerPrefs.GetInt("volici");
 
        nameCandidate.text = name.ToUpper();

        if ((autenticita >= 50) && (volici >= 50)) {
            if (language == "english") {
                textUp.text = "The battle of the two finalists, the last two candidates vying for the Callibristan presidency, is over.";
                textMid.text = "The election ended with a surprising and unexpected result. The unfavoured candidate, " + name + ", won against all odds! He fought all the slander, insults, misinformation and general populism he had to face. Unlike his opponent, who was running for head of state for the second time, he ran a decent and honest campaign.";
                textDown.text = "The election of " + name + " means that values such as mutual respect and tolerance still carry weight in our society.";
                textTitle.text = ("Duel: elections " + Random.Range(2020, 2100)).ToUpper();
            } else {
                textUp.text = "Souboj dvou finalistů, posledních dvou kandidátů ucházejících se o post prezidenta Callibristánu je u konce. ";
                textMid.text = "Volby skončily překvapivým a nečekaným výsledkem. Nefavorizovaný kandidát, " + name + ", totiž navzdory všem odhadům zvítězil! Popasoval se se všemi pomluvami, urážkami, dezinformacemi a obecně populismem, kterému musel čelit. Narozdíl od svého oponenta, který se o hlavu státu ucházel podruhé, vedl kampaň slušně a čestně.";
                textDown.text = "Jeho zvolení ukazuje, že v naší společnosti hodnoty jako jsou vzájemný respekt a tolerance, stále mají svou váhu.";
                textTitle.text = ("Duel: volby " + Random.Range(2020, 2100)).ToUpper();
            }
        } else if ((autenticita >= 50) && (volici < 50)) {
            if (language == "english") {
                textUp.text = "The battle of the two finalists, the last two candidates vying for the Callibristan presidency, is over.";
                textMid.text = "The election ended with a surprising result. The favoured candidate, " + name + ", lost against all odds. He is a man who won the hearts of the younger part of society with his appearance and opinions. He fought the battle for the seat honestly, representatively, without insults or lies, and thus his influence and reach spread to other social strata.";
                textDown.text = "In spite of all his qualities, he found himself in a situation where he had to cope with populist pressure, which unfortunately he did not manage.";
                textTitle.text = ("Duel: elections " + Random.Range(2020, 2100)).ToUpper();
            } else {
                textUp.text = "Souboj dvou finalistů, posledních dvou kandidátů ucházejících se o post prezidenta Callibristánu je u konce. ";
                textMid.text = "Volby skončily překvapivým výsledkem. Favorizovaný kandidát, " + name + ", totiž navzdory všem odhadům prohrál. Je to člověk, který si svým vystupováním a názory získal srdce predevším v mladší části společnost. Boj o křeslo vedl čestně, reprezentativně, bez urážek, lhaní a tím se jeho vliv a pole působnosti šířilo i mezi ostatní společenské vrstvy.";
                textDown.text = "I přes všechny jeho kvality, se dostal do situace, kde se musel vyrovnat s populistickým tlakem, což bohužel nezvládl.";
                textTitle.text = ("Duel: volby " + Random.Range(2020, 2100)).ToUpper();
            }
        } else if ((autenticita < 50) && (volici >= 50)) {
            if (language == "english") {
                textUp.text = "A dramatic story has unfolded in the political contest for the presidency, which ended with the victory of the populist " + name +".";
                textMid.text = "His path to triumph has been marked by the systematic dissemination of misinformation, the cultivation of hatred and the incitement of fear. This strategy, despite its controversial nature, has proved effective in a polarised political environment. " + name + " thus gained the support of those who were swayed by emotions and unverified information.";
                textDown.text = "His victory now resonates positively or negatively throughout society, with some seeing hope in his triumph while others worry about the future of democracy.";
                textTitle.text = ("Duel: elections " + Random.Range(2020, 2100)).ToUpper();
            } else {
                textUp.text = "V politickém souboji o prezidentský úřad se odehrál dramatický příběh, který skončil vítězstvím populisty " + name + ".";
                textMid.text = "Jeho cesta k triumfu byla poznamenána systematickým šířením dezinformací, kultivací nenávisti a vyvoláváním strachu. Tato strategie, i přes její kontroverznost, se ukázala být účinnou v polarizovaném politickém prostředí. " + name + " tak získal podporu těch, kteří se nechali ovlivnit emocemi a neověřenými informacemi. Jeho vítězství nyní kladně či negativně ";
                textDown.text = "rezonuje v celé společnosti, přičemž někteří vidí v jeho triumfu naději, zatímco jiní mají obavy o budoucnost demokracie.";
                textTitle.text = ("Duel: volby " + Random.Range(2020, 2100)).ToUpper();
            }
        } else {
            if (language == "english") {
                textUp.text = "The battle for the post of president of the republic was an intense contest, but the populist " + name + " did not win in the end.";
                textMid.text = "Despite his efforts to spread lies, disinformation, incite hatred and create an atmosphere of fear, the electorate has shown resistance to this strategy. The clear rejection of " +name+ " in the presidential election indicates that society is resisting the methods of manipulation and is looking for other value-based approaches in politics. \"I will not give up so easily!\" he told a reporter in an interview.";
                textDown.text = "This outcome may have important implications for the future of political culture and dialogue in the country.";
                textTitle.text = ("Duel: elections " + Random.Range(2020, 2100)).ToUpper();
            } else {
                textUp.text = "V boji o post prezidenta republiky se odehrál intenzivní souboj, ve kterém však populista " + name + " nakonec nezvítězil.";
                textMid.text = "I přes jeho snahu šířit lži, dezinformace, podněcovat nenávist a vytvářet atmosféru plnou strachu, voliči ukázali odpor vůči této strategii. Jeho jasné odmítnutí v prezidentských volbách naznačuje, že společnost se staví proti metodám manipulace a hledá jiné hodnotové přístupy v politice. \"Tak snadno se nevzdám!\", řekl  reportérce v rozhovoru.";
                textDown.text = "Tento výsledek může mít významné důsledky pro budoucnost politické kultury a dialogu v zemi.";
                textTitle.text = ("Duel: volby " + Random.Range(2020, 2100)).ToUpper();
            }
        }
    }
}
