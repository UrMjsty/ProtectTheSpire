using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    private Enemy enemy;
    private Player player;
    private BattleManager BM;
    private GameManager GM;
    [Header("Numbers")]
    [SerializeField]
    [Min(0)]
    private int playerHealth;
    [SerializeField]
    private int playerArmor;
    [SerializeField]
    [Min(0)]
    private int enemyHealth;
    [SerializeField]
    private int enemyArmor;
    [SerializeField]
    [Min(0)]
    private int enemyMaxHealth;
    [SerializeField]
    [Min(0)]
    private int playerMaxHealth;

    [HideInInspector]
    

    [Header("Texts")]
    [SerializeField]
    private Text PlayerHealth;
    [SerializeField]
    private Text EnemyHealth;
    [SerializeField]
    private Text PlayerArmor;
    [SerializeField]
    private Text EnemyArmor;
    [SerializeField]
    private Text resultText;

   
    [Header("GameObjects")]
    [SerializeField]
    private GameObject resultGO;

    private void Start()
    {
        BM = FindObjectOfType<BattleManager>();
        GM = FindObjectOfType<GameManager>();
       
    }
    public void DealDamage(int value, bool isPlayer)
    {
        if (isPlayer)
        {
            var delta = Mathf.Clamp(value * player.inventory.Weapon.Value - enemyArmor, 0, int.MaxValue);
            enemyArmor = Mathf.Clamp(enemyArmor -= value * player.inventory.Weapon.Value, 0, int.MaxValue);
            EnemyArmor.text = enemyArmor.ToString();
            enemyHealth = Mathf.Clamp(enemyHealth -= delta, 0, enemyMaxHealth);
            EnemyHealth.text = enemyHealth.ToString();
        }
        else
        {
            var delta = Mathf.Clamp(value * enemy.Damage - playerArmor, 0, int.MaxValue);
            playerArmor = Mathf.Clamp(playerArmor -= value * enemy.Damage, 0, int.MaxValue);
            PlayerArmor.text = playerArmor.ToString();
            playerHealth = Mathf.Clamp(playerHealth -= delta, 0, playerMaxHealth);
            PlayerHealth.text = playerHealth.ToString();
        }
        CheckForAlive();
    }

    public void GainArmor(int value, bool isPlayer)
    {
        if (isPlayer)
        {
            playerArmor += value * player.inventory.Helmet.Value;
            PlayerArmor.text = playerArmor.ToString();
        }
        else
        {
            enemyArmor += value * enemy.Armor;
            EnemyArmor.text = enemyArmor.ToString();
        }
    }

    public void RestoreHealth(int value, bool isPlayer)
    {
        if (isPlayer)
        {
            playerHealth = Mathf.Clamp(playerHealth += value, 0, playerMaxHealth);
            PlayerHealth.text = playerHealth.ToString();
        }
        else
        {
            enemyHealth = Mathf.Clamp(enemyHealth += value, 0, enemyMaxHealth);
            EnemyHealth.text = enemyHealth.ToString();
        }
    }
    private void CheckForAlive()
    {
        if (enemyHealth == 0 || playerHealth == 0)
        {
            BM.EndGame();
            StopAllCoroutines();
            if (playerHealth == 0)
                GM.LoseBattle();
            else
                GM.WinBattle();
        }
    }

    public void StartBatte(Enemy en, Player pl)
    {
        player = pl;
        enemy = en;

        playerHealth = player.inventory.Body.Value;
        playerMaxHealth = playerHealth;
        PlayerHealth.text = playerHealth.ToString();
        playerArmor = 0;
        PlayerArmor.text = playerArmor.ToString();

        enemyHealth = enemy.Health;
        enemyMaxHealth = enemyHealth;
        EnemyHealth.text = enemyHealth.ToString();
        enemyArmor = 0;
        EnemyArmor.text = enemyArmor.ToString();

        resultGO.SetActive(false);      
    }
}
