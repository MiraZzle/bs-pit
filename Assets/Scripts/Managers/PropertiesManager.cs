using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;

public enum PropertyType {
    Fasist, Commie, Ukraine, Izrael, Random, SpecialSkill
}

public class Property {
    public PropertyType Type { get; private set; }
    public string TextEN { get; private set; }
    public string TextCS { get; private set; }
    public bool IsGood { get; private set; }

    private List<Question> _questions = new List<Question>();

    public Property(PropertyType type, string textEN, string textCS, bool isGood, List<Question> questions) {
        Type = type;
        TextEN = textEN;
        TextCS = textCS;
        IsGood = isGood;
        _questions = questions;
    }

    public virtual void Reset() {
        foreach (Question question in _questions) question.ResetAnswers();
    }
}

public class SpecialSkill : Property {
    public bool IsUsed;

    public SpecialSkill(string textEN, string textCS, List<Question> questions)
        : base(PropertyType.SpecialSkill, textEN, textCS, false, questions) { }

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

    void Start()
    {
        // load properties
    }
}
