using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class currentlySelected : MonoBehaviour 
{
	[SerializeField]
	private TextMeshProUGUI Heat;

	private Image currentItem;
	public Image CurrentItem
	{
		get 
		{
			return this.currentItem;
		}
		set 
		{
			this.currentItem = value;
			info i = this.currentItem.transform.GetComponent<info>();
			this.Heat.text = 	"GOOD" + '\n' + i.Good + '\n' + '\n' +
								"VALUE" + '\n' + "$" + i.Price + '\n' + '\n' +
								"HEAT" + '\n' + i.Heat + '\n' + '\n' +
								"TYRANNY" + '\n' + i.Tyranny;

		}
	}
}
