using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAlert : MonoBehaviour {
	[SerializeField] // alert is the ALERT prefab 
	private GameObject alert;

	/* 
	// TESTING PURPOSE ONLY : Amount of time the alert should be shown for
	private float timeAlertShown; 
	private float timePassed;
	*/
	
	private void Awake(){
		/*
		TESTING PURPOSE ONLY :
		this.timeAlertShown = 4f;
		this.timePassed = 0;
		*/
	}
	
	// Update is called once per frame
	void Update () {
		
		/* 
		// TESTING PURPOSE ONLY : If call for an Alert to be shown, do this,
		// create alert every 4 seconds
		this.timePassed += Time.unscaledDeltaTime;
		if( timePassed >= timeAlertShown ){
			AlertPlayer();
			timePassed = 0;
		}
		*/
		
	}

	public void AlertPlayer(){
		StartCoroutine(Fade());
	}

	public IEnumerator Fade(){
		GameObject go = Instantiate(alert, transform);
		yield return new WaitForSeconds(7f);
		Destroy(go);
	}


}
