using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;


public enum CardType {
    Commie, Lover, Fascist, Putin, Drunkard, Corrupt, Murderer
}


public class Card {
    public int VoliciDelta { get; private set; }
    public int AuthenticityDelta { get; private set; }
    public Sprite Sprite { get; private set; }

    private CardType _type;
    public Card(Sprite sprite, CardType type, int voliciDelta = 10, int authenticityDelta = 10) {
        Sprite = sprite;
        _type = type;
        VoliciDelta = voliciDelta;
        AuthenticityDelta = authenticityDelta;
    }

    public bool IsRelevantToProperty(Property property) => _relevantProperties[(int)_type].Contains(property.Type);

    // pole, kde je pro kazdy typ karty seznam relevantnich vlastnosti
    static List<PropertyType>[] _relevantProperties = new List<PropertyType>[Enum.GetNames(typeof(CardType)).Length];

    public static void SetUpCards() {
        _relevantProperties[(int)CardType.Commie] = new() { PropertyType.Commie, PropertyType.ProRussian };
        _relevantProperties[(int)CardType.Lover] = new() { PropertyType.SSKollar };
        _relevantProperties[(int)CardType.Fascist] = new() { PropertyType.Fasist };
        _relevantProperties[(int)CardType.Putin] = new() { PropertyType.Commie, PropertyType.ProRussian };
        _relevantProperties[(int)CardType.Drunkard] = new() { PropertyType.SSAligator };
        _relevantProperties[(int)CardType.Corrupt] = new() { PropertyType.Corrupt, PropertyType.SSCapiHnizdo };
        _relevantProperties[(int)CardType.Murderer] = new() { PropertyType.SSKuciak, PropertyType.SSFlakanec };
    }
}


public class CardManager : MonoBehaviour {
    const int numCardsInGame = 7;

    // Commie, Lover, Fascist, Putin, Thief, Corrupt, Murderer
    [SerializeField]
    Sprite[] CardSprites = new Sprite[numCardsInGame];

    Card[] cards = new Card[numCardsInGame];

    private void CreateCards() {
        for (int i = 0; i < numCardsInGame; i++) {
            cards[i] = new Card(CardSprites[i], (CardType)i);
        }
    }

    public Card[] GetRandomCards(int num = 4) {
        num = Mathf.Clamp(num, 0, numCardsInGame);
        cards.Shuffle();
        return cards.Take(num).ToArray();
    }

    void Start()
    {
        CreateCards();
        Card.SetUpCards();
    }
}
