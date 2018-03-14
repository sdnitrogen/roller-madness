using UnityEngine;
using System;
using System.Collections;

public class InitLevel : MonoBehaviour
{
    public UserPreferences userPreferences;
    public Health playerHealth;
    public SpawnGameObjects enemySpawner;
    public SpawnGameObjects coinSpawner;
    public MeshRenderer player;
    
    private GameManager gameManager;
    private GameObject[] coins;
    
	// Use this for initialization
	void Start ()
    {
        if (userPreferences == null)
        {
            UserPreferences[] userPreferencesArray = FindObjectsOfType<UserPreferences>();
            if (userPreferencesArray.Length > 0)
            {
                userPreferences = userPreferencesArray[0];
            }
            else
            {
                Debug.LogError("Missing User Preferences");
            }
        }

        // max lives
        if ((playerHealth == null) && (GameObject.Find("RollerBall") != null))
        {
            GameObject go = GameObject.Find("RollerBall");
            if ((go != null) && (go.GetComponent<Health>() != null))
            {
                playerHealth = go.GetComponent<Health>();
            }
        }
        if (playerHealth != null)
        {
            if (Application.loadedLevel == 1)
            {
                playerHealth.numberOfLives = (int)userPreferences.maxLivesLevel1;
            }
            else if (Application.loadedLevel == 2)    
            {
                playerHealth.numberOfLives = (int)userPreferences.maxLivesLevel2;
            }
        }

        // max randomly spawned enemies
        if ((enemySpawner == null) && (GameObject.Find("Random Enemy Spawner") != null))
        {
            GameObject go = GameObject.Find("Random Enemy Spawner");
            if ((go != null) && (go.GetComponent<SpawnGameObjects>() != null))
            {
                enemySpawner = go.GetComponent<SpawnGameObjects>();
            }
        }
        if (enemySpawner != null)
        {
            if (Application.loadedLevel == 1)
            {
                enemySpawner.spawnedObjectLimit = (int)userPreferences.maxLevel1EnemySpawners;
            }
            else if (Application.loadedLevel == 2)    
            {
                enemySpawner.spawnedObjectLimit = (int)userPreferences.maxLevel2EnemySpawners;
            }
            enemySpawner.transform.gameObject.SetActive(true);
        }

        // max randomly spawned coins
        if ((coinSpawner == null) && (GameObject.Find("Random Coin Spawner") != null))
        {
            GameObject go = GameObject.Find("Random Coin Spawner");
            if ((go != null) && (go.GetComponent<SpawnGameObjects>() != null))
            {
                coinSpawner = go.GetComponent<SpawnGameObjects>();
            }
        }
        if (coinSpawner != null)
        {
            if (Application.loadedLevel == 1)
            {
                coinSpawner.spawnedObjectLimit = (int)userPreferences.maxLevel1CoinSpawners;
            }
            else if (Application.loadedLevel == 2)    
            {
                coinSpawner.spawnedObjectLimit = (int)userPreferences.maxLevel2CoinSpawners;
            }
            coinSpawner.transform.gameObject.SetActive(true);
        }
	
	    coins = GameObject.FindGameObjectsWithTag("Coin");
	    if ((coins != null) && (coins.Length > 0))
	    {
            int numStaticCoins = (int)userPreferences.maxLevel1StaticCoins;
	        if (Application.loadedLevel == 2)    
	        {
	            numStaticCoins = (int)userPreferences.maxLevel2StaticCoins;
	        }
            
            if (numStaticCoins > coins.Length)
            {
                numStaticCoins = coins.Length;
            }

            // if the number of static coins aalowed differs from the number of static coins
            // then randomly activate the amount of static coins set in user preferences
            if (numStaticCoins != coins.Length)
            {
                for (int i = 0; i < coins.Length; i++)
                {
                    coins[i].SetActive(false);
                }
                
                if (numStaticCoins > 0)
                {
                    int numCoins = numStaticCoins;
                    while (numCoins > 0)
                    {
                        int i = (int)UnityEngine.Random.Range(0, coins.Length);
                        if (!coins[i].activeSelf)
                        {
                            coins[i].SetActive(true);
                            if (Application.loadedLevel == 2)
                            {
                                // add the component if not previously added because we do want thie coin to move up and down in space
                                if (coins[i].GetComponent<UpDown>() == null)
                                {
                                    coins[i].AddComponent<UpDown>();
                                }
                                if (coins[i].GetComponent<UpDown>() != null)
                                {
                                    coins[i].GetComponent<UpDown>().StartPingPong(1.0f, 5.0f);
//                                    coins[i].GetComponent<UpDown>().StartPingPong(-4.5f, 4.5f);
                                }
                            }
                            numCoins--;
                        }
                    }
                }
            }
	    }
	
	    gameManager = transform.GetComponent<GameManager>();
	    if (gameManager != null)
	    {
	        if (Application.loadedLevel == 1)
	        {
	            gameManager.beatLevelScore = (int)userPreferences.coinsRequiredToBeatLevel1;
                gameManager.UpdateBeatLevelScore();
	        }
	        else if (Application.loadedLevel == 2)    
	        {
	            gameManager.beatLevelScore = (int)userPreferences.coinsRequiredToBeatLevel2;
    	        gameManager.UpdateBeatLevelScore();
	        }
	    }
	    
        if ((player == null) && (GameObject.Find("RollerBall") != null))
        {
            player = GameObject.Find("RollerBall").GetComponent<MeshRenderer>();
        }

        if ((userPreferences.selectedPlayer < 1) || (userPreferences.selectedPlayer > 3))
        {
            userPreferences.selectedPlayer = 1;
        }
	    if (player != null)
	    {
            player.sharedMaterial = (Material)Resources.Load("Materials/RollerBallMaterial " + userPreferences.selectedPlayer, typeof(Material));
            player.GetComponent<Health>().explosionPrefab = (GameObject)Resources.Load("Prefabs/Player Explode Particle " + userPreferences.selectedPlayer);
	    }
	}
	
	// Update is called once per frame
	void Update()
    {
	}
}
