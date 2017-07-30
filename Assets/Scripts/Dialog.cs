using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    [Multiline(3)]
    public string Text;
    public string NextText;

}
