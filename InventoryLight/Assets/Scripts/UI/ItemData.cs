using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Items;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [Serializable]
    public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
    {
        public Item HoldedItem;
        public int Amount;
        public int Slot;
        public string Name;

        public List<ItemProperty> Properties;
        [HideInInspector]
        public Inventory inv;

        [HideInInspector] public Vector3 startPosition;

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
            if (inv != null)
            {
                if (inv.DragAndDropEnabled)
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
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (inv != null)
            {
                if (inv.DragAndDropEnabled)
                {
                    if (HoldedItem != null)
                    {
                        this.transform.position = eventData.position;
                    }
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (inv != null)
            {
                if (inv.DragAndDropEnabled)
                {
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                    transform.GetComponent<RectTransform>().anchoredPosition3D = startPosition;

                    bool foundParent = false;
                    for (int i = 0; i < inv.SlotList.Count; i++)
                    {
                        if (transform.parent == inv.SlotList[i])
                        {
                            foundParent = true;
                        }
                    }
                    if (foundParent == false)
                    {
                        transform.SetParent(startParent);
                        transform.GetComponent<RectTransform>().anchoredPosition3D = startPosition;
                    }
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (inv != null)
            {
                if (eventData.clickCount == inv.OnUseClickCount)
                {
                    this.HoldedItem.Use();
                    this.Amount--;
                    if (Amount > 1)
                    {
                        transform.GetChild(0).GetComponent<Text>().text = Amount.ToString();
                    }
                    else if (Amount == 1)
                    {
                        transform.GetChild(0).GetComponent<Text>().text = "";
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (inv != null)
            {
                inv.Tooltip.gameObject.SetActive(true);
                inv.Tooltip.GetComponent<Tooltip>().Call(HoldedItem);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (inv != null)
            {
                inv.Tooltip.gameObject.SetActive(false);
            }
        }
    }
 }
