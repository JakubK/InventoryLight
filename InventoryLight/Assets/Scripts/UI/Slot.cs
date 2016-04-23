using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class Slot : MonoBehaviour, IDropHandler
    {
        public int ID;
        private Inventory inv;
        public ItemData item;

        public bool Gearable = false;

        void Start()
        {
            inv = transform.parent.parent.GetComponent<Inventory>();
            ID = transform.GetSiblingIndex();
            if (transform.childCount > 0)
            {
                if (this.transform.GetChild(0).GetComponent<ItemData>())
                {
                    item = this.transform.GetChild(0).GetComponent<ItemData>();
                }
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            try
            {
                if (inv.DragAndDropEnabled)
                {
                    ItemData droppedItemData = eventData.pointerDrag.GetComponent<ItemData>();
                    if (inv.SlotList[ID].childCount == 0)
                    {
                        droppedItemData.transform.SetParent(transform);
                        droppedItemData.GetComponent<RectTransform>().anchoredPosition3D = droppedItemData.startPosition;
                        droppedItemData.startParent = transform;
                        droppedItemData.Slot = ID;
                        droppedItemData.inv.ItemList.Remove(droppedItemData);
                        inv.ItemList.Add(droppedItemData);
                        item = droppedItemData;
                        if (Gearable)
                        {
                            droppedItemData.HoldedItem.Gear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
