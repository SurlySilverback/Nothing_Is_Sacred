using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeGraph : ScriptableObject
{
    private List<TradeNode> nodeList;

    private void Start()
    {
        foreach(TradeNode tn in FindObjectsOfType<TradeNode>())
        {
            this.nodeList.Add(tn);
        }
    }

    private IEnumerable<TradeNode> GetTradeNodes()
    {
        return this.nodeList;
    }
}