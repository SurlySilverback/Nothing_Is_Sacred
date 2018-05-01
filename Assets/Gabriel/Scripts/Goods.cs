using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Goods : ScriptableObject
{
	[SerializeField]
	protected int maslowIndex;					// Maslow's Hierarchy of Needs observes that human needs fall into a kind of 
												// urgency hierarchy. Certain basic needs (such as air, food, water, 
												// and immediate safety) must be fulfilled before other needs (such as 
												// happiness and self-fulfillment) will be pursued. maslowIndex indicates which
												// level of the urgency hierarchy the good satisfies, thereby determining its
												// selling price under certain circumstances. THIS VALUE DOES NOT CHANGE.
	[SerializedField]
	protected int scarcity;						// Measures the "base rarity" of the Good. Scarce goods have a higher base price,
												// and their price increases in greater increments as their local supply diminishes.
												// THIS VALUE DOES NOT CHANGE.

	[SerializedField]
	protected int basePrice;					// basePrice is the lowest price that a Good can be traded for. It is a reflection of 
												// of the Good's scarcity on the global scale (i.e. outside of the game world).

												// NOTE: Since we're still not sure if scarcity will be used as a multiplicative modifier
												// or if it will be used directly to calculate prices, basePrice is being created to serve
												// that role in the interim. If we can design the game so that scarcity alone can be used 
												// to determine the prices, then basePrice variable will be discarded. THIS VALUE DOES NOT CHANGE.

	[SerializedField]
	protected int baseHeat;						// The heat value of goods may increase or decrease over time, but certain goods will
												// always have a minimum heat value. baseHeat represents the minimum heat value a good 
												// will ever have. For example, Weapons will always have a higher baseHeat value 
												// than Food. THIS VALUE DOES NOT CHANGE.

	public int Heat {get; set;}					// Determines how aggressively the government pursues Goods of this type. The Heat value
												// of a Good determines how much a Unit's heat value will increase when the Unit is detected
												// by government patrols.

												// #futurefeature: The total heat value of a Unit's carried goods will determine the "sentence" 
												// of a Unit when it is captured by government forces. Prison for low heat total, execution for high heat.
}