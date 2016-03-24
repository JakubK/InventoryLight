using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour,IDropHandler
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
        ItemData droppedItemData = eventData.pointerDrag.GetComponent<ItemData>();
        if (inv.SlotList[ID].childCount == 0)
        {            droppedItemData.transform.SetParent(transform);
            droppedItemData.GetComponent<RectTransform>().anchoredPosition3D = droppedItemData.startPosition;
            droppedItemData.startParent = transform;
        }
    }
}
