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

    private void OnTriggerStay2D()
    {
        if ((details.SelectedMarket as City) == city)
        {
            city.PeoplesInventory.IsTradeEnabled = true;
            city.GovtInventory.IsTradeEnabled = true;
        }
        else
        {
            city.PeoplesInventory.IsTradeEnabled = false;
            city.GovtInventory.IsTradeEnabled = false;
        }
    }

    private void OnTriggerExit2D()
    {
        city.PeoplesInventory.IsTradeEnabled = false;
        city.GovtInventory.IsTradeEnabled = false;
    }
}