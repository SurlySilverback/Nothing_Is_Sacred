using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour {

	public void FastForward(bool isForward) {
		if (isForward) {
			Time.timeScale = 5.0f;
		} else {
			Time.timeScale = 1.0f;
		}
	}
}
