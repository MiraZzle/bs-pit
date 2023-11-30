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

    private List<Property> _goodProperties;
    public List<Property> BadProperties { get; private set; }
    public SpecialSkill SpecialSkill { get; private set; }

    public string Name { get; private set; }

    public const int MaxAuthenticity = 100;

    public int Authenticity { get; private set; } = MaxAuthenticity / 2;


    void SetUpAuthenticityBar() {
        if (authenticityBar is not null) {
            authenticityBar.fillAmount = (float)Authenticity / (float)MaxAuthenticity;
        }
    }

    void UpdateAuthenticityBar() {
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

    public void ChangeAuthenticity(int deltaAuthenticity) {
        Authenticity = Mathf.Clamp(Authenticity + deltaAuthenticity, 0, MaxAuthenticity);
    }

 
    private void SetCandidateInfo(CandidateInfo info)
    {
        Name = info.Name;
        InfoCard.Name.text = Name;
        InfoCard.Age.text = info.Age;
        InfoCard.Bio.text = info.Bio;
        InfoCard.Positives.text = PropertiesToString(_goodProperties);
        InfoCard.Negatives.text = PropertiesToString(BadProperties);
        InfoCard.Mastery.text = SpecialSkill.Description;
        InfoCard.MasteryName.text = SpecialSkill.Text;
    }

    private void GenerateNewCandidate()
    {
        // generate new properties and special skill
        _goodProperties = PropertiesManager.GetGoodCandidateProperties();
        BadProperties = PropertiesManager.GetBadCandidateProperties();
        SpecialSkill = PropertiesManager.GetSpecialSkill();

        
        CandidateInfo info = CharacterInfoManager.GetRandomCandidateInfo();
        SetCandidateInfo(info);
    }

    private string PropertiesToString(List<Property> properties)
    {
        string result = "";
        foreach (var prop in properties)
        {
            result += prop.Text;
            result += '\n';
        }

        return result[..];
    }

    private void Start()
    {
        InfoCard.Hide();
        GenerateNewCandidate();
        SetUpAuthenticityBar();
        HideAuthenticityBar();
    }

    private void Update() {
        UpdateAuthenticityBar();
    }
}
