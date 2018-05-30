using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour {
    [SerializeField]
    private float timeMultiplier;

	public void FastForward(bool isForward) {
		if (isForward) {
			Time.timeScale = timeMultiplier;
		} else {
			Time.timeScale = 1.0f;
		}
	}
}
