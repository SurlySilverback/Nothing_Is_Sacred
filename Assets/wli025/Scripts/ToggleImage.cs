using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleImage : MonoBehaviour 
{
	[SerializeField]
	private Graphic on;
	[SerializeField]
	private Graphic off;

	public void ToggleGraphic(bool value){
		on.enabled = value;
		off.enabled = !value;
	}
}
