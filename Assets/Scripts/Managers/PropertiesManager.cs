using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public enum PropertyType
{
    Fascist,
    Commie,
    ProRussian,
    Corrupt,
    Drunkard,
    Kleptoman,
    SSKuciak,
    SSCapiHnizdo,
    SSKollar,
    SSFlakanec,
    SSAligator,
    Random
}

public class Property {
    public PropertyType Type { get; private set; }

    private string _textEN;
    private string _textCS;
    public string Text => (PlayerPrefs.GetString("language") == "english") ? _textEN : _textCS;

    public bool IsGood { get; private set; }
    public bool IsUsed { get; set; } = false;

    private readonly List<Question> _questions = new List<Question>();

    public Property(PropertyType type, string textEN, string textCS, bool isGood)
    {
        Type = type;
        _textEN = textEN;
        _textCS = textCS;
        IsGood = isGood;
    }

    public Property(PropertyType type, string textEN, string textCS, bool isGood, List<Question> questions)
        : this(type, textEN, textCS, isGood)
    {
        _questions = questions;
    }

    public Question GetQuestion()
    {
        if (_questions.Count == 0)
            return null;
        _questions.Shuffle();
        return _questions[0];
    }

    public void Reset()
    {
        IsUsed = false;
        foreach (Question question in _questions)
            question.ResetAnswers();
    }
}

public class SpecialSkill : Property
{
    private string _descriptionEN;
    private string _descriptionCS;

    public string Description => (PlayerPrefs.GetString("language") == "english") ? _descriptionEN : _descriptionCS;

    public SpecialSkill(PropertyType type, string textEN, string textCS, string descriptionEN, string descriptionCS,
                        List<Question> questions)
        : base(type, textEN, textCS, false, questions)
    {
        _descriptionEN = descriptionEN;
        _descriptionCS = descriptionCS;
    }
}

public class PropertiesManager : MonoBehaviour
{
    private static List<Property> _properties = new();
    private static List<SpecialSkill> _specialSkills = new();

    public Property[] GetCandidateProperties(bool good, int num = 3)
    {
        _properties.Shuffle();
        int addedProperties = 0;
        Property[] randomProperties = new Property[num];
        foreach (Property property in _properties)
        {
            if (property.IsGood == good && !property.IsUsed) {
                property.IsUsed = true;
                randomProperties[addedProperties++] = property;
            }
            
            if (addedProperties == num)
                break;
        }

        return randomProperties;
    }

    public SpecialSkill GetSpecialSkill()
    {
        _specialSkills.Shuffle();
        foreach (var skill in _specialSkills)
        {
            if (!skill.IsUsed)
            {
                skill.IsUsed = true;
                return skill;
            }
        }
        // this should never happen
        return null;
    }

    public void ResetProperties()
    {
        foreach (Property property in _properties)
        {
            property.Reset();
        }
    }

    static void LoadProperties()
    {
        static void CreateProperty(string textEN, string textCS, PropertyType type, bool isGood, string questionEN,
                                   string questionCS, string ans1EN, string ans1CS, string ans2EN, string ans2CS,
                                   string ans3EN, string ans3CS)
        {
            // populist
            int ans1Autenticity = Random.Range(-10, -7);
            int ans1Volici = Random.Range(7, 10);
            // avoiding
            int ans2Autenticity = Random.Range(-2, 2);
            int ans2Volici = Random.Range(3, 6);
            // admitting
            int ans3Autenticity = Random.Range(6, 9);
            int ans3Volici = Random.Range(-2, 2);

            List<Answer> answers
                = new List<Answer> { new Answer(ans1Autenticity, ans1Volici, ans1EN, ans1CS, AnswerType.Populist),
                                     new Answer(ans2Autenticity, ans2Volici, ans2EN, ans2CS, AnswerType.Neutral),
                                     new Answer(ans3Autenticity, ans3Volici, ans3EN, ans3CS, AnswerType.Real) };

            Question question = new Question(questionEN, questionCS, QuestionType.Personal, answers, type);
            _properties.Add(new Property(type, textEN, textCS, isGood, new List<Question> { question }));
        }

        static void CreateSpecialSkill(string textEN, string textCS, PropertyType type, string descriptionEN,
                                       string descriptionCS, string questionEN, string questionCS, string ans1EN,
                                       string ans1CS, string ans2EN, string ans2CS, string ans3EN, string ans3CS)
        {
            // populist
            int ans1Autenticity = Random.Range(-10, -7);
            int ans1Volici = Random.Range(7, 10);
            // avoiding
            int ans2Autenticity = Random.Range(-2, 2);
            int ans2Volici = Random.Range(3, 6);
            // admitting
            int ans3Autenticity = Random.Range(6, 9);
            int ans3Volici = Random.Range(-5, 0);

            List<Answer> answers
                = new List<Answer> { new Answer(ans1Autenticity, ans1Volici, ans1EN, ans1CS, AnswerType.Populist),
                                     new Answer(ans2Autenticity, ans2Volici, ans2EN, ans2CS, AnswerType.Neutral),
                                     new Answer(ans3Autenticity, ans3Volici, ans3EN, ans3CS, AnswerType.Real) };

            Question question = new Question(questionEN, questionCS, QuestionType.Personal, answers, type);
            _specialSkills.Add(
                new SpecialSkill(type, textEN, textCS, descriptionEN, descriptionCS, new List<Question> { question }));
        }

        // WITH QUESTIONS
        // fascist
        CreateProperty(
            "Neo-fascist", "Neofšista", PropertyType.Fascist, isGood: false,
            questionEN: "Can you comment on your alleged participation in neo-fascist events?",
            questionCS: "Múžete se nějak vyjádřit k vaší údajné účasti na neofašistických akcích?",
            ans1EN: "It's a campaign and all lies! I am opposed to all extremist movements and have always defended democratic values.",
            ans1CS: "Je to kampaň a samé lži! Jsem odpůrcem všech extrémistických hnutí a vždy jsem hájil demokratické hodnoty.",
            ans2EN: "Everyone keeps talking about some alleged involvement, but no one can present any real evidence!",
            ans2CS: "Všichni pořád mluví o nějaké údajné účasti, ale nikdo nedokáže prezentovat žádné skutečné důkazy!",
            ans3EN: "I did attend such an event and after deep consideration, I admit it was a mistake.",
            ans3CS: "Ano, byl jsem na této akci. Po hlubokém zvažování uznávám, že to byla chyba.");

        // commie
        CreateProperty(
            "Communist", "Komunista", PropertyType.Commie, isGood: false,
            questionEN: "Can you somehow defend your communist-oriented statements and opinions?",
            questionCS: "Dokážete nějak obhájit své komunisticky orientované výroky a názory?",
            ans1EN: "I am not a communist! I'm here for the people, fighting for social justice and equality. In short, I am an ordinary hero of working people!",
            ans1CS: "Nejsem komunista! Jsem tu pro lidi, bojuji za sociální spravedlnost a rovnost. Zkrátka jsem obyčejný hrdina pracujících lidí!",
            ans2EN: "Political labels can be misleading. My approach to politics is based on concrete proposals and solutions for citizens, not labels.",
            ans2CS: "Politické škatulky a označení mohou být zavádějící. Můj přístup k politice je postaven na konkrétních návrzích a řešeních pro občany, ne na štítcích.",
            ans3EN: "Yes, in my younger years I was involved in the Communist Party. And I still think we should give communism another chance.",
            ans3CS: "Ano, ve svých mladých letech jsem se angažoval v komunistické straně. A stále si myslím, že bychom komunismu měli dát ještě jednu šanci.");

        // pro Russian
        CreateProperty(
            "Pro-Russian", "Proruský", PropertyType.ProRussian, isGood: false,
            questionEN: "Concerns about your pro-Russian attitudes have been voiced through the media. You even reportedly met with Vladimir Putin. What can you tell us about that?",
            questionCS: "V médiích se objevily obavy z vašich proruských postojů, dokonce jste se údajně setkal s Vladimírem Putinem. Co nám k tomu řeknete?",
            ans1EN: "These are lies and misinformation! I am a proud patriot and have always defended our national interests. I have nothing to do with any foreign power!",
            ans1CS: "To jsou lži a dezinformace! Jsem hrdý patriot a vždy jsem hájil naše národní zájmy. Nemám nic společného s žádnou cizí mocí!",
            ans2EN: "My positions are based on pragmatism and cooperation with different countries. I do not cooperate with any country at the expense of our interests, and that includes Russia.",
            ans2CS: "Mé postoje jsou založeny na pragmatismu a spolupráci s různými zeměmi. Nespolupracuji s žádnou zemí na úkor našich zájmů, a to zahrnuje i Rusko.",
            ans3EN: "Yes, I met Putin recently. But I don't believe it threatens our national security.",
            ans3CS: "Ano, v nedávné době jsem se setkal s Putinem. Ale nemyslím si, že to ohrožuje naši národní bezpečnost.");

        // corrupt
        CreateProperty(
            "Corrupt", "Zkorumpovaný", PropertyType.Corrupt, isGood: false,
            questionEN: "Can you comment on the allegations of corruption and explain what measures you would take to restore public confidence?",
            questionCS: "Můžete komentovat obvinění z korupce a objasnit, jaké opatření byste přijal pro obnovení důvěry veřejnosti?",
            ans1EN: "These are lies spread by my political rivals! I am an honest politician and have always served my constituents with the best of intentions.",
            ans1CS: "To jsou lži šířené moji politickou konkurencí! Jsem čestný politik a vždy jsem sloužil svým voličům s nejlepším záměrem.",
            ans2EN: "All allegations of corruption are serious and must be thoroughly investigated. I believe in an independent investigation and a fair trial.",
            ans2CS: "Všechna obvinění z korupce jsou vážné a musí být důkladně vyšetřeny. Věřím v nezávislé vyšetřování a spravedlivý soudní proces.",
            ans3EN: "Of course, I have indeed accepted bribes. I'm a politician, that's my paycheck.",
            ans3CS: "Skutečně jsem v minulosti přijmul úplatky. Vždyť jsem politik, to je moje výplata.");

        // drunkard
        CreateProperty(
            "Drunkard", "Opilec", PropertyType.Drunkard, isGood: false,
            questionEN: "You've been seen drunk in public more than once. How would you characterize your relationship with alcohol?",
            questionCS: "Již vícekrát jste byl spatřen opilý na veřejnosti. Jak byste charakterizoval svůj vztah k alkoholu?",
            ans1EN: "That's just nonsensical gossip! My personal life choices have nothing to do with my political work. I'm not a drunken drunk!",
            ans1CS: "To jsou jen nesmyslné pomluvy! Moje osobní životní volby nemají nic společného s mým politickým působením. Nejsem ožrala ožralý!",
            ans2EN: "My personal life is my private life. It is important to focus on my public achievements and political contributions to the citizens.",
            ans2CS: "Můj osobní život je mé soukromí. Je důležité se zaměřit na mé veřejné úspěchy a politické přínosy pro občany.",
            ans3EN: "I'm from Moravia.", ans3CS: "Jsem z Moravy.");

        // kleptoman
        CreateProperty(
            "Kleptomaniac", "Kleptoman", PropertyType.Kleptoman, isGood: false,
            questionEN: "Could you explain why you took the protocol pen during your meeting with the President of Chile?",
            questionCS: "Můžete vysvětlit, proč jste během setkání s Chillským prezidentem přemístil protokolární pero ze stolu do kapsy vašeho saka?",
            ans1EN: "These are absurd accusations! I'm a politician, not a thief. This story is being spread only to discredit me.",
            ans1CS: "To jsou absurdní obvinění! Jsem politik, ne zloděj. Tato příběh se šíří pouze proto, abych byl diskreditován.",
            ans2EN: "Stories of pen theft surprise me, of course. It's important to keep a cool head and not form opinions based on unverified information.",
            ans2CS: "Příběhy o krádeži pera mě samozřejmě překvapují. Je důležité zachovat chladnou hlavu a nezaujímat názory na základě neověřených informací.",
            ans3EN: "Yes, I stole a protocol pen. It was a nice gold trimmed one, and that's irresistible to a kleptomaniac like me.",
            ans3CS: "Ano ukradl jsem protokolární pero. Bylo hezky zlatě zdobené a to je pro kleptomana jako jsem já neodolatelné.");

        // SSKuciak
        CreateSpecialSkill(
            "Kuciak", "Kuciak", PropertyType.SSKuciak,
            descriptionEN: "This candidate is suspected of ordering the murder of a young journalist who was investigating his corruption cases.",
            descriptionCS: "Tento kandidát je podezřelý z objednání vraždy mladého novináře, který vyšetřoval jeho korupční kauzy.",
            questionEN: "How do you comment on the accusations of ordering the murder of the investigative journalist who investigated you?",
            questionCS: "Jak se vyjádříte k obviněním z objednání vraždy na investigativního novináře, který vás vyšetřoval?",
            ans1EN: "These are absurd accusations from political opponents! I would never be involved in such a heinous act. This is pure fiction.",
            ans1CS: "Jedná se o absurdní obvinění ze strany mých politických oponentů! Nikdy bych se nezapletl do takového hnusného činu. To je čistá fikce.",
            ans2EN: "Such allegations are very serious and must be properly investigated by law enforcement authorities. We should await the results of the investigation.",
            ans2CS: "Taková obvinění jsou velmi vážná a musí být řádně vyšetřena orgány činnými v trestním řízení. Měli bychom počkat na výsledky vyšetřování.",
            ans3EN: "Maybe I did it, maybe I didn't. Keep poking around and you might start to regret it.",
            ans3CS: "Možná jsem to udělal, možná ne. Šťourejte dál a možná toho začnete litovat.");

        // SSCapiHnizdo
        CreateSpecialSkill(
            "The Stork's Nest", "Čapí hnízdo", PropertyType.SSCapiHnizdo,
            descriptionEN: "This candidate is accused of illegal financing of political parties, corruption and tax fraud.",
            descriptionCS: "Tento kandidát je podeželý z nelegálního financování politických stran, korupce a daňových podvodů.",
            questionEN: "How would you explain the recent allegations of tax fraud and corruption concerning you?",
            questionCS: "Jak byste vysvětlil nedávná obvinění z daňových podvodů a korupce, které se vás týkají?",
            ans1EN: "It's a campaign, I'm a victim of political intrigue and my finances are fully transparent!",
            ans1CS: "Je to kampaň, jsem obětí politických intrik a mé finance jsou plně transparentní!",
            ans2EN: "The tax charges are very serious. I am prepared to cooperate with the tax authorities and prove my innocence.",
            ans2CS: "Daňová obvinění jsou velmi vážná. Jsem připraven spolupracovat s daňovými úřady a prokázat svoji nevinu.",
            ans3EN: "And why should I tell you all my income? Sorry duh.",
            ans3CS: "A proč bych vám já měl sdělovat všechny moje příjmy? Sdělujete vy snad někomu svoje přijímy? Sorry jako.");

        // SSKollar
        CreateSpecialSkill(
            "Kollár", "Kollár", PropertyType.SSKollar,
            descriptionEN: "This candidate already has sixteen children with eleven wives. The number of offspring may not be finite.",
            descriptionCS: "Tento kandidát má již šestnáct dětí s jedenácti ženami. Počet potomků nemusí být konečný.",
            questionEN: "Can your extensive family life have any influence on your political agenda?",
            questionCS: "Může váš rozsáhlý rodinný život nějak ovlivnit vaši politickou agendu?",
            ans1EN: "This is just speculation and attacks on my personal family situation! It's a private matter that has nothing to do with my political career.",
            ans1CS: "Jedná se o útoky na mou osobní rodinnou situaci! Je to věc soukromí, která nemá nic společného s mou politickou kariérou.",
            ans2EN: "My family situation is a personal matter that does not affect my ability to serve the citizens. Let's focus on the political issues.",
            ans2CS: "Má rodinná situace je osobní záležitost, která nemá vliv na mou schopnost sloužit občanům. Zaměřme se na politické otázky.",
            ans3EN: "Yes, I have many children and wives. But at the same time I like to spread awareness about traditional family values.",
            ans3CS: "Ano, mám mnoho dětí a manželek. Zároveň ale rád šířím osvětu o tradičních rodinných hodnotách.");

        // SSFlakanec
        CreateSpecialSkill(
            "The Slapper", "Flákanec", PropertyType.SSFlakanec,
            descriptionEN: "This candidate prefers handing out slaps to serious policy debate.",
            descriptionCS: "Tento kandidát dává přednost rozdávání flákanců oproti seriózní politické debatě.",
            questionEN: "You have resorted to physical violence several times in various political disagreements. Can you comment on that?",
            questionCS: "Několikrát jste v různých politických neshodách uchýlil k fyzickému násilí. Můžete se k tomu nějak vyjádřit?",
            ans1EN: "These are just fabrications of my political competitors who are afraid of my determination and strength. I am a fighter for justice!",
            ans1CS: "Jsou to jen výmysly mé politické konkurence, kteří se bojí mého odhodlání a síly. Jsem bojovník za spravedlnost!",
            ans2EN: "It is important to maintain standards and respect in the political debate. Violence has no place in politics, and I condemn all forms of physical assault.",
            ans2CS: "Je důležité zachovat úroveň a respekt v politické diskusi. Násilí nemá v politice místo, a já odsuzuji jakékoli formy fyzického napadání.",
            ans3EN: "Sometimes there are situations where a good smack is the best solution!",
            ans3CS: "Občas jsou situace, kde je pořádný flákanec to nejlepší řešení!");

        // SSAligator
        CreateSpecialSkill(
            "Alligator", "Aligátor", PropertyType.SSAligator,
            descriptionEN: "This candidate has a problem with alcohol and is rude to journalists.",
            descriptionCS: "Tento kandidát má problémy s alkoholem a chová se neomaleně vůči novinářům.",
            questionEN: "How would you comment on concerns about your behaviour towards journalists? Do you have a problem with alcohol?",
            questionCS: "Jak byste komentoval obavy o vaše chování vůči novinářům a jak hodláte zachovat profesionální vztahy s médii?",
            ans1EN: "These are just attempts by my political opponents to discredit me! My meetings with journalists are always professional and respectful.",
            ans1CS: "Jedná se pouze pokusy mé politické konkurence diskreditovat mě! Moje setkání s novináři jsou vždy profesionální a plné respektu.",
            ans2EN: "I appreciate the role of the media in a democracy. Sometimes my behaviour can be controversial, and I am open to discussing how to improve our relations.",
            ans2CS: "Oceňuji roli médií v demokracii. Někdy může být mé chování kontroverzní, a já jsem otevřený diskusi o tom, jak zlepšit naše vzájemné vztahy.",
            ans3EN: "Yes, I have a problem with alcohol, which affects my behavior. On the other hand, journalists are freeloaders and should be abolished.",
            ans3CS: "Ano, mám problém s alkoholem, což ovlivňuje mé chování. Na druhou stranu, novináři jsou příživníci a měli bychom je zrušit.");

        // Random bad
        _properties.Add(new Property(PropertyType.Random, "Kicked a kitty", "Nakopnul koťátko", isGood: false));

        // Random good
        _properties.Add(new Property(PropertyType.Random, "Diplomatic", "Diplomat", isGood: true));
        _properties.Add(new Property(PropertyType.Random, "Charismatic", "Charizmatický", isGood: true));
        _properties.Add(new Property(PropertyType.Random, "Honest", "Upřímný", isGood: true));
        _properties.Add(new Property(PropertyType.Random, "Fair", "Spravedlivý", isGood: true));
        _properties.Add(new Property(PropertyType.Random, "Ethically minded", "Eticky smýšlející", isGood: true));
        _properties.Add(new Property(PropertyType.Random, "Highly educated", "Vysoce vzdělaný", isGood: true));
        _properties.Add(new Property(PropertyType.Random, "Good strategist", "Dobrý stratég", isGood: true));
        _properties.Add(new Property(PropertyType.Random, "Intelligent", "Inteligetní", isGood: true));
        _properties.Add(new Property(PropertyType.Random, "Creative", "Kreativní", isGood: true));
        _properties.Add(new Property(PropertyType.Random, "Purposeful", "Cílevědomý", isGood: true));
        _properties.Add(new Property(PropertyType.Random, "Ambitions", "Ambiciózní", isGood: true));
        _properties.Add(new Property(PropertyType.Random, "Well Organized", "Organizovaný", isGood: true));
    }

    void Awake() { 
        LoadProperties(); 
    }
}
