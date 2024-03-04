using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour
{
    public Player PlayerObject;
    [SuppressMessage("ReSharper", "InconsistentNaming")] private InventoryManager IM;
    private List<Item> _availableItems;
    [SerializeField]
    private Transform itemPool;
    [SerializeField]
    private GameObject itemPrefab;
    [SerializeField]
    private Text goldText;
    [SerializeField]
    private Text refillCostText;
    private readonly int _offerCount = 3;
    public int timesRefilled ;//= 0;
    private readonly int _refillCost = 15;
    public int gold;

    private Item _errorItem;
    void Start()
    {
        _errorItem = new Item("Error", Int32.MaxValue, "/sprites/heroes/Knight", Item.ItemType.NONE, 0);
        SetGold(2000);
        refillCostText.text = "0";
        IM = FindObjectOfType<InventoryManager>();
        _availableItems = new List<Item>(ItemClass.AllItems); 
        _availableItems.RemoveRange(0, 3);
        FillOffer(_offerCount);
    }
    public void RefillOffer()
    {
        if (CanSpendGold(timesRefilled * _refillCost))
        {
            SpendGold(timesRefilled * _refillCost);
            timesRefilled++;
            refillCostText.text = (timesRefilled * _refillCost).ToString();
            foreach (Transform item in itemPool)
            {
                Destroy(item.gameObject);
                _availableItems.Add(item.gameObject.GetComponent<MerchInfo>().SelfItem);
            }
            FillOffer(_offerCount);
        }
    }
    private void FillOffer(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if(!_availableItems.Any())
                _availableItems.Add(_errorItem);
            var pos = Random.Range(0, _availableItems.Count);
            Item item = _availableItems[pos];
            GameObject itemGO = Instantiate(itemPrefab, itemPool, false);
            itemGO.GetComponent<MerchInfo>().ShowMerchInfo(item);
            _availableItems.RemoveAt(pos);
        }
    }

    private bool CanSpendGold(int value)
    {
        return gold >= value;
    }

    private void SpendGold(int value)
    {
        if(value < 0)
            return;
        gold -= value;
        goldText.text = gold.ToString();
    }
    public void GetGold(int value)
    {
        gold += value;
        goldText.text = gold.ToString();
    }

    public void SetGold(int value)
    {
        gold = value;
        goldText.text = gold.ToString();
    }
    public void Buy(int index)
    {
        var itemTransform = itemPool.GetChild(index);
        var item = itemTransform.GetComponent<MerchInfo>().SelfItem;
        if (CanSpendGold(item.Cost))
        {
            SpendGold(item.Cost);
            IM.UpdateItem(PlayerObject, item);
            Destroy(itemTransform.gameObject);
            FillOffer(1);
        }
    }
}
