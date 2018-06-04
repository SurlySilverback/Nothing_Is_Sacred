using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/// <summary>
/// This is crunch time code, refactor this later
/// </summary>
public class ItemUI : MonoBehaviour
{
    // type
    [SerializeField]
    private TextMeshProUGUI goodType;
    // price
    [SerializeField]
    private TextMeshProUGUI price;
    // Heat
    [SerializeField]
    private TextMeshProUGUI heat;
    // weight
    [SerializeField]
    private TextMeshProUGUI weight;

    private static ItemUI instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public static void SetGood(Good g)
    {
        if (g != null)
        {
            instance.goodType.text = g.type.ToString().ToUpper();
            instance.price.text = "PRICE: $" + Math.Round(ServiceLocator.Instance.GetViewInfo().SelectedMarket.GetPrice(g), 2);
            instance.heat.text = "HEAT: " + Mathf.FloorToInt(g.Heat);
            instance.weight.text = "WEIGHT: " + g.Weight;
        }
    }
}