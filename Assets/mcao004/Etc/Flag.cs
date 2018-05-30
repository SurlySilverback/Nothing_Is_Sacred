using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Flag : MonoBehaviour {
	SpriteRenderer sprite;

	public void ChangeColor(Color c) {
		Debug.Log (c);
		sprite.color = c;
	}

	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
