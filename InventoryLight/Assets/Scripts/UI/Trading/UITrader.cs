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
    public List<WareData> Wares = new List<WareData>();

    void Start()
    {
        Wares = new List<WareData>();
        GameObject instance = Instantiate(UIPrefab.gameObject);
        instance.transform.SetParent(transform);
        instance.GetComponent<RectTransform>().anchoredPosition3D = StartPosition;
    }

    void Update()
    {

    }
}
