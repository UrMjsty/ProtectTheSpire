using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private Text playerName;
    [SerializeField]
    private Text enemyName;
    private Enemy enemy;
    [SerializeField]
    private GameObject battleField;
    private CardManager CM;
    private HealthManager HM;
    [SerializeField]
    private Transform enemyHand, playerHand;
    [SerializeField]
    private Transform enemyField;
    [SerializeField]
    private GameObject CardPrefab;
    [SerializeField]
    private Image enemyImage;
    [SerializeField]
    private Button endTurnButton;
    private int Turn;
    private int playerHandSize = 4;
    public bool isPlayerTurn
    {
        get
        {
            return Turn % 2 == 0;
        }
    }

    public List<Card> EnemyDeck, PlayerDeck,
                      EnemyHand, PlayerHand,
                      RemainingEnemyDeck, RemainingPlayerDeck;
    void Start()
    {
        CM = FindObjectOfType<CardManager>();
        HM = FindObjectOfType<HealthManager>();
        // StartGame();
        battleField.SetActive(false);
    }

    private void GiveHandCards(List<Card> remdeck, Transform handobj, List<Card> hand, List<Card> deck,int handsize)
    {
        var remcards = handobj.childCount;
        //var cap = hand.Count;
        //for (int i = 0; i < cap; i++)
        //{
        //    hand.RemoveAt(0);
        //}
        //foreach (Transform child in handobj)
        //{
        //    GameObject.Destroy(child.gameObject);
        //}
        for (int i = remcards; i < handsize; i++)
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
        cardgameObject.GetComponent<CardInfo>().ShowCardInfo(card, cardgameObject);
        if (handobj == enemyHand)
            cardgameObject.GetComponent<CardInfo>().HideCardInfo(card);
        remdeck.RemoveAt(pos);

    }
    public void DrawCard()
    {
        if (isPlayerTurn)
            CardToHand(RemainingPlayerDeck, playerHand, PlayerHand, PlayerDeck);
        else
            CardToHand(RemainingEnemyDeck, enemyHand, EnemyHand, EnemyDeck);
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
            PlayerHand.RemoveAt(random);
        }
        else
        {
            if (enemyHand.childCount == 0)
                return;
            random = Random.Range(0, enemyHand.childCount);
            Destroy(enemyHand.GetChild(random).gameObject);
            EnemyHand.RemoveAt(random);
        }
    }
    private IEnumerator PlayerTurn()
    {
        StopAllCoroutines();
        endTurnButton.interactable = true;
        Turn++;
        GiveHandCards(RemainingPlayerDeck, playerHand, PlayerHand, PlayerDeck, playerHandSize);
        yield return new WaitWhile(() => true);
        yield return new WaitForSeconds(3);
        StartCoroutine(EnemyTurn());
    }
    private IEnumerator EnemyTurn()
    {
        Turn++;
        endTurnButton.interactable = false;
        GiveHandCards(RemainingEnemyDeck, enemyHand, EnemyHand, EnemyDeck, enemy.Handsize);
        yield return new WaitForSeconds(.5f);
        while (enemyHand.childCount > 0)
        {
            //var cardGo = enemyHand.GetChild(0).gameObject;
            enemyHand.GetChild(enemyHand.childCount - 1).gameObject.GetComponent<CardInfo>().ShowCardInfo(enemyHand.GetChild(enemyHand.childCount - 1).gameObject.GetComponent<CardInfo>().SelfCard, enemyHand.GetChild(enemyHand.childCount-1).gameObject);
            enemyHand.GetChild(enemyHand.childCount-1).gameObject.GetComponent<CardMovement>().MoveToField(enemyField);
            yield return new WaitForSeconds(.51f);
            CM.Use(enemyHand.GetChild(enemyHand.childCount - 1).gameObject, 0.5f);
            yield return new WaitForSeconds(.51f);
        }
        StartCoroutine(PlayerTurn());
        yield return null;
    }
    
    public void ChangeTurn()
    {
        StopAllCoroutines();
        StartCoroutine(EnemyTurn());
    }
    public void Refill()
    {
        if (RemainingEnemyDeck.Count == 0)
            RemainingEnemyDeck = new List<Card>(EnemyDeck);
        if (RemainingPlayerDeck.Count == 0)
            RemainingPlayerDeck = new List<Card>(PlayerDeck);

    }
   
    public void StartBattle(Enemy en, Player player)
    {
        enemy = en;

        battleField.SetActive(true);
        endTurnButton.interactable = true;

        Turn = 1;

        HM.StartBatte(enemy, player);

        enemyImage.sprite = enemy.Image;
        enemyName.text = enemy.Name;

        playerName.text = player.Name;

        EnemyDeck = new List<Card>(enemy.Deck);
        PlayerDeck = new List<Card>(player.Deck);
        RemainingEnemyDeck = new List<Card>(EnemyDeck);
        RemainingPlayerDeck = new List<Card>(PlayerDeck);
        EnemyHand = new List<Card>();
        PlayerHand = new List<Card>();

        GiveHandCards(RemainingEnemyDeck, enemyHand, EnemyHand, EnemyDeck, enemy.Handsize);
        GiveHandCards(RemainingPlayerDeck, playerHand, PlayerHand, PlayerDeck, playerHandSize);

        StartCoroutine(PlayerTurn());
        
    }
    public void EndGame()
    {
        StopAllCoroutines();
        foreach (Transform card in playerHand)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in enemyHand)
        {
            Destroy(card.gameObject);
        }

        EnemyDeck.Clear();
        EnemyHand.Clear();
        PlayerDeck.Clear();
        PlayerHand.Clear();
        battleField.SetActive(false);
    }
}
