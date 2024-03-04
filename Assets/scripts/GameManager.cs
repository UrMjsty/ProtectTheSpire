using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Character
{
    
    public int Health;
    private int _maxHealth;

    public int GetMaxHealth()
    {
        return Math.Max(Inventory.Body.Value, _maxHealth);
    }

    protected void SetMaxHealth(int value)
    {
        _maxHealth = value;
    }
    public int Armor;
    private int _armorUp;

    public int GetArmorUp()
    {
        return math.max(Inventory.Helmet.Value, _armorUp);
    }

    protected void SetArmorUp(int value)
    {
        _armorUp = value;
    }

    private int _damage;
    public int GetDamage()
    {
        return Math.Max(Inventory.Weapon.Value, _damage);
    }

    protected void SetDamage(int value)
    {
        _damage = value;
    }

    public int Handsize;
    public string Name;
    public List<Card> Deck;
    public List<Card> RemainingDeck;
    public List<Card> Hand;
    public Inventory Inventory;
    public Sprite Image;
    public Transform HandObject;
    public Text HealthText;
    public Text ArmorText;
    public Character Opponent;

    protected Character(int health,int handsize, string name, List<Card> deck, string imagePath)
    {
        this.SetMaxHealth(health);
        this.Health = health;
        this.Handsize = handsize;
        this.Name = name;
        this.Deck = deck;
        this.Image = Resources.Load<Sprite>(imagePath);
    }
}
public class Player : Character
{
    public int Experience;
    
    public Player(int health,int handsize, string name, List<Card> deck, Inventory inventory, string imagePath) : base( health, handsize, name, deck, imagePath)
    {
        this.Inventory = inventory; 
        this.SetMaxHealth(Inventory.Body.Value);
     //   this.Damage = Inventory.Weapon.Value;
        this.SetArmorUp(Inventory.Helmet.Value);
    }
    
}
public class GameManager : MonoBehaviour
{
    public List<Enemy> RemainingEnemies;
    private Player _player;
    private Enemy _currentEnemy;
    [SuppressMessage("ReSharper", "InconsistentNaming")] private BattleManager BM;
    [SuppressMessage("ReSharper", "InconsistentNaming")] private ShopManager SM;
    [SuppressMessage("ReSharper", "InconsistentNaming")] private InventoryManager IM;
    [SuppressMessage("ReSharper", "InconsistentNaming")] private DeckManager DM;
    [SerializeField]
    private Image enemyImage;
    [Header("GameObjects")]
    [SerializeField]
    private GameObject field;
    [SerializeField]
    private GameObject shopWindow;
    [SerializeField]
    private GameObject rewardWindow;
    [SerializeField]
    private GameObject deckWindow;
    [SerializeField]    
    private GameObject removeCardButton;
    [SerializeField]
    private GameObject addCardButton;
    [SerializeField]
    private GameObject inventoryWindow;
    [Header("Texts")]
    [SerializeField]
    private Text enemyPhrase;
    [SerializeField]
    private Text enemyNameText;
    [SerializeField]
    private Text shopButtonText;
    [SerializeField]
    private Text deckButtonText;
    [SerializeField]
    private Text inventoryButtonText;

    private int CurrentDifficulty => _player.Experience / 6;

    void Start()
    {
        _player = new Player(100, 4,"Sir Vagner", CardManagerClass.StartingDeck, GetStartingInventory(), "sprites/heroes/Knight")
            {
                Experience = 0  
            };
        Debug.Log(_player.Deck.Count.ToString());
        StartApp();
        BM = FindObjectOfType<BattleManager>();
        SM = FindObjectOfType<ShopManager>();
        IM = FindObjectOfType<InventoryManager>();
        DM = FindObjectOfType<DeckManager>();
        
        IM.UpdateInventory(_player);
        SM.PlayerObject = _player;
        DM.PlayerObject = _player;
        StartCoroutine(ChangeEnemy(0));
    }
    
    private void Update()
    {
        if( Input.GetMouseButtonDown(0) )
        {
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            RaycastHit hit;
		
            if( Physics.Raycast( ray, out hit, 100 ) )
            {
                Debug.Log( hit.transform.gameObject.name );
            }
        }
    }

    IEnumerator ChangeEnemy(int difficulty)
    {
        
        yield return new WaitForSeconds(.01f);
        
       
        var enemies = new List<Enemy>();
        foreach (var en in RemainingEnemies)
        {
            if (en.DifficultyLevel == difficulty)
            {
                enemies.Add(en);
            }
        }
        _currentEnemy = enemies[Random.Range(0, enemies.Count)];
        RemainingEnemies.Remove(_currentEnemy);
        enemyImage.sprite = _currentEnemy.Image;
        enemyPhrase.text = _currentEnemy.Phrases[Random.Range(0, _currentEnemy.Phrases.Count)];
        enemyNameText.text = _currentEnemy.Name;

    }
  
    public void StartBattle()
    {
        field.SetActive(false);
        inventoryButtonText.text = "Inventory";
        inventoryWindow.SetActive(false);
        BM.StartBattle(_currentEnemy,ref _player);
    }

    private void GainReward(Enemy.RewardType rewardType)
    {
        var reward = 0;
        switch (rewardType)
        {
            case Enemy.RewardType.EMPTY:
                break;
            case Enemy.RewardType.COMMON:
                reward = Random.Range(47, 63);
                break;
            case Enemy.RewardType.RARE:
                reward = Random.Range(72, 99);
                break;
            case Enemy.RewardType.EPIC:
                reward = Random.Range(130, 173);
                break;
            case Enemy.RewardType.LEGENDARY:
                reward = Random.Range(250, 500);
                break;
        }
        print(reward.ToString());
        SM.GetGold(reward);
    }
 
    public void WinBattle()
    {
        field.SetActive(true);
        rewardWindow.SetActive(true);
        GainReward(_currentEnemy.Reward);
        _player.Experience += 6 / (_currentEnemy.DifficultyLevel + 1);
        if (RemainingEnemies.Count == 0)
        {
            enemyImage.sprite = null;
            enemyPhrase.text = "GRATZ WP";
            return;
        }
        SM.timesRefilled = -1;
        SM.RefillOffer();
            //  CurrentDifficulty++;
        StartCoroutine(ChangeEnemy(CurrentDifficulty));
    }
    public void LoseBattle()
    {
        field.SetActive(true);
        SM.SetGold(0);
        RemainingEnemies = new List<Enemy>(EnemyClass.AllEnemies);
        _player.Experience = 0;
       StartCoroutine(ChangeEnemy(CurrentDifficulty));
    }
    private Inventory GetStartingInventory()
    {
        var inv = new Inventory
        {
            Weapon = ItemClass.AllItems[0],
            Helmet = ItemClass.AllItems[1],
            Body = ItemClass.AllItems[2]
        };
        return inv;
    }

    public void ToggleShopWindow()
    {
        if (shopWindow.activeInHierarchy)
        {
            shopButtonText.text = "Shop";
            shopWindow.SetActive(false);
            deckButtonText.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            shopButtonText.text = "Close";
            shopWindow.SetActive(true);
            deckButtonText.transform.parent.gameObject.SetActive(false);
        }
    } 
    public void ToggleInventory()
    {
        if (inventoryWindow.activeInHierarchy)
        {
            inventoryButtonText.text = "Inventory";
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryButtonText.text = "Close";
            inventoryWindow.SetActive(true);
        }
    }
  
    
   

    public void RemoveCard()
    {
        rewardWindow.SetActive(false);
        OpenDeckWindow();
        removeCardButton.SetActive(true);
    }
    public void AddCard()
    {
        rewardWindow.SetActive(false);
        OpenDeckWindow();
        addCardButton.SetActive(true);
    } 
    public void TakeMoney()
    {
        rewardWindow.SetActive(false);
    }

    public void ToggleDeckWindow()
    {
        if (deckWindow.activeInHierarchy)
        {
            DM.RemoveDeck();
            deckButtonText.text = "Deck";
            addCardButton.SetActive(false);
            removeCardButton.SetActive(false);
            deckWindow.SetActive(false);
            deckButtonText.transform.parent.position = new Vector3(0, 4, 0);
        }
        else
            OpenDeckWindow();
    }

    private void OpenDeckWindow()
    {
        DM.UpdateDeck();
        deckButtonText.text = "Close";
        deckWindow.SetActive(true);
        deckButtonText.transform.parent.position = new Vector3(8.6f , 4, 0) ;
    }
    private void StartApp()
    {
        field.SetActive(true);
        rewardWindow.SetActive(false);
        inventoryWindow.SetActive(false);
        shopWindow.SetActive(false);
        deckWindow.SetActive(false);
//        Player.Health = Player.Inventory.Body.Value;
    }
}
