using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Candidate : MonoBehaviour {
    [SerializeField]
    private Image authenticityBar;
    [SerializeField]
    private GameObject authenticityBarParent;


    public void ShowAuthenticityBar() {
        authenticityBarParent.SetActive(true);
    }
    public void HideAuthenticityBar() {
        authenticityBarParent.SetActive(false);
    }

    public Dialog DialogBox;

    public CharacterInfoCard InfoCard;

    private PropertiesManager propertiesManager;

    public Property[] GoodProperties { get; private set; }
    public Property[] BadProperties { get; private set; }
    public SpecialSkill SpecialSkill { get; private set; }

    public int Age { get; private set; }
    public string Name { get; private set; }
    public string Bio { get; private set; }

    public const int MaxAuthenticity = 100;

    public int Authenticity = MaxAuthenticity / 2;

    void Awake() { 
        propertiesManager = GameObject.FindGameObjectWithTag("logic").GetComponent<PropertiesManager>();
        UpdateAuthenticityBar();
        InfoCard.Hide();
    }

    void UpdateAuthenticityBar() {
        if (authenticityBar is not null) {
            authenticityBar.fillAmount = (float)Authenticity / (float)MaxAuthenticity;
        }
    }

    public void ChangeAuthenticity(int deltaAuthenticity) {
        Authenticity = Mathf.Clamp(Authenticity + deltaAuthenticity, 0, MaxAuthenticity);
        Debug.Log(Authenticity);

        //UpdateAuthenticityBar();
    }

    private static List<string> _usedStrings = new(); 
    public void SetRandomInfo()
    {
        string[] firstNamesCS
            = { "Kazisvět", "Honimír", "Horymír", "Spytihněv", "Kazimír", "Dobromír", "Mečislav" };

        string[] lastNamesCS
            = { "Dobrotivý", "Milosrdný", "Spravedlivý", "Laskavý", "Zlobivý", "Urputný" };

        string[] namesEN = {
            "Ben Dover",   "Whimsy McSnicker", "Barb Dwyer",
            "Hal Jalikee", "Jestin Jesterson", "Justin Time", "Drew Peacock",
        };

        int minAge = 30;
        int maxAge = 80;

        string[] biosCS = {
            "Expert na rychlé rozhodování a ještě rychlejší kafe. Slogan kampaně: „Jeden espresso, jedna země, jednoznačné rozhodnutí!“",
            "Bývalý mistr v česání medvědů, nyní se snaží česat politické problémy. Jeho motto: „Hladce od medvědů k zákonům!“",
            "Přináší nový pohled na politiku, protože se vždy snažil vidět věci z výšky - doslova, byl totiž druhým nejvyšším chlapec ve své třídě.",
            "Jediný kandidát, který dokáže rozpoznat 50 odstínů šedi v politických jednáních a zároveň uvařit skvělý guláš.",
            "Jeho zkušenosti s vyjednáváním začaly u stolu s rodiči o prodloužení večerního vysílání, nyní se s nimi snaží vyjednat novou budoucnost Callibristánu.",
            "Má nejlepší smysl pro humor v politice - každý jeho projev začíná vtipem a končí aplausem.",
            "Jeho hlavní politická strategie je postavena na dvou pilířích: pravidelná koupel v pramenité vodě a každodenní poslech 'Eye of the Tiger' při cvičení.",
            "Pracoval jako detektiv na odhalování tajných vzkazů ve školních poznámkách. Teď se věnuje odhalování skrytých potřeb národa.",
            "Bývalý mistr ve střelbě ze squashe, teď zamířil svou přesnost k politickým cílům. Jeho heslo: „Zásah do srdce problému!“",
            "Pracoval jako detektiv na odhalování tajných vzkazů ve školních poznámkách. Teď se věnuje odhalování skrytých potřeb národa.",
        };

        string[] biosEN = {
            "Promising a chicken in every pot, a car in every garage, and a pet unicorn for every child – because political promises should be legendary!",
            "Putting the 'hip' in 'Presidential.' Get ready for a four-year term of moonwalks and disco diplomacy.",
            "Knows the real secret to world peace – mandatory pizza parties every Friday. Who can argue when there's pizza?",
            "Committed to building bridges and solving problems, but mainly focused on who let the dogs out. Time for answers boys!",
            "Promising a White House makeover with feng shui and unicorn glitter. Because a well-decorated leader is a happy leader.",
            "Not just a candidate – a stand-up comedian in a suit. Get ready for a presidential roast at the State of the Union. Politics can be funny!",
            "Advocating for mandatory nap times for all citizens. A well-rested nation is a productive nation – who doesn't love a good nap?",
            "Believes in transparency, accountability, and free WiFi for everyone. A connected nation is a happy nation, and memes are diplomatic gold.",
            "Ready to tackle big issues like whether pineapple belongs on pizza and if a hot dog is a sandwich. Pressing matters can be controversial!",
            "Promising to replace the Oval Office desk with a giant etch-a-sketch for a fresh start every day. Shake things up, why not?",
        };


        string language = PlayerPrefs.GetString("language");

        // get name
        while (true) {
            firstNamesCS.Shuffle();
            lastNamesCS.Shuffle();
            namesEN.Shuffle();

            if (_usedStrings.Contains(firstNamesCS[0]) || _usedStrings.Contains(lastNamesCS[0]) || _usedStrings.Contains(namesEN[0])) {
                continue;
            }
            Name = (language == "english") ? namesEN[0] : firstNamesCS[0] + " " + lastNamesCS[0];
            _usedStrings.AddRange(new List<string> { namesEN[0], firstNamesCS[0], lastNamesCS[0] });
            break;
        }

        // get bio
        biosCS.Shuffle();
        biosEN.Shuffle();
        // if the first one is used 
        if (_usedStrings.Contains(biosCS[0]) || _usedStrings.Contains(biosEN[0])) {
            Bio = (language == "english") ? biosEN[1] : biosCS[1];
        }
        else {
            Bio = (language == "english") ? biosEN[0] : biosCS[0];
        }
        _usedStrings.Add(Bio);

        Age = UnityEngine.Random.Range(minAge, maxAge);
    }

    public void SetInfoCardParams()
    {
        InfoCard.Name.text = Name;
        InfoCard.Age.text = Age.ToString();
        InfoCard.Bio.text = Bio;
    }

    private void GenerateNewCandidate()
    {
        // generate new properties and special skill
        // propertiesManager.ResetProperties();
        GoodProperties = propertiesManager.GetCandidateProperties(good: true);
        BadProperties = propertiesManager.GetCandidateProperties(good: false);
        SpecialSkill = propertiesManager.GetSpecialSkill();

        SetRandomInfo();
        SetInfoCardParams();

        // set authenticity to 50%
        Authenticity = 50;
        UpdateAuthenticityBar();
        InfoCard.Positives.text = PropertiesToString(GoodProperties);
        InfoCard.Negatives.text = PropertiesToString(BadProperties);
        InfoCard.Mastery.text = SpecialSkill.Description;
        InfoCard.MasteryName.text = SpecialSkill.Text;
    }

    private string PropertiesToString(Property[] props)
    {

        string result = "";
        foreach (var prop in props)
        {
            result += prop.Text;
            result += '\n';
        }

        return result.Substring(0, result.Length);
    }

    private void Start()
    {
        GenerateNewCandidate();
        UpdateAuthenticityBar();
        HideAuthenticityBar();
    }

    private void Update() {
        float updateSpeed = 4f / (float)MaxAuthenticity;  // 4 % per second

        float currentFillAmount = authenticityBar.fillAmount;
        float desiredFillAmount = (float)Authenticity / (float)MaxAuthenticity;
        float dist = updateSpeed * Time.deltaTime;

        float newFillAmount = currentFillAmount;
        if (Mathf.Abs(desiredFillAmount - currentFillAmount) <= dist) {
            newFillAmount = desiredFillAmount;
        }
        else {
            newFillAmount += (desiredFillAmount > currentFillAmount) ? dist : -dist;
        }

        authenticityBar.fillAmount = newFillAmount;
    }
}
