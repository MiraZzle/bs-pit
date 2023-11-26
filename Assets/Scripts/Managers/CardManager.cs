using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    private CardType _type;
    public Card(Sprite sprite, CardType type, int voliciDelta = 10, int authenticityDelta = -10)
    {
        Sprite = sprite;
        _type = type;
        WinnerVoliciDelta = voliciDelta;
        LoserAuthenticityDelta = authenticityDelta;
    }

    public bool IsRelevantToProperty(PropertyType property) => _relevantProperties[(int)_type].Contains(property);

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
        _relevantProperties[(int)CardType.Thief] = new() { PropertyType.Kleptoman, PropertyType.Corrupt };
    }
}

public class CardManager : MonoBehaviour
{
    const int numCardsInGame = 8;

    // Commie, Lover, Fascist, Putin, Drunkard, Corrupt, Murderer, Thief
    [SerializeField]
    Sprite[] CardSpritesEN = new Sprite[numCardsInGame];
    [SerializeField]
    Sprite[] CardSpritesCS = new Sprite[numCardsInGame];

    Card[] _cardsEN = new Card[numCardsInGame];
    Card[] _cardsCS = new Card[numCardsInGame];

    private void CreateCards()
    {
        for (int i = 0; i < numCardsInGame; i++)
        {
            _cardsEN[i] = new Card(CardSpritesEN[i], (CardType)i);
            _cardsCS[i] = new Card(CardSpritesCS[i], (CardType)i);
        }
    }

    public Card[] GetRandomCards(int num = 4)
    {
        Card[] cardsToShuffle = (PlayerPrefs.GetString("language") == "english") ? _cardsEN : _cardsCS;
        num = Mathf.Clamp(num, 0, numCardsInGame);
        cardsToShuffle.Shuffle();
        return cardsToShuffle.Take(num).ToArray();
    }

    void Start()
    {
        CreateCards();
        Card.SetUpCards();
    }
}
