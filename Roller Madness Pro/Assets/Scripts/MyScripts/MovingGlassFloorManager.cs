using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingGlassFloorManager : MonoBehaviour
{
    private List<GameObject> glassFloors;

	private List<Material> materials = new List<Material>();

	private float timer;
    
	// Use this for initialization
	void Start()
    {
	}
    
    public void Awake()
    {
        timer = Time.time;

        materials.Add((Material)Resources.Load("Materials/Glass Material", typeof(Material)));
        materials.Add((Material)Resources.Load("Materials/Mirror Material", typeof(Material)));

        glassFloors = new List<GameObject>(GameObject.FindGameObjectsWithTag("Glass Floor"));

        if ((glassFloors != null) && (glassFloors.Count > 0))
        {
            for (int i = 0; i < glassFloors.Count; i++)
            {
                // get a random value evenly divisible by 0.5f
                float speed = ((float)(((int)(UnityEngine.Random.Range(1.0f, 10.0f) / 0.5f)) * 0.5f));

                if (glassFloors[i].GetComponent<UpDown>() == null)
                {
                    glassFloors[i].AddComponent<UpDown>();
                }
                if (glassFloors[i].GetComponent<UpDown>() != null)
                {
                    glassFloors[i].GetComponent<UpDown>().StartPingPong(-5, 5, speed);
                }
            }
        }
        StartCoroutine(Wait(10.0f));
        UpdateMaterials();
    }

    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }

    void UpdateMaterials()
    {
        StartCoroutine(RandomizeMaterials());
    }

    IEnumerator RandomizeMaterials()
    {
        int rng = (int)UnityEngine.Random.Range (0, 100);
        if (rng < 90)
            rng = 0;
        else    
            rng = 1;
        float delay = 0.25f;
        if (rng == 0)
        {
            delay = (float)UnityEngine.Random.Range (8.0f, 15.0f);
        }

        for (int i = 0; i < glassFloors.Count; i++)
        {
            glassFloors[i].GetComponent<MeshRenderer>().sharedMaterial = materials[rng];
        }
        yield return new WaitForSeconds(delay);
        UpdateMaterials();
    }
	
	// Update is called once per frame
	void Update()
    {
	}
}
