using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Call{
	public Plug plug1;
	public Plug plug2;
	public int repetitions = 0;

	public Call(Plug _plug1, Plug _plug2){
		plug1 = _plug1;
		plug2 = _plug2;
	}
}
