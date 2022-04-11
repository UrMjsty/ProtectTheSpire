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
    }
    public void Use(Card card)
    {
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
                    HM.DealDamage(card.SpellValue / 2, BM.isPlayerTurn);
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
}
