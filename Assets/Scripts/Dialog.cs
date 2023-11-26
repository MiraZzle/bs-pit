using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Modified
// https://github.com/Maraakis/ChristinaCreatesGames/blob/main/Text%20Mesh%20Pro%20Typewriter/TypewriterEffect.cs
public class Dialog : MonoBehaviour
{
    [Header("Dialog Settings")]
    [SerializeField, Min(11)]
    private float charactersPerSecond = 20;
    [SerializeField]
    private float interpunctuationDelay = 0.5f;

    private int _currVisIndex;
    private Coroutine _typewriterCoroutine;
    private bool _readyForNew = true;

    private WaitForSeconds _normalDelay;
    private WaitForSeconds _interpunctuationDelay;

    public bool Skip = false;
    public Action OnTypewriterEnd;

    private TMP_Text _textBox;
    [Header("Dialog Object")]
    [SerializeField]
    private GameObject dialogObj;

    [Header("Dialog Sound")]
    [SerializeField]
    private AudioClip sound;
    [SerializeField]
    private SoundManager soundManager;

    public bool IsActive
    {
        get => !_readyForNew;
    }

    public void ReadNext(string message)
    {
        SetText(message);
        ResetProps();
        PrepareNewText();
    }

    public void Show()
    {
        dialogObj.SetActive(true);
        ResetProps();
        PrepareNewText();
    }
    public void Hide()
    {
        dialogObj.SetActive(false);
        if (_typewriterCoroutine is not null)
        {
            StopCoroutine(_typewriterCoroutine);
        }
    }
    public void SetText(string text)
    {
        if (_textBox is null)
        {
            _textBox = GetComponent<TMP_Text>();
        }
        ResetProps();
        _textBox.text = text;
    }

    void ResetProps()
    {
        _textBox.maxVisibleCharacters = 0;
        _currVisIndex = 0;
        Skip = false;
    }

    void Awake()
    {
        _textBox = GetComponent<TMP_Text>();
        if (dialogObj is null)
        {
            dialogObj = gameObject;
        }

        float delay = 1 / charactersPerSecond;
        _normalDelay = new WaitForSeconds(delay);
        _interpunctuationDelay = new WaitForSeconds(interpunctuationDelay);
    }

    private void PrepareNewText()
    {
        if (!_readyForNew || _textBox.maxVisibleCharacters >= _textBox.text.Length)
        {
            return;
        }

        _readyForNew = false;
        if (_typewriterCoroutine is not null)
        {
            StopCoroutine(_typewriterCoroutine);
        }
        ResetProps();
        _typewriterCoroutine = StartCoroutine(TypewriterAnimation());
    }

    private IEnumerator TypewriterAnimation()
    {
        yield return _normalDelay;

        var info = _textBox.textInfo;
        while (_currVisIndex < _textBox.text.Length)
        {
            var lastIndex = info.characterInfo.Length - 1;
            if (_currVisIndex >= lastIndex)
            {
                _textBox.maxVisibleCharacters++;
                yield return new WaitForSeconds(0.2f);
                Handle();
                yield break;
            }

            char character = info.characterInfo[_currVisIndex].character;
            _textBox.maxVisibleCharacters++;

            if (!Skip)
            {
                if (character == '?' || character == '.' || character == ';' || character == '!')
                {
                    yield return _interpunctuationDelay;
                }
                else
                {
                    yield return _normalDelay;
                }

                if (soundManager is not null)
                {
                    if (!soundManager.IsPlaying() && Char.IsLetterOrDigit(character))
                    {
                        soundManager.Stop();
                        soundManager.PlaySound(sound);
                    }
                }
            }

            _currVisIndex++;
        }
        yield return new WaitForSeconds(0.2f);
        Handle();
    }

    private void Handle()
    {
        _readyForNew = true;
        OnTypewriterEnd?.Invoke();
    }
}
