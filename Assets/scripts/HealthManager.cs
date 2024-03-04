using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    private Enemy _enemy;
    private Player _player;
    [SuppressMessage("ReSharper", "InconsistentNaming")] private BattleManager BM;
    [SuppressMessage("ReSharper", "InconsistentNaming")] private GameManager GM;
    [FormerlySerializedAs("PlayerHealth")]
    [HideInInspector]
    [Header("Texts")]
    [SerializeField]
    private Text playerHealthText;
    [FormerlySerializedAs("EnemyHealth")] [SerializeField]
    private Text enemyHealthText;
    [FormerlySerializedAs("PlayerArmor")] [SerializeField]
    private Text playerArmorText;
    [FormerlySerializedAs("EnemyArmor")] [SerializeField]
    private Text enemyArmorText;
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
    public void TakeDamage(int value, Character victim)
    {
        var delta = Mathf.Clamp(value - victim.Armor, 0, int.MaxValue);
        victim.Armor = Mathf.Clamp(victim.Armor -= value, 0, int.MaxValue);
        victim.ArmorText.text = victim.Armor.ToString();
        victim.Health = Mathf.Clamp(victim.Health -= delta, 0, victim.GetMaxHealth());
        victim.HealthText.text = victim.Health.ToString();
        
        CheckForAlive();
    }

    public void GainArmor(int value, Character character)
    {
        character.Armor += value;
        character.ArmorText.text = character.Armor.ToString();
    }

    public void RestoreHealth(int value, Character character)
    {
        character.Health = Mathf.Clamp(character.Health += value, 0, character.GetMaxHealth()); 
        character.HealthText.text = character.Health.ToString();
        enemyHealthText.text = _enemy.Health.ToString();
        
    }
    private void CheckForAlive()
    {
        if (_enemy.Health == 0 || _player.Health == 0)
        {
            BM.EndGame();
            StopAllCoroutines();
            if (_player.Health == 0)
                GM.LoseBattle();
            else
                GM.WinBattle();
        }
    }

    public void StartBattle(Enemy en,ref Player pl)
    {
        _player = pl;
        _enemy = en;

        _enemy.ArmorText = enemyArmorText;
        _enemy.HealthText = enemyHealthText;
        _player.ArmorText = playerArmorText;
        _player.HealthText = playerHealthText;
        
        playerHealthText.text = _player.Health.ToString();
        _player.Armor = 0;
        playerArmorText.text = _player.Armor.ToString();

        enemyHealthText.text = _enemy.Health.ToString();
        _enemy.Armor = 0;
        enemyArmorText.text = _enemy.Armor.ToString();

        resultGO.SetActive(false);      
    }
    public void WinBattle()
    {
        _enemy.Health = 0;
        CheckForAlive();
    }
}
