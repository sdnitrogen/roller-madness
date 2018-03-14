using UnityEngine;
using System;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(BridgeManager))]
[System.Serializable]
public class BridgeManagerEditor: Editor
{
    
    public override void OnInspectorGUI()
	{
        DrawDefaultInspector();
        
        BridgeManager csScript = (BridgeManager)target;

		if(csScript.movePlatform)
		{
		    GUILayout.BeginHorizontal();
		    csScript.minPlatformY = EditorGUILayout.FloatField("Min Platform Y Position:", csScript.minPlatformY);
		    csScript.maxPlatformY = EditorGUILayout.FloatField("Max Platform Y Position:", csScript.maxPlatformY);
		    GUILayout.EndHorizontal();
		}
		if(csScript.moveRamps)
		{
            GUILayout.BeginHorizontal();
            csScript.minRampY = EditorGUILayout.FloatField("Min Ramp Y Position:", csScript.minRampY);
            csScript.maxRampY = EditorGUILayout.FloatField("Max Ramp Y Position:", csScript.maxRampY);
    		GUILayout.EndHorizontal();
		}
    }
}
