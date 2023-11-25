using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;

public enum PropertyType {
    Fasist, Commie, ProRussian, Corrupt, Random, SSKuciak, SSCapiHnizdo, SSKollar, SSFlakanec, SSAligator
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
    private List<Property> _properties = new();
    private List<SpecialSkill> _specialSkills = new();

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

    }

    void Start()
    {
        LoadProperties();
    }
}
