using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Candidate : MonoBehaviour {
    [SerializeField]
    private ProgressBar authenticityBar;
    
    private PropertiesManager propertiesManager = GameObject.FindGameObjectWithTag("logic").GetComponent<PropertiesManager>();

    public Property[] GoodProperties { get; private set; }
    public Property[] BadProperties { get; private set; }
    public SpecialSkill SpecialSkill { get; private set; }


    private const int maxAuthenticity = 100;
    public int Authenticity { get => (int)authenticityBar.Value; }

    public void ChangeAuthenticity(int deltaAuthenticity) {
        authenticityBar.Value = Mathf.Clamp(deltaAuthenticity, 0, maxAuthenticity);
        if (Authenticity <= maxAuthenticity / 10) {
            // auto lose game
        }
    }

    public void GenerateNewCandidate() {
        // generate new properties and special skill
        GoodProperties = propertiesManager.GetCandidateProperties(good: true);
        BadProperties = propertiesManager.GetCandidateProperties(good: false);
        SpecialSkill = propertiesManager.GetSpecialSkill();

        // set authenticity to 50%
        authenticityBar.Value = maxAuthenticity / 2;
    }

    void Start() {
        authenticityBar.Max = maxAuthenticity;
        GenerateNewCandidate();
    }
}
