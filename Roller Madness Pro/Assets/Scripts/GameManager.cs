using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class SpriteList
{
    public String name;
    
    private int _size;
    public int size
    {
        get { return  _size;}
        set
        {
            if (value > 0)
            {
                // if the size has been increased
                if (value > sprites.Count)
                {
                    for (int i = sprites.Count; i < value; i++)
                    {
                        sprites.Add(new Sprite());
                    }
                }
                else
                {
                    // if the size has been decreased
                    while (sprites.Count > value)
                    {
                        sprites.RemoveAt(sprites.Count - 1);
                    }
                }
            }
            else
                sprites.Clear();
            _size = value;
        }
    }
    public List<Sprite> sprites = new List<Sprite>();
}

public enum GameStates {Playing, Death, GameOver, BeatLevel};
public class GameManager : MonoBehaviour
{

	public static GameManager gm;

	[Tooltip("If not set, the player will default to the gameObject tagged as Player.")]
	public GameObject player;

	public GameStates gameState = GameStates.Playing;

	public int score = 0;
	public int lives = 0;
	public bool canBeatLevel = false;
	public int beatLevelScore = 0;

	public GameObject scoreBoardCanvas;
	public Text scoreBoardDisplay;

    public SpawnGameObjects coinSpawner;
    public SpawnGameObjects enemySpawner;

	public GameObject livesCanvas;
	public Text livesDisplay;

	public GameObject gameStartCanvas;
	public GameObject ExtraHint2;

	public GameObject gameOverCanvas;
	public Text gameOverScoreDisplay;

	[Tooltip("Only need to set if canBeatLevel is set to true.")]
	public GameObject beatLevelCanvas;
	public Text beatLevelDisplay;

	public AudioSource backgroundMusic;

	public AudioClip scene2Loop1;
	public AudioClip scene2Loop2;

	public AudioClip gameOverSFX;

	[Tooltip("Only need to set if canBeatLevel is set to true.")]
	public AudioClip leve1VictorySFX;
	public AudioClip leve2Victory1SFX;
	public AudioClip leve2Victory2SFX;
	public AudioClip beatLevelSFX;

	[Tooltip("Set to true to loop continuously play bonus beatLevelSFX when finishing 2 bonus version.")]
	public bool loopbeatLevelSFX = false;

	public AudioClip levelTransitionSFX;

	public GameObject beatLevelCanvasBonus;
	public RectTransform credits;
	public Image danceImage;
    public float scrollSpeed = 0.3f;

	public List<SpriteList> animations;
	private Coroutine animateSprite = null;
	private bool animating;

	public List<GameObject> walls = new List<GameObject>();
	public bool wallsEnabled = true;

	private Health playerHealth;

	private GameObject miniMapCamera = null;
	private bool miniMapCameraAvailable = false;
	private bool miniMapCameraActive = true;
    
	private Vector3 respawnPosition;
	private Quaternion respawnRotation;

	private UserPreferences userPreferences;
    
	void Start ()
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

        if (GameObject.Find("Minimap Camera"))
        {
            miniMapCamera = GameObject.Find("Minimap Camera");
        }

        if (miniMapCamera == null)
        {
            miniMapCameraActive = false;
            miniMapCameraAvailable = false;
        }
        else if (Application.loadedLevel < 2)
        {
            miniMapCameraActive = false;
            miniMapCameraAvailable = false;
            miniMapCamera.SetActive(false);
        }
        else
        {
            miniMapCameraActive = false;
            miniMapCamera.SetActive(miniMapCameraActive);
            miniMapCameraAvailable = true;
        }

		if (gm == null) 
		{
			gm = gameObject.GetComponent<GameManager>();
		}

		if (player == null)
        {
			player = GameObject.FindWithTag("Player");
		}

		// store initial position as respawn location
        // set the y of the respawnPosition so that the player is just resting on the ground
		respawnPosition = new Vector3(player.transform.position.x, 0.5f, player.transform.position.z);
		respawnRotation = player.transform.rotation;

		playerHealth = player.GetComponent<Health>();

		// setup score display
		Collect (0);

		// make other UI inactive
		gameOverCanvas.SetActive (false);
		if (canBeatLevel)
			beatLevelCanvas.SetActive (false);

	    if (Application.loadedLevel == 2)
	    {
	        if (userPreferences != null)
	        {
	            wallsEnabled = userPreferences.level2WallsEnabed;
	        }
	        else
	        {
	            wallsEnabled = true;
	        }
	        EnableWalls();
	        beatLevelSFX = (wallsEnabled ? leve2Victory1SFX : leve2Victory2SFX);

            if (backgroundMusic != null)
            {
                if (wallsEnabled)
                {
                    if (scene2Loop1 != null)
                    {
                        backgroundMusic.clip = scene2Loop1;
                    }
                }
                else if (scene2Loop2 != null)
                {
                    backgroundMusic.clip = scene2Loop2;
                }
                backgroundMusic.loop = true;
                backgroundMusic.Play();
            }

            if (gameStartCanvas == null)
            {
                gameStartCanvas = GameObject.Find("Game Start Canvas");
            }
            if (gameStartCanvas != null)
            {
                Time.timeScale = 0;
                if (ExtraHint2 == null)
                {
                    ExtraHint2 = GameObject.Find("Extra Hint 2");
                }
                if (ExtraHint2 != null)
                {
                    ExtraHint2.SetActive(!wallsEnabled);
                }
                gameStartCanvas.SetActive(true);
            }
	    }
        else
	    {
	        beatLevelSFX = leve1VictorySFX;
            if (userPreferences != null)
            {
    	        wallsEnabled = true;
            }
            else
            {
                userPreferences.level2WallsEnabed = true;
            }
	    }
	    
        if (userPreferences != null)
	    {
    	    livesDisplay.text = "Lives: " + playerHealth.numberOfLives.ToString ();
	    }
	}

	void UpdateSpawners(bool forceDestroy)
	{
	    if (enemySpawner != null)
        {
            enemySpawner.forceDestroy = forceDestroy;
        }
	    if (coinSpawner != null)
	    {
    	    coinSpawner.forceDestroy = forceDestroy;
	    }
	}

	void Update ()
    {
		switch (gameState)
		{
			case GameStates.Playing:
				if (playerHealth.isAlive == false)
				{
					livesDisplay.text = "Lives: 0";
					// update gameState
                    UpdateSpawners(true);
					gameState = GameStates.Death;

					DisablePlayer();

					gameOverScoreDisplay.text = "Game Over\nCoins collected: " + score.ToString ();

					// switch which GUI is showing		
					scoreBoardCanvas.SetActive(false);
					livesCanvas.SetActive (false);
					gameOverCanvas.SetActive(true);
				}
                else if (canBeatLevel && score>=beatLevelScore)
                {
					// update gameState
					UpdateSpawners(true);
					gameState = GameStates.BeatLevel;
                    
					// hide the player so game doesn't continue playing
					DestroyAllObjects();
//					player.SetActive(false);
                    DisablePlayer();

					// switch which GUI is showing
					gameOverCanvas.SetActive(false);
					scoreBoardCanvas.SetActive(false);
					livesCanvas.SetActive(false);
                    beatLevelDisplay.text = "Level Completed\nCoins collected: " + score.ToString ();
				}
                else
                {
 				    livesDisplay.text = "Lives: " + playerHealth.numberOfLives.ToString ();
                }
				break;
			case GameStates.Death:
				UpdateSpawners(true);
				livesDisplay.text = "Lives: " + playerHealth.numberOfLives.ToString ();
				backgroundMusic.volume -= 0.01f;
				if (backgroundMusic.volume <= 0.0f)
                {
                    backgroundMusic.Stop();
                    backgroundMusic.PlayOneShot(gameOverSFX);
					gameState = GameStates.GameOver;
				}
				break;
			case GameStates.BeatLevel:
                UpdateSpawners(true);
                if (beatLevelSFX != null)
                {
                    backgroundMusic.volume -= 0.01f;
                    if (backgroundMusic.volume <= 0.0f)
                    {
                        backgroundMusic.Stop();
                        backgroundMusic.volume = 1;
                	    if (wallsEnabled)
                        {
                	    	backgroundMusic.PlayOneShot(beatLevelSFX);
                	    	beatLevelCanvas.SetActive(true);
                        }
                	    else
                	    {
                            if (loopbeatLevelSFX)
                            {
                                backgroundMusic.clip = beatLevelSFX;
                	            backgroundMusic.loop = true;
                	            backgroundMusic.Play();
                            }
                            else
                            {
                    	        backgroundMusic.PlayOneShot(beatLevelSFX);
                            }
                	        ShowBonusBeatLevelScreen();
                	    }    
                        gameState = GameStates.GameOver;
                    }
                }
                else
                {
                    UpdateSpawners(true);
                    gameState = GameStates.GameOver;
                }
				break;
			case GameStates.GameOver:
				// nothing
				UpdateSpawners(true);
				break;
		}
	
	    if (miniMapCameraAvailable && Input.GetKeyUp(KeyCode.Tab))
	    {
	        miniMapCameraActive ^= true;
	        miniMapCamera.SetActive(miniMapCameraActive);
	    }
	}

	public void DisablePlayer()
	{
	    miniMapCamera.SetActive(false);
	    player.transform.position = respawnPosition;
	    player.transform.rotation = respawnRotation;
	    player.GetComponent<TrailRenderer>().enabled = false;
	    player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
	    player.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
	    player.GetComponent<Rigidbody>().Sleep();
	    player.GetComponent<UnityStandardAssets.Vehicles.Ball.BallUserControl>().enabled = false;
    	player.GetComponent<Health>().explosionPrefab = null;
	}

	public void DestroyAllObjects()
	{
    }

    public void EnableWalls()
    {
        for (int i = 0; i < walls.Count; i++)
        {
            if (walls[i] != null)
            {
                walls[i].SetActive(wallsEnabled);
            }
        }
    }

	public void ReloadCurrentLevel()
	{
	    if (levelTransitionSFX != null)
	    {
	        if (Application.loadedLevel == 2)
	        {
	            EnableWalls();
	        }
	        backgroundMusic.Stop();
	        backgroundMusic.volume = 1.0f;
	        backgroundMusic.PlayOneShot(levelTransitionSFX);
	    }
	    StartCoroutine(LoadLevelDelayed(Application.loadedLevel, 5.5f));
	}

	public void ReloadCurrentLevel(bool disableWalls)
	{
	    if (Application.loadedLevel == 2)
	    {
	        wallsEnabled = (!disableWalls);
	        if (userPreferences != null)
	        {
                userPreferences.level2WallsEnabed = wallsEnabled;
	        }
	        EnableWalls();
	    }
    	ReloadCurrentLevel();
	}

	public void LoadPreviousLevel()
	{
	    if (levelTransitionSFX != null)
	    {
	        backgroundMusic.Stop();
	        backgroundMusic.volume = 1.0f;
	        backgroundMusic.PlayOneShot(levelTransitionSFX);
	    }
	    StartCoroutine(LoadLevelDelayed(Application.loadedLevel - 1, 5.5f));
	}

	public void LoadNextLevel()
	{
        if (levelTransitionSFX != null)
        {
            backgroundMusic.Stop();
            backgroundMusic.volume = 1.0f;
            backgroundMusic.PlayOneShot(levelTransitionSFX);
        }
	    StartCoroutine(LoadLevelDelayed(Application.loadedLevel + 1, 5.5f));
	}

	IEnumerator LoadLevelDelayed(int sceneToLoad, float fadeTime)
	{
	    // delay before level loads
	    yield return new WaitForSeconds(fadeTime);

		//Load the selected scene, by scene index number in build settings
		Application.LoadLevel(sceneToLoad);
	}

	public void ReturnToMainMenu()
	{
        backgroundMusic.Stop();
	    Application.LoadLevel (0);
	}

	public void CloseGameStartWindow()
	{
	    gameStartCanvas.SetActive(false);
        Time.timeScale = 1;
	}

	public void UpdateBeatLevelScore()
	{
	    if (canBeatLevel)
        {
		    scoreBoardDisplay.text = score.ToString () + " of "+ beatLevelScore.ToString () + " coins collected";
	    }
        else
        {
		    scoreBoardDisplay.text = "Coins: " + score.ToString ();
	    }
	}

	public void Collect(int amount)
    {
		score += amount;
        UpdateBeatLevelScore();
	}

    public void Animate(String iconName, float fps, int loops, float delay)
    {
        int whichAnimation = GetSpriteListFromName(animations, iconName);

        if ((whichAnimation > -1) && (whichAnimation < animations.Count))
        {
            animateSprite = StartCoroutine(AnimateSprite(whichAnimation, fps, loops, delay));
        }
    }

    public int GetSpriteListFromName(List<SpriteList> spriteList, String spriteListName)
    {
        int whichSpriteList = -1;
        for (int i = 0; i < spriteList.Count; i++)
        {
            if ((spriteList[i].name != "") && (spriteList[i].name.ToLower() == spriteListName.ToLower()))
            {
                whichSpriteList = i;
                break;
            }
        }
        return whichSpriteList;
    }

    public IEnumerator AnimateSprite(int whichAnimation, float fps, int loops, float delay)
	{
        yield return new WaitForSeconds(delay);
        float animationdelay;

        if ((danceImage != null) && 
            (animations != null) && 
            (animations.Count > 0) && 
            (animations[whichAnimation] != null) && 
            (fps > 0))
        {
            animationdelay = 1/fps;
            animating = true;
            int i = 0;
            while (animating)
            {
                for (int j = 0; j < animations[whichAnimation].sprites.Count; j++)
                {
                    danceImage.sprite = animations[whichAnimation].sprites[j];
                    yield return new WaitForSeconds(animationdelay);
                }
                if (loops > 0)
                {
                    i++;
                    if (i >= loops)
                    {
                        animating = false;
                    }
                }
            }
        }
    	animating = false;
	}

	public void ShowBonusBeatLevelScreen()
	{
        wallsEnabled = true;
        userPreferences.level2WallsEnabed = wallsEnabled;
        if ((beatLevelCanvasBonus != null) && (credits != null) && (danceImage != null) && (animations != null) && (animations.Count != null))
        {
    	    beatLevelCanvasBonus.SetActive(true);
    	    StartCoroutine(ShowCredits());
        }
        else
            beatLevelCanvas.SetActive(true);
	}

    public IEnumerator ShowCredits()
    {
        float increment = 1.5f;
        float delay = 0.02f;
        float y = -1320;

        credits.anchoredPosition = new Vector2(credits.anchoredPosition.x, y);
        Animate("Dance Moves", 30, 200, 0.1f);
        while (backgroundMusic.isPlaying)
        {
            y += increment;
            if (y >= 120)
            {
                // reset y to gust before the original position to give the appearance of continual scrolling
                y = -1350;
            }
            credits.anchoredPosition = new Vector2(credits.anchoredPosition.x, y);
           yield return new WaitForSeconds(delay);
        }

        if (animateSprite != null)
        {
            animating = false;
            StopCoroutine(animateSprite);
        }
        Application.LoadLevel(0);
    }
}