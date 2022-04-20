using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;


public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private FieldType fieldType;
    private Camera mainCamera;
    private Vector3 offset;
    private bool isDraggable;
    public Transform DefaultParent;
    private BattleManager BM;

    private void Awake()
    {
        mainCamera = Camera.allCameras[0];
        BM = FindObjectOfType<BattleManager>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        
        DefaultParent = transform.parent;
        if (DefaultParent.TryGetComponent(out DropPlace dropPlace) && BM.isPlayerTurn)
        {
            isDraggable = DefaultParent.GetComponent<DropPlace>().fieldType == FieldType.PLAYER_HAND;
        }
        else
            return;
        if (!isDraggable)
            return;
        offset = transform.position - mainCamera.ScreenToWorldPoint(eventData.position);
        transform.SetParent(DefaultParent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable)
            return;
        Vector3 newPos = mainCamera.ScreenToWorldPoint(eventData.position);
        transform.position = newPos + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable)
            return;
        transform.SetParent(DefaultParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

    }

    public void MoveToField(Transform field)
    {
        transform.DOMove(field.position, .5f);
    }

}
