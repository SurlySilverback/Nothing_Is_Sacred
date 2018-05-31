using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    [SerializeField]
    private float timeMultiplier;

	public void FastForward(bool shouldFastForward)
    {
        Time.timeScale = shouldFastForward ? timeMultiplier : 1f;
	}
}