using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UberAudio;

public class TitleMusic : MonoBehaviour
{
	[SerializeField] string songName;

	void Start () 
	{
		AudioManager.Instance.Play(songName, this.gameObject);
	}
}