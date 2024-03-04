using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Item
{
    public readonly string Name;
    public readonly string Description;
    public readonly int Cost;
    //bool consumable;
    public readonly ItemType Type;
    public readonly int Value;
    public readonly Sprite Logo;
    
    public enum ItemType
    {
        NONE,
        WEAPON,
        BODY,
        HELMET
    }
    public Item(string name, int cost, string logoPath, ItemType type, int value)
    {
        Name = name;
        Description = "";
        Cost = cost;
        Logo = Resources.Load<Sprite>(logoPath);
        Type = type;
        Value = value;
    }
}
public static class ItemClass
{
    public static List<Item> AllItems = new List<Item>();
}
public class ItemManager : MonoBehaviour
{
    private void Awake()
    {
        
        ItemClass.AllItems.Add(new Item("Rusty Sword", 0, "sprites/items/Sword", Item.ItemType.WEAPON, 15));
        ItemClass.AllItems.Add(new Item("Cloak", 0, "sprites/items/Cloak", Item.ItemType.HELMET, 15));
        ItemClass.AllItems.Add(new Item("Leather Cuirass", 0, "sprites/items/LeatherBody", Item.ItemType.BODY, 100));

        ItemClass.AllItems.Add(new Item("Iron Spear", 75, "sprites/items/Spear", Item.ItemType.WEAPON, 24));
        ItemClass.AllItems.Add(new Item("Iron Helmet", 125, "sprites/items/Helmet", Item.ItemType.HELMET, 19));
        ItemClass.AllItems.Add(new Item("Iron Cuirass", 100, "sprites/items/IronLeatherBody", Item.ItemType.BODY, 135));

        ItemClass.AllItems.Add(new Item("Iron Axe", 150, "sprites/items/Axe", Item.ItemType.WEAPON, 32));
        ItemClass.AllItems.Add(new Item("Elite Helmet", 250, "sprites/items/EliteHelmet", Item.ItemType.HELMET, 25));
        ItemClass.AllItems.Add(new Item("Chest Plate", 200, "sprites/items/ChestPlate", Item.ItemType.BODY, 200));

    }
}

