using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardInfo : MonoBehaviour, IPointerClickHandler
{
    public DeckManager DM;
    public Card SelfCard { get; private set; }
    private Transform Back { get; set; }
    [SerializeField] private Image logo;
    [SerializeField] public Image enlight;
    [SerializeField] public Image scroll;
    [SerializeField]private Transform CardBack { get; set; }
    [SerializeField]private Transform Image1 { get; set; }
    [SerializeField] private Text cardName;
    [SerializeField] private TextMeshProUGUI description;
    public GameObject cardGo;
    private readonly Color32 _attackColor = new Color32(172, 50, 50, 150),
                    _healColor = new Color32(106, 190, 48, 150),
                    _protectColor = new Color32(95, 208, 228, 150);
    

    public void HideCardInfo(Card card)
    {
        CardBack.gameObject.SetActive(true);
        scroll.gameObject.SetActive(false);
        Image1.GetComponent<Image>().color = Color.clear;
        Back.GetComponent<Image>().color = Color.clear;
        SelfCard = card;
        //ShowCardInfo(card);
        logo.sprite = Resources.Load<Sprite>("sprites/cards/cardBack");
        cardName.text = "";
        description.text = "";
    }
    public void ShowCardInfo(Card card, GameObject cardGO)
    {
        SelfCard = card;
        logo.sprite = card.Logo;
        logo.preserveAspect = true;
        cardName.text = card.Name;
        description.text = card.Description;
        ShowDescription();
        ShowColor(cardGO, card);
        CardBack.gameObject.SetActive(false);
        Back.GetComponent<Image>().color = Color.white;
    }
    private void ShowDescription()
    {
        switch (SelfCard.Type)
        {
            case Card.CardType.ATTACK:
                description.text = $"Attack {SelfCard.Value}X Times\n({SelfCard.Owner.GetDamage()} Damage)";
                break;
            case Card.CardType.HEAL:
                description.text = $"Restore {SelfCard.Value} Health";
                break;
            case Card.CardType.PROTECT:
                description.text = $"Defend {SelfCard.Value}X Times\n({SelfCard.Owner.GetArmorUp()}Armor)";
                break;
        }
        foreach (var ability in SelfCard.Abilities)
        {
            switch (ability)
            {
                case Card.AbilityType.LIFESTEAL:
                    description.text += "\n Lifesteal";
                    break;
                case Card.AbilityType.BERSERK:
                    description.text += "\n BERSERK";
                    break;
                case Card.AbilityType.DRAW:
                    description.text += "\n Draw Card";
                    break;
                case Card.AbilityType.DISCARD:
                    description.text += "\n Discard Card";
                    break;
            }
        }
    }

    private void ShowColor(GameObject cardGO, Card card)
    {
        Back = cardGO.transform;
        Image1 = cardGO.transform.GetChild(1);
            CardBack = cardGO.transform.GetChild(6);
        switch (card.Type)
        {
            case Card.CardType.ATTACK:
                Image1.GetComponent<Image>().color = _attackColor;
                break;
            case Card.CardType.HEAL:
                Image1.GetComponent<Image>().color = _healColor;
                break;
            case Card.CardType.PROTECT:
                Image1.GetComponent<Image>().color = _protectColor;
                break;
        }
    }
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        DM.SetChosenCard(gameObject);
    }
   
}
