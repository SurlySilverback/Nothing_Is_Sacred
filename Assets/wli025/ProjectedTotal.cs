using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProjectedTotal : MonoBehaviour {
	float total;
	private TextMeshProUGUI projectedTotal;

	void awake(){
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
