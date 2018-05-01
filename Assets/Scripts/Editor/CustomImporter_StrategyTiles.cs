using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Tiled2Unity.CustomTiledImporter]
public class CustomImporter_StrategyTiles : Tiled2Unity.ICustomTiledImporter
{

    public void HandleCustomProperties(GameObject gameObject,
        IDictionary<string, string> customProperties)
    {
        if (customProperties.ContainsKey("City"))
        {   
            Node city = gameObject.AddComponent<Node>(); 
            city.Name = customProperties["City"]; 
        }
    }

    public void CustomizePrefab(GameObject prefab)
    {
        // Do nothing
    }
}
