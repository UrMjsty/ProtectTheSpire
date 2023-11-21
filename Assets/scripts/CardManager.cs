using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public struct Card
{
    public enum CardType
    {
        ATTACK,
        HEAL,
        PROTECT
    }
    public enum AbilityType
    {
        EMPTY,
        BERSERK,
        DRAW,
        DISCARD,
        LIFESTEAL
    }
    public readonly string Name;
    public readonly string Description;
    public readonly Sprite Logo;
    public readonly int Value;
    public readonly CardType Type;
    public readonly List<AbilityType> Abilities;
    public Character Owner;

    public Card(string name, string logoPath, int value, 
                CardType cardType = 0, AbilityType stAbility = 0, AbilityType ndAbility = 0, string description = "")
    {
        Name = name;
        Description = description;
        Logo = Resources.Load<Sprite>(logoPath);
        Value = value;
        Type = cardType;
        Abilities = new List<AbilityType>();
        if (stAbility != 0)
            Abilities.Add(stAbility);
        if (ndAbility != 0)
            Abilities.Add(ndAbility);
        Owner = null;
    }

 
}
public static class CardManagerClass
{
    public static readonly List<Card> AllCards = new List<Card>();
    
    public static List<Card> StartingDeck;
    public static List<Card> DummyDeck;
    public static List<Card> GoblinDeck;
    public static List<Card> RogueDeck;
    public static List<Card> BatDeck;
}
public class CardManager : MonoBehaviour
{
    [SuppressMessage("ReSharper", "InconsistentNaming")] private HealthManager HM;
    [SuppressMessage("ReSharper", "InconsistentNaming")] private BattleManager BM;
    public bool isPlayerCard;
    public CardInfo cardInfo;
    public CardMovement cardMovement;
    private Character User => BM.GetCurrentCharacter();
    private void Start()
    {
        HM = FindObjectOfType<HealthManager>();
        BM = FindObjectOfType<BattleManager>();
    }
    public void Awake()
    {
        CardManagerClass.AllCards.Add(new Card("Punch", "sprites/cards/punch", 1, Card.CardType.ATTACK));
        CardManagerClass.AllCards.Add(new Card("Minor Heal", "sprites/cards/minorHeal", 25, Card.CardType.HEAL));
        CardManagerClass.AllCards.Add(new Card("Small Shield", "sprites/cards/smallShield", 1, Card.CardType.PROTECT));
        CardManagerClass.AllCards.Add(new Card("BERSERK", "sprites/cards/berserk", 6, Card.CardType.ATTACK, Card.AbilityType.BERSERK));
        //CardManagerClass.AllCards.Add(new Card("Vampire Bite", "sprites/cards/vampireBite", 2, Card.CardType.ATTACK, Card.AbilityType.LIFESTEAL));
        CardManagerClass.AllCards.Add(new Card("Bat Bite", "sprites/cards/vampireBite", 1, Card.CardType.ATTACK, Card.AbilityType.LIFESTEAL));
        //CardManagerClass.AllCards.Add(new Card("Poison Potion", "sprites/cards/poisonPotion", 55, Card.CardType.HEAL, Card.AbilityType.DISCARD));
        CardManagerClass.AllCards.Add(new Card("Fast Shield", "sprites/cards/fastShield", 1, Card.CardType.PROTECT, Card.AbilityType.DRAW));


        var startDeckIndices = new List<int>() {0,0,0,1,2,2};
        var dummyDeckIndices = new List<int>() {2,2};
        var goblinDeckIndices = new List<int>() {0};
        var rogueDeckIndices = new List<int>() {0,0,1};
        var batDeckIndices = new List<int>() {4};
        CardManagerClass.StartingDeck = GetDeck(startDeckIndices);
        CardManagerClass.DummyDeck = GetDeck(dummyDeckIndices);
        CardManagerClass.RogueDeck = GetDeck(rogueDeckIndices);
        CardManagerClass.GoblinDeck = GetDeck(goblinDeckIndices);
        CardManagerClass.BatDeck = GetDeck(batDeckIndices);
    }
    public void Use(GameObject cardGO, float time)
    {
        Card card = cardGO.GetComponent<CardInfo>().SelfCard;
        cardGO.GetComponent<CardInfo>().ShowCardInfo(card, cardGO);
        switch (card.Type)
        {
            case Card.CardType.ATTACK:
                HM.TakeDamage(card.Value * User.Damage, User.Opponent);
                break;
            case Card.CardType.HEAL:
                HM.RestoreHealth(card.Value, User);
                break;
            case Card.CardType.PROTECT:
                HM.GainArmor(card.Value * User.ArmorUp, User);
                break;
        }
        foreach (var ability in card.Abilities)
        {
            switch (ability)
            {
                case Card.AbilityType.LIFESTEAL:
                    HM.RestoreHealth(card.Value * User.Damage / 2, User);
                    break;
                case Card.AbilityType.BERSERK:
                    HM.TakeDamage(1, User);
                    break;
                case Card.AbilityType.DRAW:
                    BM.DrawCard(User);
                    break;
                case Card.AbilityType.DISCARD:
                    BM.DiscardCard(User);
                    break;
            }
        }
        Destroy(cardGO, time);
    }

    private List<Card> GetDeck(List<int> indices)
    {
        var deck = new List<Card>();
        foreach (int index in indices)
        {
            deck.Add(CardManagerClass.AllCards[index]);
        }

        return deck;
    }
}
   
