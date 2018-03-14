using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SpawnObjectOptions {None, RandomizeSpawnLocations, UsePresetVectors};
public enum EnemySelection {NormalEnemy, EvilEnemy};
public enum SpawnType {CoinSpawn, EnemySpawn};
public class SpawnGameObjects : MonoBehaviour
{
    public float minSecondsBetweenSpawning = 3.0f;
    public float maxSecondsBetweenSpawning = 6.0f;
    
    public Transform chaseTarget;
    
    public int spawnedObjectLimit;
    
    public List<GameObject> spawnedObjects = new List<GameObject>();
    
    // hide in the inspector but make visible to other script files
    [HideInInspector]
    public bool forceDestroy = false;

    // hide in the inspector but make visible to other script files
    [HideInInspector]
    public SpawnType spawnType;

    // hide in the inspector but make visible to other script files
    [HideInInspector]
    public GameObject spawnPrefab;

    // hide in the inspector but make visible to other script files
    // this will be used for the enemy list
    [HideInInspector]
    public List<GameObject> spawnPrefabs = new List<GameObject>();

    // hide in the inspector but make visible to other script files
    [HideInInspector]
    public int prefabSelection = 0;
    
    // hide in the inspector but make visible to other script files
    [HideInInspector]
    public bool randomizePrefabs = false;
    
    // hide in the inspector but make visible to other script files
    [HideInInspector]
    public EnemySelection enemySelection = EnemySelection.NormalEnemy;
    
    // hide in the inspector but make visible to other script files
    [HideInInspector]
    public SpawnObjectOptions spawnObjectOptions = SpawnObjectOptions.None;

    // hide in the inspector but make visible to other script files
    [HideInInspector]
    public bool randomizeSpawnLocations;
    
    // hide in the inspector but make visible to other script files
    [HideInInspector]
    public bool usePresetVectors;
    
    // hide in the inspector but make visible to other script files
    [HideInInspector]
    public bool spawningEnabled = true;
    
    // hide in the inspector but make visible to other script files
	[HideInInspector]
	public Vector3 minPosition = new Vector3(14, 5, 14);

	// hide in the inspector but make visible to other script files
	[HideInInspector]
	public Vector3 maxPosition = new Vector3(-14, 10, -14);
	
	private float savedTime;
	private float secondsBetweenSpawning;

	Vector3[] presetVectors =
	{
        // the first 12 vectors are standard
	    new Vector3(-24.0f, 2.0f, 24.0f),
	    new Vector3(  0.0f, 2.0f, 24.0f),
	    new Vector3( 24.0f, 2.0f, 24.0f),
	    new Vector3(-24.0f, 2.0f,  0.0f),
	    new Vector3(  0.0f, 2.0f,  0.0f),
	    new Vector3( 24.0f, 2.0f,  0.0f),
	    new Vector3(-24.0f, 2.0f, -24.0f),
	    new Vector3(  0.0f, 2.0f, -24.0f),
	    new Vector3( 24.0f, 2.0f, -24.0f),
        
        // the next 28 vectors are extras
        new Vector3( 20.275f, 2.0f,   11.9f),
        new Vector3(   11.9f, 2.0f,    3.5f),
        new Vector3(   11.9f, 2.0f,   -3.5f),
        new Vector3(  -11.9f, 2.0f,    3.5f),
        new Vector3(  -11.9f, 2.0f,   -3.5f),
        new Vector3(   -3.5f, 2.0f,   11.9f),
        new Vector3(    3.5f, 2.0f,   11.9f),
        new Vector3(   -3.5f, 2.0f,  -11.9f),
        new Vector3(    3.5f, 2.0f,  -11.9f),
        new Vector3( -27.33f, 2.0f,   11.9f),
        new Vector3(-20.275f, 2.0f,   11.9f),
        new Vector3(  27.33f, 2.0f,   11.9f),
        new Vector3( -27.33f, 2.0f,  -11.9f),
        new Vector3(-20.275f, 2.0f,  -11.9f),
        new Vector3( 20.275f, 2.0f,  -11.9f),
        new Vector3(  27.33f, 2.0f,  -11.9f),
        new Vector3(   11.9f, 2.0f, -20.31f),
        new Vector3(   11.9f, 2.0f, -27.31f),
        new Vector3(  -11.9f, 2.0f, -20.31f),
        new Vector3(  -11.9f, 2.0f, -27.31f),
        new Vector3(   11.9f, 2.0f,  20.31f),
        new Vector3(   11.9f, 2.0f,  27.31f),
        new Vector3(  -11.9f, 2.0f,  20.31f),
        new Vector3(  -11.9f, 2.0f,  27.31f),
        new Vector3(   11.9f, 2.0f,   11.9f),
        new Vector3(   11.9f, 2.0f,  -11.9f),
        new Vector3(  -11.9f, 2.0f,   11.9f),
        new Vector3(  -11.9f, 2.0f,  -11.9f),
	};

	public SpawnGameObjects()
	{
		spawnedObjectLimit = -1; // -1 means no limit
	}

	// Use this for initialization
	void Start ()
    {
        if (tag == "Enemy Spawner")
        {
            if (spawnPrefabs.Count > 0)
            {
                if ((randomizePrefabs) && (spawnPrefabs.Count > 1))
                {
                    for (int i = 0; i < spawnPrefabs.Count; i++)
                    {
                        if (spawnPrefabs[i] == null)
                        {
                            spawnPrefabs.RemoveAt(i);
                        }
                    }
                    randomizePrefabs = (spawnPrefabs.Count > 1);
                }
                else
                {
                    randomizePrefabs = false;
                }

                if (prefabSelection > spawnPrefabs.Count)
                    prefabSelection = spawnPrefabs.Count - 1;

                if (!randomizePrefabs)
                {
                    if (spawnPrefabs[prefabSelection] != null)
                    {
                        spawnPrefab = spawnPrefabs[prefabSelection];
                    }
                    else
                        spawnPrefab = null;
                }
            }
        }

		savedTime = Time.time;
		secondsBetweenSpawning = UnityEngine.Random.Range (minSecondsBetweenSpawning, maxSecondsBetweenSpawning);
	}
	
	// Update is called once per frame
	void Update () {
		spawnedObjects.RemoveAll(item => item == null);
		if (!forceDestroy) // are we still playing
		{
			if (Time.time - savedTime >= secondsBetweenSpawning) // is it time to spawn again?
			{
				if (spawningEnabled)
			    {
				    MakeThingToSpawn();
				    savedTime = Time.time; // store for next spawn
				    secondsBetweenSpawning = UnityEngine.Random.Range (minSecondsBetweenSpawning, maxSecondsBetweenSpawning);
			    }
			}
   		}
	    else
	    {
	        RemoveSpawnedObjects();
	    }
	}

    // enable/disable object spawning
	public void enableSpawning(bool enabled)
	{
        spawningEnabled = enabled;
        if (!spawningEnabled)
        {
            RemoveSpawnedObjects();
        }
	}

	// remove all spawned object
	bool WeCanRemoveSpawnedObjects()
	{
        if (forceDestroy)
        {
            return true;
        }
        else
        {
	        for (int i = 0; i < (spawnedObjects.Count - 1); i++)
	        {
	            if (spawnedObjects[i] == null)
	            {
	                return true;
	            }
	        }
        }
        return false;
	}

    // remove all spawned object
	public void RemoveSpawnedObjects()
	{
        if (spawnedObjects.Count > 0)
        {
            if (forceDestroy)
            {
                while (spawnedObjects.Count > 0)
                {
                    if (spawnedObjects[0] != null)
                    {
                        Destroy(spawnedObjects[0]);
                    }
                    spawnedObjects.RemoveAt(0);
                }
            }
            else
            {
                for (int i = 0; i < (spawnedObjects.Count - 1); i++)
                {
                    if (spawnedObjects[i] != null)
                    {
                        Destroy(spawnedObjects[i]);
                    }
                }
                spawnedObjects.RemoveAll(item => item == null);
            }
        }
	}

	bool CanSpawn()
	{
	    return ((!forceDestroy) &&
                ((spawnedObjects.Count < spawnedObjectLimit) || (spawnedObjectLimit == -1)));
	}

	GameObject GetSpawnedObject(Vector3 position)
	{
        GameObject go = null;
        if (!forceDestroy)
        {
            if ((randomizePrefabs) && (spawnPrefabs.Count > 0))
            {
                int rng = (int)UnityEngine.Random.Range (0, spawnPrefabs.Count);
                // return an newly created, randomly selected, gameObject
                go = (GameObject)Instantiate(spawnPrefabs[rng], transform.position, transform.rotation);
                go.transform.position = position;
            }
            else if (spawnPrefab != null)
            {
                // return an newly created gameObject
                go = (GameObject)Instantiate(spawnPrefab, transform.position, transform.rotation);
                go.transform.position = position;
            }
            if ((go != null) && (chaseTarget != null) && (go.GetComponent<Chaser> () != null))
            {
                go.GetComponent<Chaser>().SetTarget(chaseTarget);
            }
        }
        return go;
	}

	void MakeThingToSpawn()
	{
		if (CanSpawn())
        {
            Vector3 position = new Vector3(UnityEngine.Random.Range(minPosition.x, maxPosition.x), UnityEngine.Random.Range(minPosition.y, maxPosition.y), UnityEngine.Random.Range(minPosition.z, maxPosition.z) );

            // get the position of the spawned object
		    if ((randomizeSpawnLocations) && (spawnObjectOptions == SpawnObjectOptions.RandomizeSpawnLocations))
		    {
		        position = new Vector3(UnityEngine.Random.Range(minPosition.x, maxPosition.x), UnityEngine.Random.Range(minPosition.y, maxPosition.y), UnityEngine.Random.Range(minPosition.z, maxPosition.z) );
		    }
		    else if ((Application.loadedLevel == 2) && (usePresetVectors) && (spawnObjectOptions == SpawnObjectOptions.UsePresetVectors))
		    {
		        // determine if the position selected includes all vecors in the preset vectors list or just the first 12
		        int maxRngs = (int)UnityEngine.Random.Range (12, presetVectors.Length);
		        int rng = (int)UnityEngine.Random.Range (0, maxRngs);
		        if ((rng >= 0) && (rng < presetVectors.Length))
		        {
                    if (spawnType == SpawnType.CoinSpawn)
		                position = presetVectors[rng];
                    else    
		                position = new Vector3(presetVectors[rng].x, 9.0f, presetVectors[rng].z);
		        }
		    }
            GameObject clone = GetSpawnedObject(position);
        
            if (clone != null)
            {
                // add the clone to the list so we do not exceed our limit
                spawnedObjects.Add(clone);
            }
            if (WeCanRemoveSpawnedObjects())
            {
                RemoveSpawnedObjects();
            }
        }
    }
}
