using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HeatCircleView : MonoBehaviour
{
	[SerializeField]
	private City city; 
	private SpriteRenderer spriteRenderer; 

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>(); 
	}
	
	public void ToggleVisible(bool isVisible)
    { 
		spriteRenderer.enabled = isVisible; 
	}

	// Update is called once per frame
	void Update ()
    {
		float circleSize = city.Heat / city.MaxHeat;  
		transform.localScale = new Vector3(circleSize, circleSize, 1);
	}
}
