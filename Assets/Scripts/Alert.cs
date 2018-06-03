using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alert : MonoBehaviour
{
	private void Start()
    {
        Button button = transform.GetChild(2).GetComponent<Button>();
        button.onClick.AddListener(delegate { Destroy(gameObject); } );
	}
}