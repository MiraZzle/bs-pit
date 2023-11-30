using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public struct CandidateInfo {
    public string Name { get; private set; }
    public string Age { get; private set; }
    public string Bio { get; private set; }
    public CandidateInfo(string name, int age, string bio) {
        Name = name;
        Age = age.ToString();
        Bio = bio;
    }
}

public class CharacterInfoManager : MonoBehaviour
{
    private static List<string> _usedStrings = new();

    void Start() {
        _usedStrings.Clear();
    }

    public static CandidateInfo GetRandomCandidateInfo() {
        string[] firstNamesCS = { "Kazisvět", "Honimír", "Horymír", "Spytihněv", "Kazimír", "Dobromír", "Mečislav" };

        string[] lastNamesCS = { "Dobrotivý", "Milosrdný", "Spravedlivý", "Laskavý", "Zlobivý", "Urputný" };

        string[] namesEN = {
            "Ben Dover",   "Whimsy McSnicker", "Barb Dwyer",
            "Hal Jalikee", "Jestin Jesterson", "Justin Time", "Drew Peacock",
        };

        int minAge = 30;
        int maxAge = 80;

        string[] biosCS = {
            "Expert na rychlé rozhodování a ještě rychlejší kafe. Slogan kampaně: „Jedno espresso, jedna země, jednoznačná rozhodnutí!“",
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

        string name; int age; string bio;

        // get name
        while (true) {
            if (language == "english") {
                namesEN.Shuffle();
                if (_usedStrings.Contains(namesEN[0])) continue;
                name = namesEN[0];
                _usedStrings.Add(namesEN[0]);
                break;
            }
            else {
                firstNamesCS.Shuffle();
                lastNamesCS.Shuffle();
                if (_usedStrings.Contains(firstNamesCS[0]) || _usedStrings.Contains(lastNamesCS[0])) continue;
                name = firstNamesCS[0] + " " + lastNamesCS[0];
                _usedStrings.Add(firstNamesCS[0]);
                _usedStrings.Add(lastNamesCS[0]);
                break;
            }
        }

        // get bio
        biosCS.Shuffle();
        biosEN.Shuffle();
        // use that there are only two candidates
        if (language == "english") {
            bio = (_usedStrings.Contains(biosEN[0])) ? biosEN[1] : biosEN[0];
        }
        else {
            bio = (_usedStrings.Contains(biosCS[0])) ? biosCS[1] : biosCS[0];
        }
        _usedStrings.Add(bio);

        age = UnityEngine.Random.Range(minAge, maxAge);

        return new CandidateInfo(name, age, bio);
    }
}
