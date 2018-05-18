using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Market : ScriptableObject
{
    [SerializeField]
    private List<Good> goods;
    [SerializeField]
    private List<float> prices;
    private Dictionary<Good, float> goodsPrices;

    private void Awake()
    {
        Assert.AreEqual(goods.Count, prices.Count, "Number of goods does not match number of prices");
        for (int i = 0; i < goods.Count; i++)
        {
            this.goodsPrices.Add(goods[i], prices[i]);
        }
    }

    public IEnumerable<Good> GetGoods()
    {
        return this.goodsPrices.Keys;
    }

    public float GetPrice(Good good)
    {
        return this.goodsPrices[good];
    }

    public void SetPrice(Good good, float prices)
    {
        this.goodsPrices[good] = prices;
    }
}