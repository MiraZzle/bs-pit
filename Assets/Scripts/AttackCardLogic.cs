using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackCardLogic : MonoBehaviour
{
    [SerializeField]
    Image stamp;

    [SerializeField]
    GameObject attackCard;

    [SerializeField]
    Sprite approvedStampSprite;
    [SerializeField]
    Sprite deniedStampSprite;

    CardManager cardManager;
    DebateManager debateManager;
    SoundManager soundManager;

    Card _card;
    public void SetCard(Card card) {
        _card = card;
        attackCard.GetComponent<Image>().sprite = card.Sprite;
    }


    private bool _isUsed = false;
    public void ShowCard() {
        attackCard.SetActive(true);
        if (_isUsed) ShowStamp();
    }
    public void HideCard() {
        attackCard.SetActive(false);
        if (_isUsed) HideStamp();
    }
    private void ShowStamp() {
        stamp.gameObject.SetActive(true);

    }
    private void HideStamp() {
        stamp.gameObject.SetActive(false);

    }

    private void Start() {
        debateManager = GameObject.FindGameObjectWithTag("logic").GetComponent<DebateManager>();
        cardManager = GameObject.FindGameObjectWithTag("logic").GetComponent<CardManager>();
        soundManager = GameObject.FindGameObjectWithTag("sound").GetComponent<SoundManager>();

        HideCard();
        HideStamp();
    }


    public void CardOnClick() {
        soundManager.PlayMouseClickSE();
        attackCard.GetComponent<Button>().interactable = false;
        _isUsed = true;
        TurnManager.CardAnimationPlaying = true;
        StartCoroutine(UseCardAnimation());
    }

    bool PressedContinue() => Input.GetKeyDown(KeyCode.Space);


    bool _transitionAnimation = false;
    bool _stampAnimation = false;


    IEnumerator UseCardAnimation() {
        float animationDelay = 0.2f;
        cardManager.HideAllCardsExcept(this);
        yield return new WaitForSeconds(animationDelay);

        Vector3 originalCardPositon = attackCard.transform.position;
        
        // move the card to center
        _transitionAnimation = true;
        yield return new WaitUntil(() => !_transitionAnimation);
        yield return new WaitForSeconds(2*animationDelay);
        // the card is now centered

        // set stamp
        bool playerWon = debateManager.DecidePlayerWin(_card);
        stamp.sprite = playerWon ? approvedStampSprite : deniedStampSprite;
        SetUpStampAnimation();
        _stampAnimation = true;
        soundManager.PlayStampSE();
        yield return new WaitUntil(() => !_stampAnimation);
        //yield return new WaitUntil(() => PressedContinue());

        yield return new WaitForSeconds(3 * animationDelay);
        HideCard();
        debateManager.UpdateAuthenticityAndVoters(_card, playerWon);
        yield return new WaitForSeconds(3 * animationDelay);

        attackCard.transform.position = originalCardPositon;
        HandleCard();
    }

    private void HandleCard() {
        Button cardButton = attackCard.GetComponent<Button>();
        cardButton.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1f);
        TurnManager.CardAnimationPlaying = false;
        TurnManager.CardHandled();
    }

    const float _minScale = 3.1f;
    const float _maxScale = 24;
    const float _stampAnimationDuration = 0.4f;

    private void SetUpStampAnimation() {
        stamp.transform.localScale = new Vector3(_maxScale, _maxScale, _maxScale);
        ShowStamp();
    }

    const float _transitionSpeed = 150f / 0.2f;

    private void Update() {
        stamp.transform.position = attackCard.transform.position;

        if (_transitionAnimation) {
            Vector3 cardPos = attackCard.GetComponent<RectTransform>().localPosition;
            float dist = _transitionSpeed * Time.deltaTime;
            float cardX = cardPos.x;
            float newCardX = (cardX > 0) ? cardX - dist: cardX + dist;

            // if the card moved further from the center
            if (Mathf.Abs(newCardX - 0) >= Mathf.Abs(cardX - 0)) {
                newCardX = 0;
                _transitionAnimation = false;
            }
            cardPos.x = newCardX;
            attackCard.GetComponent<RectTransform>().localPosition = cardPos;
        }

        if (_stampAnimation) {
            float speed = (_maxScale - _minScale) / _stampAnimationDuration;
            float dist = Time.deltaTime * speed;

            float currentScale = stamp.transform.localScale.x;
            float newScale = currentScale - dist;
            if (newScale <= _minScale) {
                newScale = _minScale;
                _stampAnimation = false;
            }
            stamp.transform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }
}
