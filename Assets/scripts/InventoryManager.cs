using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Inventory
{
    public Item Weapon;
    public Item Body;
    public Item Helmet;
}
public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private Image helmetImage;
    [SerializeField]
    private Image bodyImage;
    [SerializeField]
    private Image weaponImage; 
    [SerializeField]
    private Text helmetText;
    [SerializeField]
    private Text bodyText;
    [SerializeField]
    private Text weaponText;
    public void UpdateInventory(Player player)
    {
        helmetImage.sprite = player.Inventory.Helmet.Logo;
        bodyImage.sprite = player.Inventory.Body.Logo;
        weaponImage.sprite = player.Inventory.Weapon.Logo;
        helmetText.text = $"{player.Inventory.Helmet.Value.ToString()} Armor";
        bodyText.text = $"{player.Inventory.Body.Value.ToString()} Health";
        weaponText.text = $"{player.Inventory.Weapon.Value.ToString()} Damage";
    }

    public void UpdateItem(Player player, Item item)
    {
        switch (item.Type)
        {
            case Item.ItemType.NONE:
                break;
            case Item.ItemType.WEAPON:
                player.Inventory.Weapon = item;
                weaponImage.sprite = item.Logo;
                weaponText.text = $"{item.Value.ToString()} Damage";
                break;
            case Item.ItemType.BODY:
                player.Inventory.Body = item;
                bodyImage.sprite = item.Logo;
                bodyText.text = $"{item.Value.ToString()} Health";
                break;
            case Item.ItemType.HELMET:
                player.Inventory.Helmet = item;
                helmetImage.sprite = item.Logo;
                helmetText.text = $"{item.Value.ToString()} Armor";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
