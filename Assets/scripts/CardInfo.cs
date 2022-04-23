using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInfo : MonoBehaviour
{
    public Card SelfCard;
    public Transform back;
    public Image Logo;
    public Transform cardBack;
    public Transform image;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public GameObject cardGO;
    private Color32 attackColor = new Color32(172, 50, 50, 150),
                    healColor = new Color32(106, 190, 48, 150),
                    protectColor = new Color32(95, 208, 228, 150);

    public void HideCardInfo(Card card)
    {
        cardBack.gameObject.SetActive(true);
        image.GetComponent<Image>().color = Color.clear;
        back.GetComponent<Image>().color = Color.clear;
        SelfCard = card;
        //ShowCardInfo(card);
        Logo.sprite = Resources.Load<Sprite>("sprites/cards/cardBack");
        Name.text = "";
        Description.text = "";
    }
    public void ShowCardInfo(Card card, GameObject cardGO)
    {
        
        SelfCard = card;
        Logo.sprite = card.Logo;
        Logo.preserveAspect = true;
        Name.text = card.Name;
        Description.text = card.Description;
        ShowDescription(card);
        SetColor(cardGO, card);
        cardBack.gameObject.SetActive(false);
        back.GetComponent<Image>().color = Color.white;
    }
    private void ShowDescription(Card card)
    {
        switch (card.Type)
        {
            case Card.CardType.ATTACK:
                Description.text = $"Deal {card.SpellValue} Damage ";
                break;
            case Card.CardType.HEAL:
                Description.text = $"Restore {card.SpellValue} Health";
                break;
            case Card.CardType.PROTECT:
                Description.text = $"Gain {card.SpellValue} Armor";
                break;
        }
        foreach (var ability in card.abilities)
        {
            switch (ability)
            {
                case Card.AbilityType.LIFESTEAL:
                    Description.text += "\n Lifesteal";
                    break;
                case Card.AbilityType.BERSERK:
                    Description.text += "\n BERSERK";
                    break;
                case Card.AbilityType.DRAW:
                    Description.text += "\n Draw Card";
                    break;
                case Card.AbilityType.DISCARD:
                    Description.text += "\n Discard Card";
                    break;
                default:
                    break;
            }
        }
    }
    public void SetColor(GameObject cardgo, Card card)
    {
        back = cardgo.transform;
        image = cardgo.transform.GetChild(0);
        cardBack = cardgo.transform.GetChild(5);
        // cardgo.transform.childcount == 5
        switch (card.Type)
        {
            case Card.CardType.ATTACK:
                image.GetComponent<Image>().color = attackColor;
                break;
            case Card.CardType.HEAL:
                image.GetComponent<Image>().color = healColor;
                break;
            case Card.CardType.PROTECT:
                image.GetComponent<Image>().color = protectColor;
                break;
            default:
                break;
        }
    }
    public void Clutch()
    {

    }
    private void Start()
    { 
       
    }
}
