using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableTrading : MonoBehaviour
{
    private City city;
    private ShowDetails details;

    private void Start()
    {
        city = transform.parent.parent.GetComponent<City>();
        details = ServiceLocator.Instance.GetViewInfo();
    }
    /*
    private void OnTriggerEnter2D()
    {
        city.PeoplesInventory.IsTradeEnabled = true;
        city.GovtInventory.IsTradeEnabled = true;
    }

    private void OnTriggerExit2D()
    {
        city.PeoplesInventory.IsTradeEnabled = false;
        city.GovtInventory.IsTradeEnabled = false;
    }
    */
}