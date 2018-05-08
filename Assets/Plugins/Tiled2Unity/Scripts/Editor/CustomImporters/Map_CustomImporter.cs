using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Tiled2Unity;


[CustomTiledImporter]
public class Map_CustomImporter : ICustomTiledImporter
{
    private string mapConfigPath = "Assets/Plugins/Tiled2Unity/Scripts/Editor/CustomImporters/PathConfig";
    // private List<T

    public void HandleCustomProperties(GameObject gameObject,
        IDictionary<string, string> customProperties)
    {
        string tileType;
        customProperties.TryGetValue("Type", out tileType);
        if (tileType == "City")
        {
            Transform cityPosition = gameObject.transform.GetChild(0).transform;
            string cityName;
            customProperties.TryGetValue("Name", out cityName);
            Debug.Log(cityPosition.TransformPoint(Vector3.zero));
            // cityLocations.Add(new CityLocation(cityName, cityPosition.TransformPoint(Vector3.zero)));
        }
    }

    private void ConstructTradeGraph()
    {

    }

    public void CustomizePrefab(GameObject prefab)
    {
        // TradingPaths paths = AssetDatabase.LoadAssetAtPath<TradingPaths>(mapConfigPath);
        // TradeGraph tradeGraph = prefab.AddComponent<TradeGraph>();
        // tradeGraph.
    }
}
