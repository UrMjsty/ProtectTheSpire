using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Battle 
{
    public List<Card> EnemyDeck, PlayerDeck,
                      EnemyHand, PlayerHand,
                      RemainingEnemyDeck, RemainingPlayerDeck;
    private int deckSize = 4;
    public Battle()
    {
        EnemyDeck = GiveDeckCard();
        PlayerDeck = GiveDeckCard();
        //PlayerDeck = new List<Card>();
        //PlayerDeck.Add(CardManagerClass.AllCards[5]);
        //PlayerDeck.Add(CardManagerClass.AllCards[5]);
        //PlayerDeck.Add(CardManagerClass.AllCards[5]);
        //PlayerDeck.Add(CardManagerClass.AllCards[5]);
        //PlayerDeck.Add(CardManagerClass.AllCards[4]);
        //PlayerDeck.Add(CardManagerClass.AllCards[4]);
        //PlayerDeck.Add(CardManagerClass.AllCards[4]);
        //PlayerDeck.Add(CardManagerClass.AllCards[4]);

        EnemyDeck = new List<Card>();
        EnemyDeck.Add(CardManagerClass.AllCards[0]);
        EnemyDeck.Add(CardManagerClass.AllCards[1]);
        EnemyDeck.Add(CardManagerClass.AllCards[2]);
        EnemyDeck.Add(CardManagerClass.AllCards[2]);
      
        RemainingEnemyDeck = new List<Card>(EnemyDeck);
        RemainingPlayerDeck = new List<Card>(PlayerDeck);

        EnemyHand = new List<Card>();
        PlayerHand = new List<Card>();

        
    }
    private List<Card> GiveDeckCard()
    {
        List<Card> list = new List<Card>();
        for (int i = 0; i < deckSize; i++)
        {
            list.Add(CardManagerClass.AllCards[Random.Range(0, CardManagerClass.AllCards.Count)]);
        }
        return list;
    }
}
public class BattleManager : MonoBehaviour
{
    private CardManager CM;
    public Battle currentBattle;
    [SerializeField]
    private Transform enemyHand, playerHand;
    [SerializeField]
    private GameObject CardPrefab;
    [SerializeField]
    private Button endTurnButton;
    private int Turn;
    private int handSize = 4;
    public bool isPlayerTurn
    {
        get
        {
            return Turn % 2 == 0;
        }
    }    
    void Start()
    {
        Turn = 0;
        currentBattle = new Battle();
        CM = FindObjectOfType<CardManager>();
        GiveHandCards(currentBattle.RemainingEnemyDeck, enemyHand, currentBattle.EnemyHand, currentBattle.EnemyDeck);     
        GiveHandCards(currentBattle.RemainingPlayerDeck, playerHand, currentBattle.PlayerHand, currentBattle.PlayerDeck);

        StartCoroutine(TurnFunction());
        //CheckDeck();
    }

   private void GiveHandCards(List<Card> remdeck, Transform handobj, List<Card> hand, List<Card> deck)
    {
        
        var cap = hand.Count;
        for (int i = 0; i < cap; i++)
        {
            hand.RemoveAt(0);
        }
        foreach (Transform child in handobj)
        {
            GameObject.Destroy(child.gameObject);
        }
        for (int i = 0; i < handSize; i++)
        {
            CardToHand(remdeck, handobj, hand, deck);
        }
    }
    private void CardToHand(List<Card> remdeck, Transform handobj, List<Card> hand, List<Card> deck)
    {
        if (remdeck.Count == 0)
        {
            Refill();
            remdeck = new List<Card>(deck);
         
        }
        var pos = Random.Range(0, remdeck.Count);
        Card card = remdeck[pos];
        hand.Add(card);
        GameObject cardgameObject = Instantiate(CardPrefab, handobj, false);
        cardgameObject.GetComponent<CardInfo>().SetColor(cardgameObject, card);
        
        if (handobj == enemyHand)
            cardgameObject.GetComponent<CardInfo>().HideCardInfo(card);
        else
            cardgameObject.GetComponent<CardInfo>().ShowCardInfo(card);
        remdeck.RemoveAt(pos);

    }
    public void DrawCard()
    {
        if (isPlayerTurn)
            CardToHand(currentBattle.RemainingPlayerDeck, playerHand, currentBattle.PlayerHand, currentBattle.PlayerDeck);
        else
            CardToHand(currentBattle.RemainingEnemyDeck, enemyHand, currentBattle.EnemyHand, currentBattle.EnemyDeck);
    }
    public void DiscardCard()
    {
        var random = 0;
        if (isPlayerTurn)
        {
            if (playerHand.childCount == 0)
                return;
            random = Random.Range(0, playerHand.childCount);
            Destroy(playerHand.GetChild(random).gameObject);
            currentBattle.PlayerHand.RemoveAt(random);
        }
        else
        {
            if (enemyHand.childCount == 0)
                return;
            random = Random.Range(0, enemyHand.childCount);
            Destroy(enemyHand.GetChild(random).gameObject);
            currentBattle.EnemyHand.RemoveAt(random);
        }
    }
  
    private IEnumerator TurnFunction()
    {
        if (isPlayerTurn)
            yield return new WaitWhile(() => true);
        else
            StartCoroutine(EnemyTurn());
    } 
    private IEnumerator EnemyTurn()
    {
        StopAllCoroutines();
        // while (currentBattle.EnemyHand.Count > 0 && sad < 3)
        for (int i = 0; i < enemyHand.childCount; i++)
        {
            CM.Use(enemyHand.GetChild(i).gameObject);
            Debug.Log("asd");
            //yield return new WaitForSeconds(.05f);
            //Debug.Log("wha");
        }
        ChangeTurn();
        yield return null;
    }
    public void ChangeTurn()
    {
        StopAllCoroutines();
        Turn++;
        endTurnButton.interactable = isPlayerTurn;
        if (isPlayerTurn)
        {
            GiveHandCards(currentBattle.RemainingPlayerDeck, playerHand, currentBattle.PlayerHand, currentBattle.PlayerDeck);
        }
        else
        {
            GiveHandCards(currentBattle.RemainingEnemyDeck, enemyHand, currentBattle.EnemyHand, currentBattle.EnemyDeck);       
        }
            StartCoroutine(TurnFunction());
    }
    public void Refill()
    {
        if (currentBattle.RemainingEnemyDeck.Count == 0)
            currentBattle.RemainingEnemyDeck = new List<Card>(currentBattle.EnemyDeck);
        if (currentBattle.RemainingPlayerDeck.Count == 0)
            currentBattle.RemainingPlayerDeck = new List<Card>(currentBattle.PlayerDeck);

    }
    public void CheckDeck()
    {
        Debug.Log($"{currentBattle.EnemyDeck.Count(c => c.Name == "punch")} punch," +
            $"{currentBattle.EnemyDeck.Count(c => c.Name == "minor heal")} heal," +
            $"{ currentBattle.EnemyDeck.Count(c => c.Name == "small shield")} shield");
        Debug.Log($"{currentBattle.PlayerDeck.Count(c => c.Name == "punch")} punch," +
            $"{currentBattle.PlayerDeck.Count(c => c.Name == "minor heal")} heal," +
            $"{ currentBattle.PlayerDeck.Count(c => c.Name == "small shield")} shield");
      
    }
    
}
