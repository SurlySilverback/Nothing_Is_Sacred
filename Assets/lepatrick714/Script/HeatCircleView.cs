﻿using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HeatCircleView : MonoBehaviour
{
	[SerializeField]
	private City city; 
	[SerializeField]
	private float chaosMultiplier;
	private SpriteRenderer spriteRenderer; 

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>(); 
	}
	
	public void ToggleVisible(bool isVisible)
    { 
		spriteRenderer.enabled = isVisible; 
	}
    
	// Update is called once per frame
	void Update ()
    {
		float circleSize = city.Chaos * chaosMultiplier / city.MaxChaos;  
		transform.localScale = new Vector3(circleSize * (7/4.0f), circleSize, 1);
		// Debug.Log(circleSize);
	}
}
