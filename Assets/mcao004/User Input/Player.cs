using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float startingMoney;
    [SerializeField]
    private float startingComplicity;
    public UnityEvent OnGameOver;

	public float Money { get; set; }			// Stores the running total of the Player's money.
	public float Complicity { get; set; }		// Stores the running total of the Player's Complicity with the government.

    private void Awake()
    {
        Money = startingMoney;
        Complicity = startingComplicity;
        if (OnGameOver != null)
        {
            OnGameOver = new UnityEvent();
        }
    }

    private void OnDestroy()
    {
        OnGameOver.Invoke();
    }
}