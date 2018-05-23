using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatCircleController : MonoBehaviour {
	// Max Heat Numerical Value is 1000 
	float maxHeat = 10; 
	public City city; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Debug.Log(city.GetHeat());
		float size_of_circle = city.GetHeat() / 100;  
		transform.localScale = new Vector3(size_of_circle, size_of_circle, 0);
		// if(city.GetHeat() < 1000) { 
			// city.SetHeat(city.GetHeat() + 10); 
		// }
	}
}
