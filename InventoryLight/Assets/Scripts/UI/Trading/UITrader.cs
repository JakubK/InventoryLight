using UnityEngine;
using System.Collections;
using Assets.Scripts.Items;
using System.Collections.Generic;

public class UITrader : MonoBehaviour 
{
    [SerializeField]
    Transform UIPrefab;

    [SerializeField]
    Vector3 StartPosition;

    public ItemDatabase database;
    [SerializeField]
    [HideInInspector]
    public List<WareData> Wares = new List<WareData>();

    void Start()
    {
        GameObject instance = Instantiate(UIPrefab.gameObject);
        instance.transform.SetParent(transform);
        instance.GetComponent<RectTransform>().anchoredPosition3D = StartPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            foreach (var ware in Wares)
            {
                print(ware.WareID.ToString());
            }
        }
    }
}
