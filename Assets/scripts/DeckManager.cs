using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DeckManager : MonoBehaviour
{
    public Player PlayerObject;

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject buttonPrefab;

    [SerializeField] private Transform deck;
    [SerializeField] private Transform deckButtons;

    [SerializeField]private GameObject chosenCardGameObject;
    private CardInfo _chosenCardInfo;
    private GameManager GM;

    private void Start()
    {
        GM = FindObjectOfType<GameManager>();
        chosenCardGameObject = null;
    }

    public void SetChosenCard(GameObject cardGo)
    {
        if (chosenCardGameObject != null)
        {
            print(chosenCardGameObject.transform.GetChild(0).name);
            chosenCardGameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        chosenCardGameObject = cardGo;
       chosenCardGameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void UpdateDeck()
    {
        foreach (Card card in PlayerObject.Deck)
        {
            GameObject cardGameObject = Instantiate(cardPrefab, deck, false);
           // GameObject buttonGameObject = Instantiate(buttonPrefab, deckButtons, false);
            var card1 = card;
            card1.Owner = PlayerObject;
            var info = cardGameObject.GetComponent<CardInfo>();
            info.ShowCardInfo(card1, cardGameObject);
            info.DM = this;
            // cardGameObject.GetComponent<CardInfo>().ShowCardInfo(card1, cardGameObject);
        }
    }

    public void RemoveDeck()
    {
        var c = deck.childCount;
        Debug.Log(c.ToString());
        for (int i = 0; i < c; i++)
        {
            Destroy(deck.GetChild(i).gameObject);
        }
    }

    public void RemoveCard()
    {
        if(chosenCardGameObject == null)
            return;
        print(PlayerObject.Deck.Remove(chosenCardGameObject.GetComponent<CardInfo>().SelfCard).ToString());
        GM.ToggleDeckWindow();
    }  
    public void AddCard()
    {
        if(chosenCardGameObject == null)
            return;
        PlayerObject.Deck.Add(chosenCardGameObject.GetComponent<CardInfo>().SelfCard);
        GM.ToggleDeckWindow();
    }
}
