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

    private string mapConfigPath = "Assets/Plugins/Tiled2Unity/Scripts/Editor/CustomImporters/Road.prefab";
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
        yield break;
    }

    private void CreateLine(Transform parent, GameObject emptyLine, Vector2 startPoint, Vector2 endPoint)
    {
        GameObject line = GameObject.Instantiate(emptyLine, parent);
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        PolygonCollider2D collider = line.GetComponent<PolygonCollider2D>();

        // Calculate bounds for collider
        Vector2 direction = endPoint - startPoint;
        Vector2 orthoDir = new Vector2(direction.y, -direction.x).normalized;
        orthoDir *= lineRenderer.startWidth / 2;

        // Set box corners for collider
        Vector2[] corners = new Vector2[4];
        corners[0] = startPoint + orthoDir;
        corners[1] = startPoint - orthoDir;
        corners[2] = endPoint - orthoDir;
        corners[3] = endPoint + orthoDir;
        collider.points = corners;

        // Set line renderer visual
        Vector3[] lineRendererPoints = new Vector3[2];
        lineRendererPoints[0] = startPoint;
        lineRendererPoints[1] = endPoint;
        lineRenderer.SetPositions(lineRendererPoints);
    }
    
    public void CustomizePrefab(GameObject prefab)
    {
        // Set road parent game object
        GameObject roadsGO = new GameObject()
        {
            name = "Roads"
        };
        int index = prefab.transform.Find("Cities").GetSiblingIndex();
        roadsGO.transform.SetParent(prefab.transform);
        roadsGO.transform.SetSiblingIndex(index);
        // Initialize city location lookup for roads
        Dictionary<string, Vector2> cityPositions = new Dictionary<string, Vector2>();
        foreach (CityInfo city in cities)
        {
            Debug.Log(city.cityName);
            cityPositions.Add(city.cityName, city.location);
        }
        // Construct roads
        GameObject line = AssetDatabase.LoadAssetAtPath<GameObject>(mapConfigPath);
        foreach (Road r in ConstructRoads())
        {
            Debug.Log(r.Source + " " + r.Destination);
            CreateLine(roadsGO.transform, line, cityPositions[r.Source], cityPositions[r.Destination]);
        }
    }
}
