using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Dialogue Scriptable Object")]
public class DialogueScriptableObject : ScriptableObject
{
    [Serializable]
    public struct segment
    {
        public string comment;
        public string name;
    }
    public List<segment> entireDialogue;
}
