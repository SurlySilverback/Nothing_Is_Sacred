using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using UnityEngine.Events;
using UnityEngine.Assertions;

public class Player : MonoBehaviour
{
    [SerializeField]
    private ViewInformation viewInfo;

    [SerializeField]
    private float startingMoney;
    [SerializeField]
    private float startingComplicity;
    public UnityEvent OnGameOver;
    [SerializeField]
    private MapGraph cityGraph;

    [Space(10)]
    [Header("Unit")]
    [SerializeField]
    private GameObject unit;
    [SerializeField]
    private float unitPrice;

	public float Money { get; set; }			// Stores the running total of the Player's money.
	public float Complicity { get; set; }		// Stores the running total of the Player's Complicity with the government.

    private void Awake()
    {
        Money = startingMoney;
        Complicity = startingComplicity;
        if (OnGameOver == null)
        {
            OnGameOver = new UnityEvent();
        }
    }

    private void Start()
    {

    }

    public bool TryToBuyGood(Good g)
    {
        float price = viewInfo.SelectedMarket.GetPrice(g);
        if (Money - price >= 0)
        {
            Money -= price;
            return true;
        }
        return false;
    }

    public void SellGood(Good g)
    {
        float price = viewInfo.SelectedMarket.GetPrice(g);
        Money += price;
    }

    private void OnDestroy()
    {
        OnGameOver.Invoke();
    }

    public void PurchaseUnit()
    {
        if (Money >= unitPrice)
        {
            Money -= unitPrice;
            Instantiate(unit);
        }
        // text.text = "Money: " + Money + "";
    }
}