using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Candidate : MonoBehaviour
{
    [SerializeField]
    private ProgressBar authenticityBar;

    public CharacterInfoCard InfoCard;

    private PropertiesManager propertiesManager;

    public Property[] GoodProperties { get; private set; }
    public Property[] BadProperties { get; private set; }
    public SpecialSkill SpecialSkill { get; private set; }

    private const int maxAuthenticity = 100;

    public int Authenticity
    {
        get => (int)authenticityBar.Value;
    }

    void Awake() { propertiesManager = GameObject.FindGameObjectWithTag("logic").GetComponent<PropertiesManager>(); }

    public void ChangeAuthenticity(int deltaAuthenticity)
    {
        authenticityBar.Value = Mathf.Clamp(deltaAuthenticity, 0, maxAuthenticity);
        if (Authenticity <= maxAuthenticity / 10)
        {
            // auto lose game
        }
    }

    public static string[,] GetRandomInfo()
    {
        string[] names = {
            "Jack Statesman",         "Olivia Diplomacia",     "Winston Debatewell",   "Harper Policyson",
            "Grace Lawmaker",         "Felix Governance",      "Sylvia Senatestar",    "Maxwell Publicservant",
            "Eleanor Republica",      "Victor Capitolwise",    "Penelope Pollster",    "Harrison Legislationaire",
            "Amelia Statescraft",     "Dexter Politikid",      "Valerie Caucusqueen",  "Leo Campaigner",
            "Clara Electra",          "Desmond Rhetorico",     "Fiona Parliamentress", "Oscar Statutekeeper",
            "Zoey Debateguru",        "Preston Politypioneer", "Serena Rulermind",     "Nolan Policywise",
            "Lila Legislaturelia",    "Dominic Civicman",      "Vanessa Politiq",      "Simon Governington",
            "Mira Consensushine",     "Elliott Politeer",      "Abigail Voteville",    "Angus Bureaucraton",
            "Isabella Ballotbelle",   "Felix Mandateman",      "Nadia Caucusfire",     "Graham Statesmind",
            "Celeste Parliamentista", "Owen Legislativeon",    "Maya Electique",       "Nolan Publiconductor",
            "Zoe Politess",           "Vincent Policygeist",   "Harper Statedancer",   "Gemma Ballotbabe",
            "Oliver Civicwise",       "Lydia Lawlady",         "Quentin Pollitician",  "Esme Policygem",
            "Theodore Governator",    "Phoebe Politiqueen",

        };

        int minAge = 26;
        int maxAge = 80;

        string[] bios = {
            "Born into a family with a legacy of political intrigue, this candidate has mastered the art of manipulation and backroom dealings. Their rise through the ranks is marked by a shrewd and calculated approach to power.",
            "With a background in intelligence and covert operations, this candidate's political career is shrouded in secrecy. Their connections to shadowy organizations raise questions about their true allegiance and motives.",
            "A master of disinformation, this candidate has honed the skill of shaping public perception through manipulation. Their campaigns are marked by smear tactics and a willingness to exploit fears for personal gain.",
            "Descended from a lineage known for ruthless governance, this candidate embraces a Machiavellian approach to politics. Their ambition knows no bounds, and they are willing to sacrifice ethics for the sake of amassing power.",
            "Raised in a family with a history of corruption, this candidate's political journey is stained by scandals and unethical practices. Their pursuit of personal wealth and influence takes precedence over the welfare of the constituents.",
            "A puppeteer behind the scenes, this candidate thrives on orchestrating political chaos to create opportunities for personal gain. Their connections to clandestine networks make them a dangerous force in the world of politics.",
            "From a background of organized crime, this candidate has seamlessly transitioned into the political arena, using their network to control key power structures. Their policies serve the interests of the underworld rather than the public.",
            "Dedication to public service is a façade for this candidate, who secretly leverages their position for personal enrichment. Behind the scenes, they engage in corruption and exploit their authority for personal gain.",
            "Passion for democratic values masks this candidate's true agenda of undermining institutions. Their commitment to civic engagement is a smokescreen for sowing discord and eroding the foundations of democracy.",
            "A wolf in sheep's clothing, this candidate presents a polished image while harboring authoritarian tendencies. Their vision for governance involves consolidating power and suppressing dissent through any means necessary.",
            "Born into a family of public servants, this candidate has dedicated their life to upholding the principles of justice and fairness. Their extensive experience in international relations makes them a seasoned diplomat ready to navigate the complexities of modern politics.",
            "With a background in foreign affairs and a passion for diplomacy, this candidate has spent years fostering connections between nations. Their commitment to building bridges and finding common ground makes them an ideal candidate for promoting global harmony.",
            "A skilled orator from an early age, this candidate's prowess in debates and discussions has earned them a reputation for being articulate and persuasive. Their commitment to open dialogue makes them a strong advocate for transparent governance.",
            "A childhood dream of shaping policies that positively impact lives has driven this candidate's career in politics. With a keen understanding of socio-economic issues, they aim to implement effective policies that address the needs of diverse communities.",
            "Raised in a family with a history of public service, this candidate is driven by a deep sense of responsibility to their community. Their legal background equips them with the tools needed to craft legislation that promotes justice and equality.",
            "A technocrat with a vision for streamlined governance, this candidate brings innovative solutions to the political arena. Their expertise in leveraging technology for efficient public administration sets them apart as a forward-thinking candidate.",
            "From grassroots activism to the Senate, this candidate's journey exemplifies their commitment to social justice. Their advocacy for marginalized communities and legislative experience make them a compassionate and effective legislator.",
            "Dedication to public service is rooted in this candidate's belief in the power of community-driven initiatives. Their experience working with non-profit organizations showcases their commitment to making a positive impact on society.",
            "Passion for democratic values led this candidate to champion electoral reforms and civic engagement. With a history of community organizing, they aspire to empower citizens to actively participate in shaping their government.",
            "A career in public administration and finance positions this candidate as someone with a strong grasp of economic policies. Their commitment to fiscal responsibility and strategic planning makes them a reliable steward of public resources.",
        };

        names.Shuffle();
        bios.Shuffle();

        string name1 = names[0];
        string age1 = UnityEngine.Random.Range(minAge, maxAge).ToString();
        string bio1 = bios[0];

        string name2 = names[1];
        string age2 = UnityEngine.Random.Range(minAge, maxAge).ToString();
        string bio2 = bios[1];

        return new string[,] {
            {name1, age1, bio1},
            {name2, age2, bio2}
        };
    }

    public void SetInfo(string name, string age, string bio)
    {
        InfoCard.Name.text = name;
        InfoCard.Age.text = age;
        InfoCard.Bio.text = bio;
    }

    private void GenerateNewCandidate()
    {
        // generate new properties and special skill
        propertiesManager.ResetProperties();
        GoodProperties = propertiesManager.GetCandidateProperties(good: true);
        BadProperties = propertiesManager.GetCandidateProperties(good: false);
        SpecialSkill = propertiesManager.GetSpecialSkill();

        // set authenticity to 50%
        // authenticityBar.Value = maxAuthenticity / 2;

        InfoCard.Positives.text = PropertiesToString(GoodProperties);
        InfoCard.Negatives.text = PropertiesToString(BadProperties);
        InfoCard.Mastery.text = SpecialSkill.Text + ": " + SpecialSkill.Description;
    }

    private string PropertiesToString(Property[] props)
    {

        string result = "";
        foreach (var prop in props)
        {
            result += prop.Text;
            result += ", ";
        }

        return result.Substring(0, result.Length - 2);
    }

    void Start()
    {
        // authenticityBar.Max = maxAuthenticity;
        GenerateNewCandidate();
    }
}
