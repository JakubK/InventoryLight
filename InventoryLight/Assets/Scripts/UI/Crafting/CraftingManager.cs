using UnityEngine;
using System.Collections;
using Assets.Scripts.UI.Crafting;
using System.Collections.Generic;
using Assets.Scripts.Items;

namespace Assets.Scripts.UI.Crafting
{
    public class CraftingManager : MonoBehaviour
    {
        public Transform InputContainer;
        public Transform Output;

        OutputSlot output;
        InputSlot[,] input;
        public ItemDatabase database;

        [SerializeField]

        Transform slotPrefab;

        public Transform itemPrefab;

        Transform itemPrefab;



        void Start()
        {

            input = new InputSlot[3, 3];

            if (Output)
            {
                if (Output.GetComponent<OutputSlot>())

            input = new InputSlot[3,3];

            if(Output)
            {
                if(Output.GetComponent<OutputSlot>())

                {
                    output = Output.GetComponent<OutputSlot>();
                }
            }

            if (InputContainer)
            {
                int i = 0;
                for (int y = 0; y < 3; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        GameObject slotInstance = Instantiate(slotPrefab.gameObject);
                        slotInstance.transform.SetParent(InputContainer);
                        slotInstance.transform.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

                        slotInstance.GetComponent<RectTransform>().sizeDelta = slotPrefab.GetComponent<RectTransform>().sizeDelta;

                        input[x, y] = InputContainer.GetChild(i).GetComponent<InputSlot>();
                        i++;
                    }
                }


            if(InputContainer)
            {
                    int i = 0;
                    for(int y = 0;y < 3;y++)
                    {
                        for(int x = 0;x < 3;x++)
                        {
                            GameObject itemInstance = Instantiate(itemPrefab.gameObject);
                            itemInstance.transform.SetParent(InputContainer);
                            itemInstance.transform.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

                            itemInstance.GetComponent<RectTransform>().sizeDelta = itemPrefab.GetComponent<RectTransform>().sizeDelta;

                            input[x, y] = itemInstance.AddComponent<InputSlot>();

                            input[x, y] = InputContainer.GetChild(i).GetComponent<InputSlot>();
                            i++;


                        }
                    }

            }

        }

        public void Call()
        {
            for (int i = 0; i < database.BluePrints.Count; i++)
            {

                string[,] bluePrint = new string[3, 3];

                string[,] bluePrint = new string[3,3];


                bluePrint[0, 0] = database.BluePrints[i].x1y1;
                bluePrint[1, 0] = database.BluePrints[i].x2y1;
                bluePrint[2, 0] = database.BluePrints[i].x3y1;

                bluePrint[0, 1] = database.BluePrints[i].x1y2;
                bluePrint[1, 1] = database.BluePrints[i].x2y2;
                bluePrint[2, 1] = database.BluePrints[i].x3y2;

                bluePrint[0, 2] = database.BluePrints[i].x1y3;
                bluePrint[1, 2] = database.BluePrints[i].x2y3;
                bluePrint[2, 2] = database.BluePrints[i].x3y3;

                int points = 0;

                for (int y = 0; y < 3; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        if (input[x, y].data.HoldedItem.ID == int.Parse(bluePrint[x, y]))
                        {
                            points++;
                        }
                    }
                }

                if (points == 9)
                {
                    print("RESULT");

                    print("The output blueprint is " + database.BluePrints[i].OutputID);

                    output.Call(database.BluePrints[i].OutputID);

                }
            }
        }
    }

}



