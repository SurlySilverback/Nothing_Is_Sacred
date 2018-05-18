using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T> : IEnumerable {

	List<T> list;
	List<float> priority;

	public PriorityQueue()
	{
		list = new List<T> ();
		priority = new List<float> ();
	}

	public IEnumerator GetEnumerator()
	{
		return list.GetEnumerator ();
	}

	public int Count{ get{ return list.Count; } }

	public int Enqueue(T new_item, float new_item_priority)
	{
		for (uint i = 0; i < priority.Count; ++i) 
		{
			if (new_item_priority > priority[i])
			{
				list.Insert (i, new_item);
				priority.Insert (i, new_item_priority);
				return i;
			}
		}

		list.Add (new_item);
		priority.Add (new_item_priority);
		return list.Count - 1;
	}

	public T Dequeue()
	{
		T item = list [0];
		priority.RemoveAt (0);
		list.RemoveAt (0);
		return item;
	}

	public T Peek()
	{
		return list [0];
	}

	public float PeekPriority()
	{
		return priority [0];
	}
}