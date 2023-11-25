using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;

public enum PropertyType {
    Fascist, Commie, ProRussian, Corrupt, Drunkard, Random, SSKuciak, SSCapiHnizdo, SSKollar, SSFlakanec, SSAligator
}

public class Property {
    public PropertyType Type { get; private set; }

    private string _textEN;
    private string _textCS;
    public string Text => (PlayerPrefs.GetString("language") == "english") ? _textEN : _textCS;

    public bool IsGood { get; private set; }

    private readonly List<Question> _questions = new List<Question>();

    public Property(PropertyType type, string textEN, string textCS, bool isGood, List<Question> questions) {
        Type = type;
        _textEN = textEN;
        _textCS = textCS;
        IsGood = isGood;
        _questions = questions;
    }

    public Question GetQuestion() {
        if (_questions.Count == 0) return null;
        _questions.Shuffle();
        return _questions[0];
    }

    public virtual void Reset() {
        foreach (Question question in _questions) question.ResetAnswers();
    }
}

public class SpecialSkill : Property {
    public bool IsUsed;

    public SpecialSkill(PropertyType type, string textEN, string textCS, List<Question> questions)
        : base(type, textEN, textCS, false, questions) { }

    public override void Reset() {
        base.Reset();
        IsUsed = false;
    }
}


public class PropertiesManager : MonoBehaviour
{
    private static List<Property> _properties = new();
    private static List<SpecialSkill> _specialSkills = new();

    public Property[] GetCandidateProperties(bool good, int count = 3) {
        _properties.Shuffle();
        int addedProperties = 0;
        Property[] randomProperties = new Property[count];
        foreach (Property property in _properties) {
            if (property.IsGood == good) randomProperties[addedProperties++] = property;
            if (addedProperties == count) break;
        }

        return randomProperties;
    }

    public SpecialSkill GetSpecialSkill() {
        _specialSkills.Shuffle();
        foreach (var skill in _specialSkills) {
            if (!skill.IsUsed) {
                skill.IsUsed = true;
                return skill;
            }
        }
        // this should never happen
        return null; 
    }

    public void ResetProperties() {
        foreach (Property property in _properties) {
            property.Reset();
        }
        foreach (SpecialSkill skill in _specialSkills) {
            skill.Reset();
        }
    }

    static void LoadProperties() {
        static void CreateProperty(
            string propertyEN, string propertyCS, PropertyType type, bool isGood, 
            string questionEN, string questionCS,
            int ans1Autenticity, int ans1Volici, string ans1EN, string ans1CS,
            int ans2Autenticity, int ans2Volici, string ans2EN, string ans2CS,
            int ans3Autenticity, int ans3Volici, string ans3EN, string ans3CS
            )
        {
            List<Answer> answers = new List<Answer> {
                new Answer(ans1Autenticity, ans1Volici, ans1EN, ans1CS, AnswerType.Neutral),
                new Answer(ans2Autenticity, ans2Volici, ans2EN, ans2CS, AnswerType.Neutral),
                new Answer(ans3Autenticity, ans3Volici, ans3EN, ans3CS, AnswerType.Neutral)
            };

            Question question = new Question(questionEN, questionCS, answers);
            _properties.Add(new Property(type, propertyEN, propertyCS, isGood, new List<Question> { question }));
        }

        // WITH QUESTIONS   
        // fascist
        CreateProperty(
            "Neo-fascist", "Neofa3ista", PropertyType.Fascist, isGood: false,
            questionEN: "Can you comment on your alleged participation in neo-fascist actions?",
            questionCS: "Múžete se nějak vyjádřit k vaší údajné účasti na neofašistických akcích?",
            -10, 10,
            ans1EN: "It's a campaign, I absolutely refuse my participation in any such event!",
            ans1CS: "Je to kampaň, naprosto odmítám mojí účast na nějaké takové akci!",
            0, 5,
            ans2EN: "Everyone keeps talking about some alleged involvement, but no one can present any real evidence!",
            ans2CS: "Všichni pořád mluví o nějaké údajné účasti, ale nikdo nedokáže prezentovat žádné skutečné důkazy!",
            8, -8,
            ans3EN: "I did attend such an event and I admit it was a mistake.",
            ans3CS: "Opravdu jsem se takové akce zúčastnil a uznávám, že to byla chyba."
        );

        // commie

        // pro Russian

        // corrupt

        // drunkard

        // Random - kicked a kitty

        // Random - doesn't recycle

        // Random - loves chatGTP a bit too much

        // SSKuciak

        // SSCapiHnizdo

        // SSKollar

        // SSFlakanec

        // SSAligator

    }

    void Start()
    {
        LoadProperties();
    }
}
