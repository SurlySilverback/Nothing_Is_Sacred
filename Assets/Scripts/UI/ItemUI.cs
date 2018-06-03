using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        instance.goodType.text = g.type.ToString();
        instance.price.text = "$ " + ServiceLocator.Instance.GetViewInfo().SelectedMarket.GetPrice(g);
        instance.heat.text = "Heat: " + Mathf.FloorToInt(g.Heat);
        instance.weight.text = "Weight: " + g.Weight;
    }
}