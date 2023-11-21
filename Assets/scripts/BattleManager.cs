using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private Text playerName;
    [SerializeField]
    private Text enemyName;
    private Enemy _enemy;
    private Player _player;
    [SerializeField]
    private GameObject battleField;
    [SuppressMessage("ReSharper", "InconsistentNaming")] private CardManager CM;
    [SuppressMessage("ReSharper", "InconsistentNaming")] private HealthManager HM;
    [SerializeField]
    private Transform enemyHand, playerHand;
    [SerializeField]
    private Transform enemyField;
    [SerializeField]
    private GameObject cardPrefab;
    [SerializeField]
    private Image enemyImage;
    [SerializeField]
    private Image playerImage;
    [SerializeField]
    private Button endTurnButton;
    private int _turn;
    public bool IsPlayerTurn => _turn % 2 == 0;

    void Start()
    {
        CM = FindObjectOfType<CardManager>();
        HM = FindObjectOfType<HealthManager>();
        // StartGame();
        battleField.SetActive(false);
    }

    private void GiveHandCards(Character character)
    {
        var remcards = character.HandObject.childCount;
        for (int i = remcards; i < character.Handsize; i++)
        {
            CardToHand(character);
        }
    }
    private void CardToHand(Character character)
    {
        if (character.RemainingDeck.Count == 0)
        {
            Refill(character);
         
        }
        var pos = Random.Range(0, character.RemainingDeck.Count);
        Card card = character.RemainingDeck[pos];
        card.Owner = character;
        character.Hand.Add(card);
        GameObject cardGameObject = Instantiate(cardPrefab, character.HandObject, false);
        cardGameObject.GetComponent<CardInfo>().ShowCardInfo(card, cardGameObject);
        if (character == _enemy)
            cardGameObject.GetComponent<CardInfo>().HideCardInfo(card);
        character.RemainingDeck.RemoveAt(pos);

    }
    public void DrawCard(Character character)
    {
        CardToHand(character);
    }
    public void DiscardCard(Character character)
    {
        var childcount = character.HandObject.childCount;
        if (childcount == 0)
            return;
        var random = Random.Range(0, childcount); 
        Destroy(character.HandObject.GetChild(random).gameObject); 
        character.Hand.RemoveAt(random);
    }

    public Character GetCurrentCharacter()
    {
        return IsPlayerTurn ? _player : _enemy;
    }
    private IEnumerator PlayerTurn()
    {
        StopAllCoroutines();
        endTurnButton.interactable = true;
        _turn++;
        GiveHandCards(_player);
        yield return new WaitWhile(() => true);
        yield return new WaitForSeconds(3);
        StartCoroutine(EnemyTurn());
    }
    private IEnumerator EnemyTurn()
    {
        _turn++;
        endTurnButton.interactable = false;
        GiveHandCards(_enemy);
        yield return new WaitForSeconds(.5f);
        while (_enemy.HandObject.childCount > 0)
        {
            //var cardGo = enemyHand.GetChild(0).gameObject;
            var enemyHandChildCount = _enemy.HandObject.childCount;
            _enemy.HandObject.GetChild(enemyHandChildCount - 1).gameObject.GetComponent<CardInfo>().ShowCardInfo(_enemy.HandObject.GetChild(enemyHandChildCount - 1)
                .gameObject.GetComponent<CardInfo>().SelfCard, _enemy.HandObject.GetChild(enemyHandChildCount-1).gameObject);
            _enemy.HandObject.GetChild(enemyHandChildCount-1).gameObject.GetComponent<CardMovement>().MoveToField(enemyField);
            yield return new WaitForSeconds(.51f);
            CM.Use(_enemy.HandObject.GetChild(enemyHandChildCount - 1).gameObject, 0.5f);
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

    private void Refill(Character character)
    {
        character.RemainingDeck = new List<Card>(character.Deck);
    }
   
    public void StartBattle(Enemy en,ref Player player)
    {
        _enemy = en;
        _player = player;

        _enemy.Opponent = _player;
        _player.Opponent = _enemy;
        
        battleField.SetActive(true);
        endTurnButton.interactable = true;

        _turn = 1;

        HM.StartBattle(_enemy,ref player);

        enemyImage.sprite = _enemy.Image;
        enemyName.text = _enemy.Name;

        playerImage.sprite = player.Image;
        playerName.text = player.Name;

        _enemy.HandObject = enemyHand;
        _player.HandObject = playerHand;

        _enemy.RemainingDeck = new List<Card>(_enemy.Deck);
        _player.RemainingDeck = new List<Card>(_player.Deck);
        _enemy.Hand = new List<Card>();
        _player.Hand = new List<Card>();

        GiveHandCards(_enemy);
        GiveHandCards(_player);

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

        //   _enemy.Deck.Clear();
        _enemy.Hand.Clear();
      //  _player.Deck.Clear();
        _player.Hand.Clear();
        battleField.SetActive(false);
    }
}
