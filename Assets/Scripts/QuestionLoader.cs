// Ignore Spelling: Volici
#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public enum AnswerType
{
    Populist,
    Neutral,
    Real
}

static class MyExtensions
{
    public static void Shuffle<T>(this IList<T> list) {
        double[] key = new double[list.Count];
        for (int i = 0; i < key.Length; i++) {
            key[i] = Random.Range(0f, 1f);
        }

        T[] array = list.ToArray();
        Array.Sort(key, array);
        for (int i = 0; i < list.Count; i++) {
            list[i] = array[i];
        }
    }
}

public record Answer
{
    public int DeltaAuthenticity { get; private set; }
    public int DeltaVolici { get; private set; }

    private string _textEN;
    private string _textCS;
    public string Text => (PlayerPrefs.GetString("language") == "english") ? _textEN : _textCS;

    public AnswerType Type { get; private set; }

    public bool IsUsed = false;
    public Answer(int deltaAuthenticity, int deltaVolici, string textEN, string textCS, AnswerType type)
    {
        DeltaAuthenticity = deltaAuthenticity;
        DeltaVolici = deltaVolici;
        _textEN = textEN;
        _textCS = textCS;
        Type = type;
    }
}

public enum QuestionType
{
    Personal,
    General
}

public record Question
{
    private string _textEN;
    private string _textCS;
    public QuestionType Type { get; private set; }
    public PropertyType? AssociatedProperty { get; private set; }
    public string Text => (PlayerPrefs.GetString("language") == "english") ? _textEN : _textCS;

    private List<Answer> _populistAnswers = new();
    private List<Answer> _neutralAnswers = new();

    public Question(string textEN, string textCS, QuestionType type, List<Answer> answers,
                    PropertyType? property = null)
    {
        _textEN = textEN;
        _textCS = textCS;
        Type = type;
        AssociatedProperty = property;

        foreach (Answer answer in answers)
        {
            if (answer.Type == AnswerType.Populist)
                _populistAnswers.Add(answer);
            else
                _neutralAnswers.Add(answer);
        }
    }

    private void ShuffleAnswers()
    {
        _populistAnswers.Shuffle();
        _neutralAnswers.Shuffle();
    }

    public List<Answer> GetAnswers()
    {
        ShuffleAnswers();

        List<Answer> answers = new();
        // try to get 1 populist answer
        foreach (Answer answer in _populistAnswers)
        {
            if (!answer.IsUsed)
            {
                answers.Add(answer);
                break;
            }
        }

        // fill with normal answers
        foreach (Answer answer in _neutralAnswers)
        {
            if (!answer.IsUsed)
            {
                answers.Add(answer);
                if (answers.Count == 3)
                    break;
            }
        }

        return answers;
    }

    public void ResetAnswers()
    {
        foreach (var answer in _populistAnswers)
            answer.IsUsed = false;
        foreach (var answer in _neutralAnswers)
            answer.IsUsed = false;
    }
}

public class QuestionLoader : MonoBehaviour
{
    static string _questionsFilePath = Path.Combine(Application.streamingAssetsPath, "general-questions.txt");

    static List<Question> _questions = new List<Question>();

    static string[] questionSpecialString = { "O" };
    static string[] englishSpecialString = { "E" };
    static string[] answerSpecialStrings = { "P", "N", "C" };

    static void LoadQuestionsFromFile()
    {
        string TextFromLine(string[] line, int textStartIndex)
        {
            StringBuilder text = new StringBuilder();
            for (int i = textStartIndex; i < line.Length - 1; i++)
            {
                text.Append(line[i] + " ");
            }
            text.Append(line[line.Length - 1]);
            return text.ToString();
        }

        string[] ReadUntilSpecialString(StreamReader reader, string[] specialStrings, string terminator = "END")
        {
            while (!reader.EndOfStream)
            {
                string[] line
                    = reader.ReadLine().Split(new char[] { ' ', '\t' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (line.Length == 0)
                    continue;
                if (specialStrings.Contains(line[0]))
                    return line;
                if (line[0] == terminator)
                    return line;
            }
            return null;
        }

        Answer? LoadAnswer(StreamReader reader)
        {
            string[] answerLineCS = ReadUntilSpecialString(reader, answerSpecialStrings);
            if (answerLineCS[0] == "END")
                return null; // end of question

            string[] answerLineEN = ReadUntilSpecialString(reader, englishSpecialString);

            string answerTextCS = null;
            string answerTextEN = TextFromLine(answerLineEN, 1);
            int answerDeltaAuthenticity = 0;
            int answerDeltaVolici = 0;
            AnswerType answerType = (answerLineCS[0] == "P") ? AnswerType.Populist : AnswerType.Neutral;

            switch (answerLineCS[0])
            {
                case "P": // populist
                    answerDeltaAuthenticity = -Random.Range(7, 10);
                    answerDeltaVolici = Random.Range(7, 10);
                    answerTextCS = TextFromLine(answerLineCS, 1);
                    break;
                case "N": // neutral
                    answerDeltaAuthenticity = Random.Range(3, 6);
                    answerDeltaVolici = Random.Range(3, 6);
                    answerTextCS = TextFromLine(answerLineCS, 1);
                    break;
                case "C": // custom
                    answerDeltaAuthenticity = int.Parse(answerLineCS[1]);
                    answerDeltaVolici = int.Parse(answerLineCS[2]);
                    answerTextCS = TextFromLine(answerLineCS, 3);
                    break;
                default:
                    break;
            }

            return new Answer(answerDeltaAuthenticity, answerDeltaVolici, answerTextEN, answerTextCS, answerType);
        }

        Question? LoadQuestion(StreamReader reader)
        {
            string[] questionLineCS = ReadUntilSpecialString(reader, questionSpecialString);
            string[] questionLineEN = ReadUntilSpecialString(reader, englishSpecialString);
            if (questionLineCS == null)
                return null; // end of stream

            string questionTextCS = TextFromLine(questionLineCS, 1);
            string questionTextEN = TextFromLine(questionLineEN, 1);

            List<Answer> answers = new();

            while (true)
            {
                Answer? answer = LoadAnswer(reader);
                if (answer is null)
                    break;
                answers.Add(answer);
            }

            return new Question(questionTextEN, questionTextCS, QuestionType.General, answers);
        }

        using (StreamReader reader = new StreamReader(_questionsFilePath))
        {
            while (!reader.EndOfStream)
            {
                Question? question = LoadQuestion(reader);
                if (question is null)
                    break; // end of stream

                _questions.Add(question);
            }
        }
    }

    private static void ResetQuestions()
    {
        foreach (Question question in _questions)
        {
            question.ResetAnswers();
        }
    }

    // this should be called at the start of the game to get 4 random general questions
    public static Question[] GetRandomQuestions(int count = 4)
    {
        ResetQuestions();
        _questions.Shuffle();
        return _questions.Take(4).ToArray();
    }

    public static Question[] GetQuestionsForCandidate(Candidate candidate, int count = 3)
    {
        Question[] questions = new Question[count];
        questions[0] = candidate.SpecialSkill.GetQuestion();
        int addedQuestions = 1;

        foreach (Property property in candidate.BadProperties)
        {
            if (property.GetQuestion() is not null)
            {
                questions[addedQuestions++] = property.GetQuestion();
                if (addedQuestions == count)
                    break;
            }
        }

        questions.Shuffle();
        return questions;
    }

    public static Question GetFinalQuestion() {
        string questionTextEN = "As we conclude this debate, each candidate now has the chance for final thoughts or comments. Is there anything you'd like to share with the audience and voters before we finish?";
        string questionTextCS = "Na závěr této debaty má každý z kandidátů možnost vyjádřit své závěrečné myšlenky a připomínky. Je něco, o co byste se chtěli podělit s publikem a voliči, než skončíme?";

        string[] answerTextsEN = new string[] {
            "In these challenging times, it is more important than ever that we come together as a nation. Let's unite for a better future in Callibristan.",
            "I believe tonight showcased the strength of our vision for the country. With your support, we'll make positive changes for all.",
            "Regardless of your political affiliation, I urge everyone to participate in the election. Your voice matters, and together, we can shape the future of our nation.",
            "This debate is just one step in a long journey. I'm excited about the upcoming opportunities to engage with the voters."
        };

        string[] answerTextsCS = new string[] {
            "V této náročné době je důležitější než kdy jindy, abychom se jako národ spojili. I přes naše rozdíly máme společný cíl - lepší budoucnost pro Callibristán.",
            "Věřím, že dnešní večer ukázal sílu naší vize pro tento národ. Jsem přesvědčen, že s vaší podporou můžeme dosáhnout pozitivních změn pro všechny naše občany.",
            "Bez ohledu na vaši politickou příslušnost vyzývám každého, aby se voleb zúčastnil. Na vašem hlase záleží a společně můžeme utvářet budoucnost našeho národa.",
            "Tato debata je jen jedním z kroků na dlouhé cestě. Těším se na nadcházející příležitosti, kdy budu moci opět navázat kontakt s voliči."
        };

        List<Answer> answers = new();
        for (int i = 0; i < answerTextsCS.Length; i++) {
            answers.Add(new Answer(3, 0, answerTextsEN[i], answerTextsCS[i], AnswerType.Neutral) );
        }

        return new Question(questionTextEN, questionTextCS, QuestionType.General, answers);
    }

    void Awake() { LoadQuestionsFromFile(); }
}
