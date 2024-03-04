using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MerchInfo : MonoBehaviour
{
    public Item SelfItem;
    public Transform back;
    [FormerlySerializedAs("Logo")] public Image logo;
    public Text Name;
    [FormerlySerializedAs("Description")] public TextMeshProUGUI description;
    [FormerlySerializedAs("Price")] public TextMeshProUGUI price;
    [SuppressMessage("ReSharper", "InconsistentNaming")] public ShopManager SM;
    
    public void ShowMerchInfo(Item item)
    {
        SelfItem = item;
        logo.sprite = item.Logo;
        logo.preserveAspect = true;
        Name.text = item.Name;
        description.text = item.Description;
        ShowDescription();
        back.GetComponent<Image>().color = new Color32(255, 215, 0, 200);
        price.text = item.Cost.ToString();
    }
    private void ShowDescription()
    {
        switch (SelfItem.Type)
        {
            case Item.ItemType.NONE:
                break;
            case Item.ItemType.WEAPON:
                description.text = $"{SelfItem.Value} Damage";
                break;
            case Item.ItemType.BODY:
                description.text = $"{SelfItem.Value} Health";
                break;
            case Item.ItemType.HELMET:
                description.text = $"{SelfItem.Value} Armor";
                break;
        }
    }
    
}
