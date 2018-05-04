using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CityLocation
{
    public string cityName;
    public Vector2 position;
    public CityLocation(string cityName, Vector2 position)
    {
        this.cityName = cityName;
        this.position = position;
    }
}