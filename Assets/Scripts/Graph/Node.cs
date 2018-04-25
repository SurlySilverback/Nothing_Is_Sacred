using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodes : MonoBehaviour {
	public string Name { get; set; } 
	public Vector2 Position { get; set; } 
	private List<Edge> ListOfEdges;
}