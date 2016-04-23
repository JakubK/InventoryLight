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
        CraftingManager cm;

        void Start()
        {
            cm = transform.parent.parent.GetComponent<CraftingManager>();
        }

        public void OnDrop(PointerEventData eventData)
        {
            try
            {
                if (data == null)
                {
                    ItemData droppedItemData = eventData.pointerDrag.GetComponent<ItemData>();
                    droppedItemData.transform.SetParent(transform);
                    droppedItemData.GetComponent<RectTransform>().anchoredPosition3D = droppedItemData.startPosition;
                    droppedItemData.startParent = transform;
                    droppedItemData.inv.ItemList.Remove(droppedItemData);
                    data = droppedItemData;

                    cm.Call();
                }
            }
            catch (Exception ex)
            {

            }
                    cm.Call();      
                }
        }
    }
}
