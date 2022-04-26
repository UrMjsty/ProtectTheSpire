using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
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
    private int maxHealth;

    [HideInInspector]
    public int startPlayerHealth;
    public int startPlayerArmor;
    public int startEnemyHealth; 
    public int startEnemyArmor;

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
        startEnemyArmor = 0;
        startEnemyHealth = 5;
        startPlayerArmor = 5;
        startPlayerHealth = 5;
    }
    public void DealDamage(int value, bool isPlayer)
    {
        if (isPlayer)
        {
            var delta = Mathf.Clamp(value - enemyArmor, 0, int.MaxValue);
            enemyArmor = Mathf.Clamp(enemyArmor -= value, 0, int.MaxValue);
            EnemyArmor.text = enemyArmor.ToString();
            enemyHealth = Mathf.Clamp(enemyHealth -= delta, 0, maxHealth);
            EnemyHealth.text = enemyHealth.ToString();
        }
        else
        {
            var delta = Mathf.Clamp(value - playerArmor, 0, int.MaxValue);
            playerArmor = Mathf.Clamp(playerArmor -= value, 0, int.MaxValue);
            PlayerArmor.text = playerArmor.ToString();
            playerHealth = Mathf.Clamp(playerHealth -= delta, 0, maxHealth);
            PlayerHealth.text = playerHealth.ToString();
        }
        CheckForAlive();
    }

    public void GainArmor(int value, bool isPlayer)
    {
        if (isPlayer)
        {
            playerArmor += value;
            PlayerArmor.text = playerArmor.ToString();
        }
        else
        {
            enemyArmor += value;
            EnemyArmor.text = enemyArmor.ToString();
        }
    }

    public void RestoreHealth(int value, bool isPlayer)
    {
        if (isPlayer)
        {
            playerHealth = Mathf.Clamp(playerHealth += value, 0, maxHealth);
            PlayerHealth.text = playerHealth.ToString();
        }
        else
        {
            enemyHealth = Mathf.Clamp(enemyHealth += value, 0, maxHealth);
            EnemyHealth.text = enemyHealth.ToString();
        }
    }
    private void ShowInfo()
    {
        Debug.Log(playerHealth);
        Debug.Log(playerArmor);
        Debug.Log(enemyHealth);
    }
    private void CheckForAlive()
    {
        if (enemyHealth == 0 || playerHealth == 0)
        {
            BM.EndGame();
            //resultGO.SetActive(true);
            StopAllCoroutines();
            if (playerHealth == 0)
                //  resultText.text = "Pathetic";
                GM.LoseBattle();
            else
                //resultText.text = "Win";
                GM.WinBattle();
        }
    }

    public void StartBatte(Enemy enemy, Player player)
    {
        playerHealth = player.Health;
        PlayerHealth.text = playerHealth.ToString();
        playerArmor = player.Armor;
        PlayerArmor.text = playerArmor.ToString();
        enemyHealth = enemy.Health;
        EnemyHealth.text = enemyHealth.ToString();
        enemyArmor = enemy.Armor;
        EnemyArmor.text = enemyArmor.ToString();
        resultGO.SetActive(false);      
    }
}
