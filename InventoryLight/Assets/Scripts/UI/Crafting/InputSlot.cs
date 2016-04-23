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
<<<<<<< HEAD
            try
            {
=======
>>>>>>> 2421ca45ee6fdbb1692eacf0e3175bdf8542ea4f
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

            }                  cm.Call();      
                
        }
        }
    }

