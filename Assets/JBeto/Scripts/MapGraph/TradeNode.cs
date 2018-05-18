using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TradeEdge
{
    public TradeNode to;
    public int cost;
}

public class TradeNode : MonoBehaviour
{
    [SerializeField]
    private List<TradeEdge> neighbors;
    public Market Market;

    public IEnumerable<TradeEdge> GetNeighbors()
    {
        return neighbors;
    }
}