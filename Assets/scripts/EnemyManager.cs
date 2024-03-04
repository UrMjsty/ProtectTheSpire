using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Enemy : Character
{
    
    public readonly RewardType Reward;
    public readonly List<string> Phrases;
    public readonly int DifficultyLevel;
    public enum RewardType
    {
        EMPTY,
        COMMON,
        RARE,
        EPIC,
        LEGENDARY
    }

    public Enemy (string name, string imagePath, int health, int armorUp, int damage, List<Card> deck, int handsize, RewardType reward, List<string> phrases, int dlevel): base( health,handsize, name, deck, imagePath)
    {
        Name = name;
        Image = Resources.Load<Sprite>(imagePath);
        Health = health;
        SetArmorUp(armorUp);
        SetDamage(damage);
        Deck = new List<Card>(deck);
        Handsize = handsize;
        Reward = reward;
        Phrases = new List<string>(phrases);
        DifficultyLevel = dlevel;
    }
}
public static class EnemyClass
{
    public static readonly List<Enemy> AllEnemies = new List<Enemy>();
    

    public static readonly List<string> DummyPhrases = new List<string>(){"Fight Me!", "ha-ha-ha-ha", "You can do it!", "Just do it!", "Good luck, freak"};
    public static readonly List<string> GoblinPhrases = new List<string>(){"i kill you!", "you weak!", "My strong!", "rizzik zik rak!", "run away fool!"};
    public static readonly List<string> RoguePhrases = new List<string>(){"Prepare your pockets.", "Trick or treat?", "You are unlucky.", "Give up or die.", "It's just a business."};
    public static readonly List<string> BatPhrases = new List<string>(){"ik-ki-ki-k", "ki-ik-k-ki", "ik-k-ki-ki", "ki-ik-ki-k", "ki-k-ik-ki"};
}
public class EnemyManager : MonoBehaviour
{
    [SuppressMessage("ReSharper", "InconsistentNaming")] private GameManager GM;
    void Start()
    {
        EnemyClass.AllEnemies.Add(new Enemy("Dummy", "sprites/enemies/Dummy", 40, 10, 0, CardManagerClass.DummyDeck, 1, Enemy.RewardType.COMMON, EnemyClass.DummyPhrases, 0));
        EnemyClass.AllEnemies.Add(new Enemy("Goblin", "sprites/enemies/Goblin", 70, 10, 17, CardManagerClass.GoblinDeck, 2, Enemy.RewardType.COMMON, EnemyClass.GoblinPhrases, 1));
        EnemyClass.AllEnemies.Add(new Enemy("Rogue", "sprites/enemies/Rogue", 80, 15, 15, CardManagerClass.RogueDeck, 2, Enemy.RewardType.COMMON, EnemyClass.RoguePhrases, 1));
        EnemyClass.AllEnemies.Add(new Enemy("Bat1", "sprites/enemies/Bat", 110, 0, 20, CardManagerClass.BatDeck, 2, Enemy.RewardType.COMMON, EnemyClass.BatPhrases, 2));
        EnemyClass.AllEnemies.Add(new Enemy("Bat2", "sprites/enemies/Bat", 110, 0, 20, CardManagerClass.BatDeck, 2, Enemy.RewardType.COMMON, EnemyClass.BatPhrases, 2));
        EnemyClass.AllEnemies.Add(new Enemy("Bat3", "sprites/enemies/Bat", 110, 0, 20, CardManagerClass.BatDeck, 2, Enemy.RewardType.COMMON, EnemyClass.BatPhrases, 2));
    
        GM = FindObjectOfType<GameManager>();
        GM.RemainingEnemies = new List<Enemy>(EnemyClass.AllEnemies);
    }
   

}
