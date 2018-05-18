using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateTime : MonoBehaviour 
{
	private System.DateTime moment; 
	private float timePassed; 

	// Getters
	public static System.DateTime getDate() { 
		return this.moment; 
	}








	// One second of Real time = One Minute of In Game Time 
	void UpdateMin() 
	{ 
		this.moment.AddMinutes(1);
	}

	
	// Update is called once per frame
	void Awake()
	{
		this.moment = new System.DateTime(2013, 9, 15, 12, 0, 0);
	}

	// Use this for initialization
	void Start () 
	{

	}
	void Update () 
	{	
		this.timePassed += Time.deltaTime; 
		if (this.timePassed >= 1) 
		{ 
			UpdateMin(); 
			timePassed -= 1; 
		}
	}
}
