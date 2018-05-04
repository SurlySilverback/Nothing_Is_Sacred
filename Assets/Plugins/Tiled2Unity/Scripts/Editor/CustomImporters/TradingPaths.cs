using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TradingPaths : ScriptableObject
{
    [System.Serializable]
    public struct TradeConnection
    {
        public string tradePoint1;
        public string tradePoint2;
    }

    public List<TradeConnection> tradeGraph;
}