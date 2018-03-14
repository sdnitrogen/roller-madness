using UnityEngine;
using System;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SpawnGameObjects))]
[System.Serializable]
public class SpawnGameObjectsEditor: Editor
{
    private static String[] SpawnTypes = new String[] {"Coin Spawn", "Enemy Spawn"};
    private static String[] spawnOptions = new String[] {"None", "Use Preset Vectors", "Randomize Spawn Locations"};
    private static String[] enemySelections = new String[] {"Enemy", "Evil Enemy"};
    private static bool showFoldOut = false;
    
    public override void OnInspectorGUI()
	{
        DrawDefaultInspector();
        
        SpawnGameObjects csScript = (SpawnGameObjects)target;
        
        SpawnType spawnType = csScript.spawnType;
        spawnType = (SpawnType)EditorGUILayout.EnumPopup("Spawn Type : ", spawnType);
        csScript.spawnType = spawnType;

        if (spawnType == SpawnType.CoinSpawn)
        {
            csScript.tag = "Coin Spawner";
            GameObject spawnPrefab = csScript.spawnPrefab;
            spawnPrefab = (GameObject)EditorGUILayout.ObjectField("Spawn Prefab : ", spawnPrefab, typeof(GameObject), true);
            csScript.spawnPrefab = spawnPrefab;
        }
        else
        {
            csScript.tag = "Enemy Spawner";

            // get the number of objects in the list
            int spawnPrefabsSize = csScript.spawnPrefabs.Count;

            GameObject[] spawnPrefabArray = csScript.spawnPrefabs.ToArray();

            GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
            showFoldOut = EditorGUILayout.Foldout(showFoldOut, "Spawn Prefabs", foldoutStyle);

            if (showFoldOut)
            {
                // display an int field for input
                spawnPrefabsSize = EditorGUILayout.IntField("    Size", spawnPrefabsSize);
    
                if (spawnPrefabsSize > 2)
                {
                    spawnPrefabsSize = 2;
                }
        
                // change the size of the list base on the spawnPrefabsSize value
                if ((spawnPrefabsSize > 0) && (spawnPrefabsSize != spawnPrefabArray.Length))
                {
                    if (spawnPrefabsSize > spawnPrefabArray.Length)
                    {
                        GameObject[] go = null;
                        System.Array.Resize<GameObject>(ref go, spawnPrefabsSize);
                        if (go != null)
                        {
                            for (int i = 0; i < spawnPrefabArray.Length; i++)
                            {
                                go[i] = spawnPrefabArray[i];
                            }
                            System.Array.Resize<GameObject>(ref spawnPrefabArray, go.Length);
                            spawnPrefabArray = go;
                        }
                    }
                    else
                    {
                        GameObject[] go = null;
                        System.Array.Resize<GameObject>(ref go, spawnPrefabsSize);
                        if (go != null)
                        {
                            for (int i = 0; i < spawnPrefabsSize; i++)
                            {
                                go[i] = spawnPrefabArray[i];
                            }
                            System.Array.Resize<GameObject>(ref spawnPrefabArray, spawnPrefabsSize);
                            spawnPrefabArray = go;
                        }
                    }
                }

                if (spawnPrefabsSize > 0)
                {
                    for (int i = 0; i < spawnPrefabsSize; i++)
                    {
                        spawnPrefabArray[i] = (GameObject)EditorGUILayout.ObjectField("    Element " + i.ToString(), spawnPrefabArray[i] , typeof(GameObject), true);
                    }
                    csScript.spawnPrefabs.Clear();
                    csScript.spawnPrefabs = new List<GameObject>(spawnPrefabArray);


                    // if there are more than one spawnPrefabs, add an int field for selection
                    if (spawnPrefabsSize > 1) // 2
                    {
                        bool randomizePrefabs = csScript.randomizePrefabs;
                        // display an bool field for input
                        randomizePrefabs = EditorGUILayout.Toggle("Randomize Prefab", randomizePrefabs);
                        csScript.randomizePrefabs = randomizePrefabs;

                        if (!randomizePrefabs)
                        {
                            int prefabSelection = csScript.prefabSelection;
                            // display an int field for input
                            prefabSelection = EditorGUILayout.IntField("Prefab Selection", prefabSelection);
                            csScript.prefabSelection = prefabSelection;
                        }
                    }
                    else
                    {
                        csScript.randomizePrefabs = false;
                        csScript.prefabSelection = 0;
                    }
                }
            }
        }

        SpawnObjectOptions spawnObjectOptions = csScript.spawnObjectOptions;
        spawnObjectOptions = (SpawnObjectOptions)EditorGUILayout.EnumPopup("Spawn Object Options : ", spawnObjectOptions);
        csScript.spawnObjectOptions = spawnObjectOptions;

        if (spawnObjectOptions == SpawnObjectOptions.RandomizeSpawnLocations)
        {
            csScript.randomizeSpawnLocations = true;
            csScript.usePresetVectors = false;
            csScript.minPosition = EditorGUILayout.Vector3Field("Minimum Spawn Location:", csScript.minPosition);
            csScript.maxPosition = EditorGUILayout.Vector3Field("Maximum Spawn Location:", csScript.maxPosition);
        }
        else if (spawnObjectOptions == SpawnObjectOptions.UsePresetVectors)
        {
            csScript.randomizeSpawnLocations = false;
            csScript.usePresetVectors = true;
        }
        else
        {
            csScript.randomizeSpawnLocations = false;
            csScript.usePresetVectors = false;
        }
    }
}