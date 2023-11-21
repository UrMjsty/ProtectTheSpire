using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Serialization;


public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private FieldType fieldType;
    private Camera _mainCamera;
    private Vector3 _offset;
    private bool _isDraggable;
    [FormerlySerializedAs("DefaultParent")] public Transform defaultParent;
    [SuppressMessage("ReSharper", "InconsistentNaming")] private BattleManager BM;

    private void Awake()
    {
        _mainCamera = Camera.allCameras[0];
        BM = FindObjectOfType<BattleManager>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        
        defaultParent = transform.parent;
        if (defaultParent.TryGetComponent(out DropPlace dropPlace) && BM.IsPlayerTurn)
        {
            _isDraggable = defaultParent.GetComponent<DropPlace>().fieldType == FieldType.PLAYER_HAND;
        }
        else
            return;
        if (!_isDraggable)
            return;
        _offset = transform.position - _mainCamera.ScreenToWorldPoint(eventData.position);
        transform.SetParent(defaultParent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDraggable)
            return;
        Vector3 newPos = _mainCamera.ScreenToWorldPoint(eventData.position);
        transform.position = newPos + _offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_isDraggable)
            return;
        transform.SetParent(defaultParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

    }

    public void MoveToField(Transform field)
    {
        transform.DOMove(field.position, .5f);
    }

}
