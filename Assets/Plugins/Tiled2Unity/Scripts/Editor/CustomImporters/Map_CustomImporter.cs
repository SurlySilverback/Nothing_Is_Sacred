using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Tiled2Unity;


[CustomTiledImporter]
public class Map_CustomImporter : ICustomTiledImporter
{
    // Connects two cities together, identified by string name
    private class Road
    {
        public string Source { get; private set; }
        public string Destination { get; private set; }

        public Road(string source, string dest)
        {
            this.Source = source;
            this.Destination = dest;
        }

        public override bool Equals(object obj)
        {
            Road otherRoad = (Road)obj;
            return (this.Source == otherRoad.Source && this.Destination == otherRoad.Destination) ||
                (this.Source == otherRoad.Destination && this.Destination == otherRoad.Source);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    private struct CityInfo
    {
        public string cityName;
        public Vector2 location;
        public List<string> neighbors;
        public CityInfo(string cityName, Vector2 loc, List<string> neighbors)
        {
            this.cityName = cityName;
            this.location = loc;
            this.neighbors = neighbors;
        }
    }

    private string mapConfigPath = "Assets/Plugins/Tiled2Unity/Scripts/Editor/CustomImporters/PathConfig";
    private List<CityInfo> cities = new List<CityInfo>();
    
    public void HandleCustomProperties(GameObject gameObject,
        IDictionary<string, string> customProperties)
    {
        string tileType;
        customProperties.TryGetValue("Type", out tileType);
        if (tileType == "City")
        {
            string cityName;
            customProperties.TryGetValue("Name", out cityName);
            if (cityName == null)
            {
                Debug.LogError("Import error: City name is missing in custom properties");
            }

            string neighbors;
            customProperties.TryGetValue("Neighbors", out neighbors);
            if (neighbors == null)
            {
                Debug.LogError("Import error: 'Neighbors' is missing in custom properties");
            }
            List<string> cityNeighbors = new List<string>(neighbors.Split(new char[] { '\n' }));

            Vector2 cityPosition = gameObject.transform.GetChild(0).TransformPoint(Vector3.zero);
            cities.Add(new CityInfo(cityName, cityPosition, cityNeighbors));
        }
    }

    private IEnumerable<Road> ConstructRoads()
    {
        HashSet<Road> roads = new HashSet<Road>();
        foreach(CityInfo c in cities)
        {
            foreach(string neighbor in c.neighbors)
            {
                Road r = new Road(c.cityName, neighbor);
                if (!roads.Contains(r))
                {
                    roads.Add(r);
                    yield return r;
                }
            }
        }
        yield return null;
    }

    private void CreateLine(Transform parent, Vector2 startPoint, Vector2 endPoint, float thickness)
    {
        GameObject roadVisual = new GameObject()
        {
            name = "RoadRenderer"
        };
        LineRenderer lineRenderer = roadVisual.AddComponent<LineRenderer>();
        PolygonCollider2D collider = roadVisual.AddComponent<PolygonCollider2D>();
        roadVisual.transform.SetParent(parent);
    }

    public void CustomizePrefab(GameObject prefab)
    {
        GameObject roadsGO = new GameObject()
        {
            name = "Roads"
        };
        int index = prefab.transform.Find("Cities").GetSiblingIndex();
        roadsGO.transform.SetParent(prefab.transform);
        roadsGO.transform.SetSiblingIndex(index);

        GameObject roadVisual= new GameObject()
        {
            name = "RoadRenderer"
        };
        
        foreach (Road r in ConstructRoads())
        {
            
        }
    }
}
