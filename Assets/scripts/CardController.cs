using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    private HealthManager HM;
    private BattleManager BM;
    public bool isPlayerCard;
    public Card Card;
    public CardInfo cardInfo;
    public CardMovement cardMovement;
    // Start is called before the first frame update
    void Start()
    {
        HM = FindObjectOfType<HealthManager>();
        BM = FindObjectOfType<BattleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Init(Card card, bool isPC)
    {
        Card = card;
        isPlayerCard = isPC;
    }/*
    public void Use(Card card)
    {
        Character user = null;
        Character victim = null;
        if (BM.IsPlayerTurn)
        {
            user = 
        }
        switch (card.Type)
        {
            case Card.CardType.ATTACK:
                HM.TakeDamage(card.Value, BM.IsPlayerTurn);
                break;
            case Card.CardType.HEAL:
                HM.RestoreHealth(card.Value, BM.IsPlayerTurn);
                break;
            case Card.CardType.PROTECT:
                HM.GainArmor(card.Value, BM.IsPlayerTurn);
                break;
            default:
                break;
        }
        foreach (var ability in card.abilities)
        {
            switch (ability)
            {
                case Card.AbilityType.LIFESTEAL:
                    HM.RestoreHealth(card.Value, BM.IsPlayerTurn);
                    break;
                case Card.AbilityType.BERSERK:
                    HM.TakeDamage(card.Value / 2, BM.IsPlayerTurn);
                    break;
                case Card.AbilityType.DRAW:
                    BM.DrawCard();
                    break;
                case Card.AbilityType.DISCARD:
                    BM.DiscardCard();
                    break;
            }
        }
        
    }
    */
}
