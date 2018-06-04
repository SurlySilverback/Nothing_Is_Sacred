using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(CircleCollider2D))]
public class SmugglePoint : MonoBehaviour, IMarket
{
    public List<Good> products;

    public float GetPrice(Good g)
    {
        Good found = products.Single<Good>(x => x == g);
        if (found == null)
            return 1;
        else
        {
            float basePrice = found.GetBasePrice();
            return found.GetBasePrice() + Random.Range(0, basePrice * .4f);
        }
    }
}