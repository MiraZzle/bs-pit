using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebageManager : MonoBehaviour
{
    private Question[] _generalQuestions;
    private Question[] _playerQuestions;
    private Question[] _enemyQuestions;

    public int PlayerAuthenticity { get; private set; }
    public int EnemyAuthenticity { get; private set; }

    [SerializeField]
    private GameObject _playerObject;
    private Candidate _player;


    [SerializeField]
    private GameObject _enemyObject;
    private Candidate _enemy;


    void Start()
    {
        _player = _playerObject.GetComponent<Candidate>();
        _enemy = _enemyObject.GetComponent<Candidate>();

        Debug.Log(_player == null);

        //_generalQuestions = QuestionLoader.GetRandomQuestions();
        //_playerQuestions = QuestionLoader.GetQuestionsForCandidate(_player);
        //_playerQuestions = QuestionLoader.GetQuestionsForCandidate(_enemy);

        //Debug.Log(_playerQuestions[0].Text);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
