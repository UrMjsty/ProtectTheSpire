using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
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

    [SerializeField]
    private int playerHealth;
    [SerializeField]
    private int playerArmor;
    [SerializeField]
    private int enemyHealth;
    [SerializeField]
    private int enemyArmor;
    [SerializeField]
    private int maxHealth;

    [SerializeField]
    private GameObject resultGO;

    private void Start()
    {
        EnemyArmor.text = enemyArmor.ToString();
        EnemyHealth.text = enemyHealth.ToString();
        PlayerArmor.text = playerArmor.ToString();
        PlayerHealth.text = playerHealth.ToString();
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
            resultGO.SetActive(true);
            StopAllCoroutines();
            if (playerHealth == 0)
                resultText.text = "Pathetic";
            else
                resultText.text = "Win";
        }
    }
}
