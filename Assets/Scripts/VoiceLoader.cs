using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLoader : MonoBehaviour {
	public Animator animator;
	private int index = 0;
	private Call currentCall;
	private bool playedFirstPlug = false;
	public void PlayBeep (string beep) {
		AudioManager.instance.PlaySound (beep);
	}

	public void PlayPlug1 () {

		try {
			currentCall = TableManager.instance.waitingCalls[index];
		} catch (Exception ex) {
			Debug.Log (ex);
		}

		if (TableManager.instance.waitingCalls.Count != 0 && index < TableManager.instance.waitingCalls.Count) {
			AudioManager.instance.PlaySound (currentCall.plug1.plugID);
			playedFirstPlug = true;
		}

	}

	public void PlayPlug2 () {

		if (TableManager.instance.waitingCalls.Count != 0 && index < TableManager.instance.waitingCalls.Count) {
			if (playedFirstPlug) {
				AudioManager.instance.PlaySound (currentCall.plug2.plugID);

				currentCall.repetitions++;
				
				playedFirstPlug = false;
			}
		}

		if (index < TableManager.instance.waitingCalls.Count) {

			if (currentCall.repetitions <= TableManager.instance.maxRepetitions) {
				index++;
			}

		} else {
			index = 0;
		}
	}

	public void EmptyMethod () {
		//Only Make more space in game;	
	}
}