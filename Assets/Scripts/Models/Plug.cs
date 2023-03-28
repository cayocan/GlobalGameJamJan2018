using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Plug
{
    public string plugID;
    public enum Type { Letter, Number };
    public Type type;
    public enum PlugState {Unplugged, Plugged, Connected}
    public PlugState plugState;
    public Transform transform;
    public SpriteRenderer connector;
    public SpriteRenderer light;
}

