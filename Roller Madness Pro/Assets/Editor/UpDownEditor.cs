using UnityEngine;
using System;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(UpDown))]
[System.Serializable]
public class UpDownEditor: Editor
{
    
    public override void OnInspectorGUI()
	{
        DrawDefaultInspector();
        
        UpDown csScript = (UpDown)target;

		if(csScript.useTimeFactor)
		{
            float timeFactor = csScript.timeFactor;
            timeFactor =  EditorGUILayout.Slider("Time Factor:", timeFactor, 0.5f, 10.0f);
            csScript.timeFactor = timeFactor;
		}
    }
}