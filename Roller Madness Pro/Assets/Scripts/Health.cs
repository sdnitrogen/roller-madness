using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{

    public enum deathAction {loadLevelWhenDead,doNothingWhenDead};

    public float healthPoints = 1f;
    public float respawnHealthPoints = 1f;  // base health points

    public int numberOfLives = 1;           // lives and variables for respawning
    public bool isAlive = true;	

    public GameObject explosionPrefab;

    public deathAction onLivesGone = deathAction.doNothingWhenDead;

    public string LevelToLoad = "";

    private Vector3 respawnPosition;
    private Quaternion respawnRotation;


    // Use this for initialization
    void Start () 
    {
        // store initial position as respawn location
        if (transform.gameObject.tag == "Player")
        {
            // since this is the player set the y of the respawnPosition so that the player is just resting on the ground
            respawnPosition = new Vector3(transform.position.x, 0.5f, transform.position.z);
        }
        else
        {
            respawnPosition = transform.position;
        }
        respawnRotation = transform.rotation;

        if (LevelToLoad == "") // default to current scene 
        {
            LevelToLoad = Application.loadedLevelName;
        }
    }

    // Update is called once per frame
    void Update () 
    {
        // if the object is 'dead'
        if (healthPoints <= 0)
        {
            numberOfLives--; // decrement # of lives, update lives GUI

            if (explosionPrefab != null)
            {
                Instantiate (explosionPrefab, transform.position, Quaternion.identity);
            }

            // respawn
            if (numberOfLives > 0)
            {
                transform.position = respawnPosition; // reset the player to respawn position
                transform.rotation = respawnRotation;
                
                if (transform.gameObject.tag == "Player")
                {
                    transform.gameObject.GetComponent<TrailRenderer>().enabled = false;
                    transform.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    transform.gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
                }
                healthPoints = respawnHealthPoints;   // give the player full health again
            }
            else
            {   // here is where you do stuff once ALL lives are gone)
                isAlive = false;

                switch(onLivesGone)
                {
                    case deathAction.loadLevelWhenDead:
                    Application.LoadLevel (LevelToLoad);
                    break;
                    case deathAction.doNothingWhenDead:
                    // do nothing, death must be handled in another way elsewhere
                    break;
                }
                if (transform.gameObject.tag != "Player")
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void ApplyDamage(float amount)
    {	
        healthPoints = healthPoints - amount;	
    }

    public void ApplyHeal(float amount)
    {
        healthPoints = healthPoints + amount;
    }

    public void ApplyBonusLife(int amount)
    {
        numberOfLives = numberOfLives + amount;
    }

    public void updateRespawn(Vector3 newRespawnPosition, Quaternion newRespawnRotation)
    {
        respawnPosition = newRespawnPosition;
        respawnRotation = newRespawnRotation;
    }
}