using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour {

	// Use this for initialization
	void Start () {

		Time.timeScale = 60; // One second of real time equals one minute of in-game time.
	}
	
	// Update is called once per frame
	void Update () {

		// Other functions, specifically buttons, will call and alter TimeScale.
		// The fast forward button will move time forward faster by X.
		// The play button will return time to Time.timeScale = 60.
		// We do not want to call Time.timeScale in every frame, so the Update() method here remains blank.
	}
}
