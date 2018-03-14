using UnityEngine;
using System.Collections;

public class Treasure : MonoBehaviour {

	public int value = 10;
	public GameObject explosionPrefab;
    
    int counter = 0;

	void OnTriggerEnter (Collider other)
	{
        counter++;
		if (other.gameObject.tag == "Player")
        {
			if (GameManager.gm!=null)
			{
				// tell the game manager to Collect
				GameManager.gm.Collect (value);
			}
			
			// explode if specified
			if (explosionPrefab != null) {
				Instantiate (explosionPrefab, transform.position, Quaternion.identity);
			}
			
			// destroy after collection
			Destroy (gameObject);
		}
	}
}
