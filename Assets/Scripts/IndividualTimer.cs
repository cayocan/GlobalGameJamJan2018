using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualTimer : MonoBehaviour {
	public Connection connection;
	public float callTime;
	public float endedCallTime;

	private void Start() {
		callTime = Random.Range(TableManager.instance.minCallTime, TableManager.instance.maxCallTime);
		endedCallTime = TableManager.instance.endedCallTime;

		StartCoroutine(CallTime(callTime));
	}

	IEnumerator CallTime(float time){
		yield return new WaitForSeconds(time);
		connection.State = Connection.ConnectionState.EndCall;
		
		connection.plug1.light.enabled = false;
		connection.plug2.light.enabled = false;

		Debug.Log("EndCall");
		StartCoroutine(EndedCallTime(endedCallTime));
	}

	IEnumerator EndedCallTime(float time){
		yield return new WaitForSeconds(time);
		
		TableManager.instance.TryCloseConnection(connection.plug1);

		TableManager.instance.MakeStrike();

		Debug.Log("Strike");
		connection.State = Connection.ConnectionState.Strike;
	}
}
