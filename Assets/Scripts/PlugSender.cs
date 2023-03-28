using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugSender : MonoBehaviour
{
    public Plug plug;

    private void OnMouseDown()
    {
        TableManager.instance.CurrentPlug = plug;
    }
}
