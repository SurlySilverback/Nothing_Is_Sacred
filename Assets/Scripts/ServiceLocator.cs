using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class ServiceLocator : Singleton<ServiceLocator>
{
	// our garbage implementation for adding icons to inventory when a city produces a good
	public Good weapons;
	public Good drugs;
	public Good exotics;
	public Good food;
	public Good fuel;
	public Good ideas;
	public Good medicine;
	public Good textiles;
	public Good water;

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