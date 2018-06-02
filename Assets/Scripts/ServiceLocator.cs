using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class ServiceLocator : Singleton<ServiceLocator>
{
    private Player player;
    private ShowDetails viewInfo;
    private MainGovernment mainGov;
    private InGameTime clock;
    private List<City> cities;
    private AlertSystem alertSystem;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        viewInfo = FindObjectOfType<ShowDetails>();
        mainGov = FindObjectOfType<MainGovernment>();
        clock = FindObjectOfType<InGameTime>();
        alertSystem = FindObjectOfType<AlertSystem>();
        cities = new List<City>(FindObjectsOfType<City>());
    }

    public AlertSystem GetAlertSystem()
    {
        return alertSystem;
    }

    public InGameTime GetClock()
    {
        return clock;
    }

    public Player GetPlayer()
    {
        return player;
    }

    public ShowDetails GetViewInfo()
    {
        return viewInfo;
    }

    public IEnumerable<City> GetCities()
    {
        return cities;
    }

    public MainGovernment GetMainGovernment()
    {
        return mainGov;
    }
}