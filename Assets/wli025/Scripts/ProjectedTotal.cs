using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProjectedTotal : MonoBehaviour {
	private float total;
	private TextMeshProUGUI projectedTotal;

	void Awake(){
		total = 0;
	}

	// Use this for initialization
	void Start () {
		projectedTotal = GetComponent<TextMeshProUGUI>();
		projectedTotal.text = "PROJECTED TOTAL   $ " + total;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
