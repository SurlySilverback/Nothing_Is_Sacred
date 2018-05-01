using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Unit : ScriptableObject
{
	[SerializeField]
	protected float baseSpeed;				// Determines the base speed of the unit on roads.
	public int Heat {get; set;}					// Determines how aggressively the government will chase after this unit.
	public int Subtlety {get; set;}				// Determines how effective the unit is at avoiding detection.
	public Vector2 Coordinates {get; set;}			// The unit's x-y coordinates on the map. 
	public Vector2Int inventorySize;		// Determines the max capacity carryable by the unit.
	public List<Goods> Inventory;			// The Goods currently carried by the inventory.
}