using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimizeUI : MonoBehaviour {
	private RectTransform rectTransform;
	private bool isMoving;
	private float h;
	private float amountMoved;
	[SerializeField]
	private float speed;
	private bool isDown;
	private Vector3 startingPosition;
	private Vector3 endingPosition;
	// Use this for initialization
	void Start () {
		// print(rect.height);
	}
	
	// Update is called once per frame
	void Update () {
		if(isMoving){
			if(!isDown){
				transform.Translate(0, -Time.unscaledDeltaTime * speed, 0, Space.World);
				amountMoved += Time.unscaledDeltaTime * speed;
				if(amountMoved >= (h)){
					rectTransform.localPosition = endingPosition;
					isMoving = false;
					isDown = true;
					amountMoved = 0;
				}
			}
			else{
				transform.Translate(0, Time.unscaledDeltaTime * speed, 0, Space.World);
				amountMoved += Time.unscaledDeltaTime * speed;
				if(amountMoved >= (h)){
					rectTransform.localPosition = startingPosition;
					isMoving = false;
					isDown = false;
					amountMoved = 0;
				}
			}

		}
	}

	private void Awake(){
		rectTransform = GetComponent<RectTransform>();
		isMoving = false;
		h = rectTransform.rect.height;
		amountMoved = 0;
		isDown = false;
		startingPosition = transform.localPosition;
		endingPosition = new Vector3(startingPosition.x, startingPosition.y-h, startingPosition.z);

	}

	public void Minimize(){
		isMoving = true;
	}
}