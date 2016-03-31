using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Items;
using UnityEngine.UI;

namespace  Assets.Scripts.UI
{
    public class Tooltip : MonoBehaviour
    {
        Inventory _inventory;
        ItemDatabase _database;

        public Transform HeaderContainter;
        public Transform DataContainer;
        public Transform ItemContainer;

        [SerializeField] private Vector3 startPos;

        void Start()
        {
            if (transform.parent.GetComponent<Inventory>())
            {
                _inventory = GetComponent<Inventory>();
            }
            else
            {
                throw new NotImplementedException("Tooltip must be the child of the GameObject with Inventory component attached");
            }

        }

        public void Call(Item i)
        {
            this.GetComponent<RectTransform>().anchoredPosition3D = startPos;
            HeaderContainter.GetComponent<Text>().text = i.Name;
            DataContainer.GetComponent<Text>().text = i.Description;
            if (ItemContainer != null)
            {
                ItemContainer.GetComponent<Image>().sprite = i.Icon;
            }
        }


    }
}

