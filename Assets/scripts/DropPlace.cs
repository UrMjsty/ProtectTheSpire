using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum FieldType
{
    PLAYER_HAND,
    ENEMY_HAND,
    PLAYABLE_ZONE
}

public class DropPlace : MonoBehaviour, IDropHandler
{
    public FieldType fieldType;
    [SerializeField]
    private Transform playerHand;
    private BattleManager BM;
    private CardManager CM;

    private void Awake()
    {
        BM = FindObjectOfType<BattleManager>();
        CM = FindObjectOfType<CardManager>();
    }
    public void OnDrop(PointerEventData eventData)
    {

        CardInfo cardscr = eventData.pointerDrag.GetComponent<CardInfo>();
        //Card card = cardscr.transform
        if (fieldType != FieldType.PLAYABLE_ZONE || cardscr.GetComponent<CardMovement>().defaultParent != playerHand || !BM.IsPlayerTurn)
            return;
        //CC.Use(card);
        CM.Use(cardscr.gameObject, 0);
        //Destroy(cardscr.transform.gameObject);
        
            
    }
}
