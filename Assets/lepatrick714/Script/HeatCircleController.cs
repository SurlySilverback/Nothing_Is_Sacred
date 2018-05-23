using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatCircleController : MonoBehaviour {
	// Max Heat Numerical Value is 1000 
	float maxHeat = 10; 
	public City city; 
	private SpriteRenderer sr; 

	void Awake()
	{
		sr = GetComponent<SpriteRenderer>(); 
	}

	// Use this for initialization
	void Start () {

	}
	
	public void ToggleVisible(bool isVisible) { 
		sr.enabled = isVisible; 
	}

	// Update is called once per frame
	void Update () {
		// Debug.Log(city.GetHeat());
		float size_of_circle = city.GetHeat() / 100;  
		transform.localScale = new Vector3(size_of_circle, size_of_circle, 0);
	}
}
