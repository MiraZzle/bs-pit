using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CardType
{
    Commie,
    Lover,
    Fascist,
    Putin,
    Drunkard,
    Corrupt,
    Murderer,
    Thief
}

public class Card
{
    public int WinnerVoliciDelta { get; private set; }
    public int LoserAuthenticityDelta { get; private set; }
    public Sprite Sprite { get; private set; }

    public CardType Type { get; private set; }
    public Card(Sprite sprite, CardType type, int voliciDelta = 10, int authenticityDelta = -10)
    {
        Sprite = sprite;
        Type = type;
        WinnerVoliciDelta = voliciDelta;
        LoserAuthenticityDelta = authenticityDelta;
    }

    public bool IsRelevantToProperty(PropertyType property) => _relevantProperties[(int)Type].Contains(property);

    // pole, kde je pro kazdy typ karty seznam relevantnich vlastnosti
    static List<PropertyType>[] _relevantProperties = new List<PropertyType>[Enum.GetNames(typeof(CardType)).Length];

    public static void SetUpCards()
    {
        _relevantProperties[(int)CardType.Commie] = new() { PropertyType.Commie, PropertyType.ProRussian };
        _relevantProperties[(int)CardType.Lover] = new() { PropertyType.SSKollar };
        _relevantProperties[(int)CardType.Fascist] = new() { PropertyType.Fascist };
        _relevantProperties[(int)CardType.Putin] = new() { PropertyType.Commie, PropertyType.ProRussian };
        _relevantProperties[(int)CardType.Drunkard] = new() { PropertyType.Drunkard, PropertyType.SSAligator };
        _relevantProperties[(int)CardType.Corrupt] = new() { PropertyType.Corrupt, PropertyType.SSCapiHnizdo };
        _relevantProperties[(int)CardType.Murderer] = new() { PropertyType.SSKuciak, PropertyType.SSFlakanec };
        _relevantProperties[(int)CardType.Thief] = new() { PropertyType.Kleptoman, PropertyType.Corrupt, PropertyType.SSCapiHnizdo };
    }
}

public class CardManager : MonoBehaviour {
    const int numCardsInGame = 8;

    // Commie, Lover, Fascist, Putin, Drunkard, Corrupt, Murderer, Thief
    [SerializeField]
    Sprite[] CardSpritesEN = new Sprite[numCardsInGame];
    [SerializeField]
    Sprite[] CardSpritesCS = new Sprite[numCardsInGame];

    Card[] _cardsEN = new Card[numCardsInGame];
    Card[] _cardsCS = new Card[numCardsInGame];

    private void CreateCards() {
        for (int i = 0; i < numCardsInGame; i++) {
            _cardsEN[i] = new Card(CardSpritesEN[i], (CardType)i);
            _cardsCS[i] = new Card(CardSpritesCS[i], (CardType)i);
        }
    }

    public const int NumCards = 4;

    [SerializeField]
    AttackCardLogic[] gameObjectCards = new AttackCardLogic[NumCards];
    [SerializeField]
    TMP_Text pressSpaceToContinue;

    public void SetUpRandomCards()
    {
        Card[] cardsToShuffle = (PlayerPrefs.GetString("language") == "english") ? _cardsEN : _cardsCS;
        cardsToShuffle.Shuffle();

        for (int i = 0; i < NumCards; i++) {
            gameObjectCards[i].SetCard(cardsToShuffle[i]);
        }
    }

    public void HideCards() {
        pressSpaceToContinue.gameObject.SetActive(false);
        foreach (var card in gameObjectCards) {
            card.HideCard();
        }
    }
    public void HideAllCardsExcept(AttackCardLogic cardToStay) {
        pressSpaceToContinue.gameObject.SetActive(false);
        foreach (var card in gameObjectCards) {
            if (card == cardToStay) continue;
            card.HideCard();
        }
    }

    public void ShowCards() {
        pressSpaceToContinue.gameObject.SetActive(true);
        foreach (var card in gameObjectCards) {
            card.ShowCard();
        }
    }


    void Awake()
    {
        CreateCards();
        Card.SetUpCards();
    }
}
