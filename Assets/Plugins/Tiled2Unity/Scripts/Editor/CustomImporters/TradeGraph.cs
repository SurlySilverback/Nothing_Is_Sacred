using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeNode : MonoBehaviour
{
    // Market
    // Position
    public string tradeName;
    public Vector2 location;
    public List<TradeNode> neighbors;
}

public class TradeGraph : MonoBehaviour
{
    public List<TradeNode> tradeGraph;


}