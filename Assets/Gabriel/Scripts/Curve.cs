using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class Curve : ScriptableObject {

	[SerializeField]
	private AnimationCurve curve;

	public float Evaluate(float value)
	{
		return curve.Evaluate (value);
	}
}
