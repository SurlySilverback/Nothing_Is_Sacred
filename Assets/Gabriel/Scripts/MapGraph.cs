using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGraph : MonoBehaviour, IEnumerable
{
	[SerializeField]
	private List<City> cities;
    
	public IEnumerator GetEnumerator()
	{
        return cities.GetEnumerator();
	}
}