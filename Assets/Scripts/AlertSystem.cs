﻿using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(VerticalLayoutGroup))]
public class AlertSystem : MonoBehaviour
{
	[SerializeField]
	private GameObject alert;
	[SerializeField]
	private float timeAlertShown; 

	public void AlertPlayer(string alertMessage)
    {
		StartCoroutine(AppearThenDelete(alertMessage));
	}
    
	private IEnumerator AppearThenDelete(string message)
    {
		GameObject go = Instantiate(alert, transform);
        TextMeshProUGUI alertText = UnityUtility.GetSafeComponent<TextMeshProUGUI>(go.transform.GetChild(1).gameObject);
        alertText.text = message;
		yield return new WaitForSecondsRealtime(timeAlertShown);
		Destroy(go);
	}
}