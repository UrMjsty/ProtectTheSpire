using System.Collections;
using System.Collections.Generic;
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
    public string Name;
    public string Description;
    public Sprite Logo;
    public int SpellValue;
    public CardType Type;
    public List<AbilityType> abilities;

    public Card(string name, string logoPath, int spellValue, 
                CardType cardType = 0, AbilityType stAbility = 0, AbilityType ndAbility = 0, string description = "kek")
    {
        Name = name;
        Description = description;
        Logo = Resources.Load<Sprite>(logoPath);
        SpellValue = spellValue;
        Type = cardType;
        abilities = new List<AbilityType>();
        if (stAbility != 0)
            abilities.Add(stAbility);
        if (ndAbility != 0)
            abilities.Add(ndAbility);
    } 
}
public static class CardManagerClass
{
    public static List<Card> AllCards = new List<Card>();

    public static List<Card> StartingDeck = new List<Card>();
    public static List<Card> DummyDeck = new List<Card>();
    public static List<Card> GoblinDeck = new List<Card>();
    public static List<Card> RogueDeck = new List<Card>();
    public static List<Card> BatDeck = new List<Card>();
}
public class CardManager : MonoBehaviour
{
    private HealthManager HM;
    private BattleManager BM;
    public bool isPlayerCard;
    public CardInfo cardInfo;
    public CardMovement cardMovement;
    private void Start()
    {
        HM = FindObjectOfType<HealthManager>();
        BM = FindObjectOfType<BattleManager>();
    }
    public void Awake()
    {
        CardManagerClass.AllCards.Add(new Card("Punch", "sprites/cards/punch", 1, Card.CardType.ATTACK));
        CardManagerClass.AllCards.Add(new Card("Minor Heal", "sprites/cards/minorHeal", 2, Card.CardType.HEAL));
        CardManagerClass.AllCards.Add(new Card("Small Shield", "sprites/cards/smallShield", 1, Card.CardType.PROTECT));
        CardManagerClass.AllCards.Add(new Card("BERSERK", "sprites/cards/berserk", 6, Card.CardType.ATTACK, Card.AbilityType.BERSERK));
        //CardManagerClass.AllCards.Add(new Card("Vampire Bite", "sprites/cards/vampireBite", 2, Card.CardType.ATTACK, Card.AbilityType.LIFESTEAL));
        CardManagerClass.AllCards.Add(new Card("Bat Bite", "sprites/cards/vampireBite", 1, Card.CardType.ATTACK, Card.AbilityType.LIFESTEAL));
        //CardManagerClass.AllCards.Add(new Card("Poison Potion", "sprites/cards/poisonPotion", 5, Card.CardType.HEAL, Card.AbilityType.DISCARD));
        CardManagerClass.AllCards.Add(new Card("Fast Shield", "sprites/cards/fastShield", 1, Card.CardType.PROTECT, Card.AbilityType.DRAW));



        CardManagerClass.StartingDeck = Starting();
        CardManagerClass.DummyDeck = Dummy();
        CardManagerClass.RogueDeck = Rogue();
        CardManagerClass.GoblinDeck = Goblin();
        CardManagerClass.BatDeck = Bat();
    }
    public void Use(GameObject cardgo, float time)
    {
        Card card = cardgo.GetComponent<CardInfo>().SelfCard;
        cardgo.GetComponent<CardInfo>().ShowCardInfo(card, cardgo);
        switch (card.Type)
        {
            case Card.CardType.ATTACK:
                HM.DealDamage(card.SpellValue, BM.isPlayerTurn);
                break;
            case Card.CardType.HEAL:
                HM.RestoreHealth(card.SpellValue, BM.isPlayerTurn);
                break;
            case Card.CardType.PROTECT:
                HM.GainArmor(card.SpellValue, BM.isPlayerTurn);
                break;
            default:
                break;
        }
        foreach (var ability in card.abilities)
        {
            switch (ability)
            {
                case Card.AbilityType.LIFESTEAL:
                    HM.RestoreHealth(card.SpellValue, BM.isPlayerTurn);
                    break;
                case Card.AbilityType.BERSERK:
                    HM.DealDamage(1, !BM.isPlayerTurn);
                    break;
                case Card.AbilityType.DRAW:
                    BM.DrawCard();
                    break;
                case Card.AbilityType.DISCARD:
                    BM.DiscardCard();
                    break;
            }
        }
      // Debug.Log("lol why");
        Destroy(cardgo, time);
    }
    public List<Card> Starting()
    {
        var deck = new List<Card>();
        deck.Add(CardManagerClass.AllCards[0]);
        deck.Add(CardManagerClass.AllCards[0]);
        deck.Add(CardManagerClass.AllCards[0]);
        deck.Add(CardManagerClass.AllCards[2]);
        deck.Add(CardManagerClass.AllCards[2]);
        deck.Add(CardManagerClass.AllCards[1]);
        return deck;
    }
    public List<Card> Dummy()
    {
        var deck = new List<Card>();
        deck.Add(CardManagerClass.AllCards[2]);
        deck.Add(CardManagerClass.AllCards[2]);
        return deck;
    }
    public List<Card> Goblin()
    {
        var deck = new List<Card>();
        deck.Add(CardManagerClass.AllCards[0]);
        return deck;
    }
    public List<Card> Rogue()
    {
        var deck = new List<Card>();
        deck.Add(CardManagerClass.AllCards[0]);
        deck.Add(CardManagerClass.AllCards[0]);
        deck.Add(CardManagerClass.AllCards[1]);
        return deck;
    }
    public List<Card> Bat()
    {
        var deck = new List<Card>();
        deck.Add(CardManagerClass.AllCards[4]);
        return deck;
    }
}
