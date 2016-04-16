using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Crafting
{
    public class InputSlot : MonoBehaviour,IDropHandler
    {
        public ItemData data;
        OutputSlot output;

        void Start()
        {

        }

        public void OnDrop(PointerEventData eventData)
        {
                if (data == null)
                {
                    ItemData droppedItemData = eventData.pointerDrag.GetComponent<ItemData>();
                    droppedItemData.transform.SetParent(transform);
                    droppedItemData.GetComponent<RectTransform>().anchoredPosition3D = droppedItemData.startPosition;
                    droppedItemData.startParent = transform;
                    droppedItemData.inv.ItemList.Remove(droppedItemData);
                    data = droppedItemData;

                    output.Call();
                }
            
        }
    }
}
