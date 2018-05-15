using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class Unit : MonoBehaviour
{
    [SerializeField]
    protected float baseSpeed;					    // Determines the base speed of the unit on roads.
    [SerializeField]
    private Inventory inventory;
    public int Heat { get; set; }                   // Determines how aggressively the government will chase after this unit.
    public int Subtlety { get; set; }               // Determines how effective the unit is at avoiding detection.
    private UnitController unitController;

    private void Awake()
    {
        this.unitController = GetComponent<UnitController>();
    }
    
    // Use this for initialization
    private void Start()
    {
		
	}
	
    public void MoveUnit()
    {

    }

	private void Update()
    {
		
	}
}
