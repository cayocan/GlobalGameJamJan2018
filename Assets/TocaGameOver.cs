using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TocaGameOver : MonoBehaviour {

	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable () {
		AudioManager.instance.PlaySound("Over");
	}
}