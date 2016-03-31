using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class Slot : MonoBehaviour, IDropHandler
    {
        public int ID;
        private Inventory inv;

        void Start()
        {
            inv = transform.parent.parent.GetComponent<Inventory>();
            ID = transform.GetSiblingIndex();
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (inv.DragAndDropEnabled)
            {
                ItemData droppedItemData = eventData.pointerDrag.GetComponent<ItemData>();
                if (inv.SlotList[ID].childCount == 0)
                {
                    droppedItemData.transform.SetParent(transform);
                    droppedItemData.GetComponent<RectTransform>().anchoredPosition3D = droppedItemData.startPosition;
                    droppedItemData.startParent = transform;
                }
            }
        }
    }
}
