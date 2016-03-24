using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Items;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public Item HoldedItem;
    public int Amount;
    public int Slot;

    public List<ItemProperty> Properties;
    private Inventory inv;

    [HideInInspector]
    public Vector3 startPosition;

    [HideInInspector] public Transform startParent;

    void Start()
    {
            inv = transform.parent.parent.parent.GetComponent<Inventory>();
    
            Properties = new List<ItemProperty>();
            foreach (ItemProperty property in HoldedItem.ItemProperties)
            {
                Properties.Add(new ItemProperty(property.PropertyName, property.PropertyValue));
            }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (HoldedItem != null)
        {
            startPosition = transform.GetComponent<RectTransform>().anchoredPosition3D;
            startParent = transform.parent;
            this.transform.SetParent(transform.parent.parent);
            this.transform.position = eventData.position;

            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (HoldedItem != null)
        {
            this.transform.position = eventData.position;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        transform.GetComponent<RectTransform>().anchoredPosition3D = startPosition;

        bool FoundParent = false;
        for (int i = 0; i < inv.SlotList.Count; i++)
        {
            if (transform.parent == inv.SlotList[i])
            {
                FoundParent = true;
            }
        }
        if (FoundParent == false)
        {
            transform.SetParent(startParent);
            transform.GetComponent<RectTransform>().anchoredPosition3D = startPosition;
        }
    }

  

  
}
