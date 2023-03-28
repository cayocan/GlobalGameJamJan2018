using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Connection {
	public Plug plug1;
	public Plug plug2;
	public LineRenderer connectionLine;
	public IndividualTimer individualTimer;
	private ConnectionState state = ConnectionState.Call;

    public ConnectionState State 
	{ 
		get
		{
			return state;
		}
		set 
		{
			state = value;
		} 
	}

    public enum ConnectionState {
		Call,
		EndCall,
		Strike
	}

	public Connection (Plug _plug1, Plug _plug2) {
		plug1 = _plug1;
		plug2 = _plug2;
	}

	public Connection (Plug _plug1, Plug _plug2, LineRenderer line, IndividualTimer _timer) {
		plug1 = _plug1;
		plug2 = _plug2;
		connectionLine = line;
		individualTimer = _timer;
	}
}