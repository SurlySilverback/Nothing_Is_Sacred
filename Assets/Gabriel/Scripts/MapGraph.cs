using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Object that contains all the cities
public class MapGraph : MonoBehaviour, IEnumerable
{
    [SerializeField]
    private List<City> cities;
    
	public IEnumerator GetEnumerator()
	{
        return cities.GetEnumerator();
	}
}