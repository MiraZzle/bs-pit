// Ignore Spelling: Volici
#nullable enable

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public enum AnswerType {
    Populist, Neutral
}

static class MyExtensions {
    public static void Shuffle<T>(this IList<T> list) {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = Random.Range(0, n);
            (list[k], list[n]) = (list[k], list[n]);
        }
    }
}

public record Answer {
    public int DeltaAuthenticity { get; private set; }
    public int DeltaVolici { get; private set; }
    public string TextEN { get; private set; }
    public string TextCS { get; private set; }

    public AnswerType Type { get; private set; }

    public bool IsUsed = false;
    public Answer(int deltaAuthenticity, int deltaVolici, string textEN, string textCS, AnswerType type) {
        DeltaAuthenticity = deltaAuthenticity;
        DeltaVolici = deltaVolici;
        TextEN = textEN;
        TextCS = textCS;
        Type = type;
    }
}

public record Question {
    public string TextEN { get; private set; }
    public string TextCS { get; private set; }

    private List<Answer> _populistAnswers = new();
    private List<Answer> _neutralAnswers = new();

    public Question(string textEN, string textCS, List<Answer> answers) {
        TextEN = textEN;
        TextCS = textCS;

        foreach (Answer answer in answers) {
            if (answer.Type == AnswerType.Neutral) _neutralAnswers.Add(answer);
            if (answer.Type == AnswerType.Populist) _populistAnswers.Add(answer);
        }
    }

    private void ShuffleAnswers() {
        _populistAnswers.Shuffle();
        _neutralAnswers.Shuffle();
    }

    public Answer[] GetAnswers() {
        ShuffleAnswers();

        Answer[] answers = new Answer[3];
        // get 1 populist answer
        foreach (Answer answer in _populistAnswers ) {
            if (!answer.IsUsed) {
                answers[0] = answer;
                break;
            }
        }

        // fill with normal answers
        foreach (Answer answer in _neutralAnswers ) {
            if (!answer.IsUsed) {
                answers[answers.Length] = answer;
                if (answers.Length == 3) break;
            }
        }

        return answers;
    }

    public void ResetAnswers() {
        foreach (var answer in _populistAnswers) answer.IsUsed = false;
        foreach (var answer in _neutralAnswers) answer.IsUsed = false;
    }
}

public class QuestionLoader : MonoBehaviour
{
    readonly string _questionsFilePath = Path.Combine(Application.streamingAssetsPath, "general-questions.txt");

    List<Question> _questions = new List<Question>();

    string[] questionSpecialString = { "O" };
    string[] englishSpecialString = { "E" };
    string[] answerSpecialStrings = { "P", "N", "C" };

    void LoadQuestionsFromFile() {
        string TextFromLine(string[] line, int textStartIndex) {
            StringBuilder text = new StringBuilder();
            for (int i = textStartIndex; i < line.Length-1; i++) {
                text.Append(line[i] + " ");
            }
            text.Append(line[line.Length-1]);
            return text.ToString();
        }

        string[] ReadUntilSpecialString(StreamReader reader, string[] specialStrings, string terminator = "END") {
            while (!reader.EndOfStream) {
                string[] line = reader.ReadLine().Split(new char[] { ' ', '\t' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (line.Length == 0) continue;
                if (specialStrings.Contains(line[0])) return line;
                if (line[0] == terminator) return line;
            }
            return null;
        }

        Answer? LoadAnswer(StreamReader reader) {
            string[] answerLineCS = ReadUntilSpecialString(reader, answerSpecialStrings);
            if (answerLineCS[0] == "END") return null;  // end of question

            string[] answerLineEN = ReadUntilSpecialString(reader, englishSpecialString);

            string answerTextCS = null;
            string answerTextEN = TextFromLine(answerLineEN, 1);
            int answerDeltaAuthenticity = 0;
            int answerDeltaVolici = 0;
            AnswerType answerType = (answerLineCS[0] == "P") ? AnswerType.Populist : AnswerType.Neutral;

            switch (answerLineCS[0]) {
                case "P":  // populist
                    answerDeltaAuthenticity = -Random.Range(7, 10);
                    answerDeltaVolici = Random.Range(7, 10);
                    answerTextCS = TextFromLine(answerLineCS, 1);
                    break;
                case "N":  // neutral
                    answerDeltaAuthenticity = Random.Range(3, 6);
                    answerDeltaVolici = Random.Range(3, 6);
                    answerTextCS = TextFromLine(answerLineCS, 1);
                    break;
                case "C":  // custom
                    answerDeltaAuthenticity = int.Parse(answerLineCS[1]);
                    answerDeltaVolici = int.Parse(answerLineCS[2]);
                    answerTextCS = TextFromLine(answerLineCS, 3);
                    break;
                default:
                    break;
            }

            return new Answer(answerDeltaAuthenticity, answerDeltaVolici, answerTextEN, answerTextCS, answerType);
        }

        Question? LoadQuestion(StreamReader reader) {
            string[] questionLineCS = ReadUntilSpecialString(reader, questionSpecialString);
            string[] questionLineEN = ReadUntilSpecialString(reader, englishSpecialString);
            if (questionLineCS == null) return null;  // end of stream

            string questionTextCS = TextFromLine(questionLineCS, 1);
            string questionTextEN = TextFromLine(questionLineEN, 1);

            List<Answer> answers = new();

            while (true) {
                Answer? answer = LoadAnswer(reader);
                if (answer is null) break;
                answers.Add(answer);
            }

            return new Question(questionTextEN, questionTextCS, answers);
        }


        using (StreamReader reader = new StreamReader(_questionsFilePath)) {
            while (!reader.EndOfStream) {
                Question? question = LoadQuestion(reader);
                if (question is null) break;  // end of stream

                _questions.Add(question);
            }
        }
    }

    private void ResetQuestions() {
        foreach (Question question in _questions) {
            question.ResetAnswers();
        }
    }

    // this should be called at the start of the game to get 4 random general questions
    public Question[] GetRandomQuestions(int count = 4) {
        ResetQuestions();
        _questions.Shuffle();
        return _questions.Take(4).ToArray();
    }

    void Start()
    {
        LoadQuestionsFromFile();
    }
}