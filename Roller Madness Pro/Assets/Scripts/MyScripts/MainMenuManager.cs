using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    public Camera mainMenuCamera;
    public Camera skyboxCamera;

    public ParticleSystem warpSpeedParticleSystem;

    public Button startGameButton;
    public Button gameSettingsButton;
    public Button quitButton;
    
    private CanvasGroup canvasGroup;

    private AudioSource mainMenuAudioSource;
    private AudioSource startGameAudioSource;
    private AudioSource quitGameAudioSource;

	private float shapeAngleStartValue = 45.0f;
	private float shapeAngleEndValue   = 90.0f;

	// Use this for initialization
	void Start ()
    {
        if ((mainMenuCamera == null) && (GameObject.Find("Main Camera") != null) && (GameObject.Find("Main Camera").GetComponent<Camera>() != null))
        {
            mainMenuCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }

        if ((skyboxCamera == null) && (GameObject.Find("Skybox Camera") != null) && (GameObject.Find("Skybox Camera").GetComponent<Camera>() != null))
        {
            skyboxCamera = GameObject.Find("Skybox Camera").GetComponent<Camera>();
        }

        if ((warpSpeedParticleSystem == null) && (GameObject.Find("Warp Speed Particle System") != null) && (GameObject.Find("Warp Speed Particle System").GetComponent<ParticleSystem>() != null))
        {
            warpSpeedParticleSystem = GameObject.Find("Warp Speed Particle System").GetComponent<ParticleSystem>();
        }

        mainMenuAudioSource = transform.gameObject.GetComponent<AudioSource>();
        startGameAudioSource = startGameButton.GetComponent<AudioSource>();
        quitGameAudioSource = quitButton.GetComponent<AudioSource>();

        canvasGroup = transform.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            if (mainMenuAudioSource != null)
            {
                mainMenuAudioSource.Play();
            }
            StartCoroutine(DoFade(true, 13.5f));
        }
	}
	
	public void StartButtonClicked()
	{
        if (startGameAudioSource != null)
        {
            startGameAudioSource.Play();
        }

		if ((mainMenuCamera != null) && (skyboxCamera != null) && (warpSpeedParticleSystem != null))
		{
		    mainMenuCamera.clearFlags = CameraClearFlags.Skybox;
		    skyboxCamera.transform.gameObject.SetActive(false);

		    warpSpeedParticleSystem.transform.gameObject.SetActive(true);

		    StartCoroutine(WarpSpeedAhead(1, 11.0f));
		}
		else
		{
		    StartCoroutine(LoadLevelDelayed(1, 11.0f));
		}
	}

	public void QuitButtonClicked()
	{
        if (quitGameAudioSource != null)
        {
            quitGameAudioSource.Play();
        }
        StartCoroutine(DoFade(false, 3.5f));
        StartCoroutine(QuitGame());
	}

	IEnumerator QuitGame()
	{
    	yield return new WaitForSeconds(3.5f);
	    #if UNITY_EDITOR
	        UnityEditor.EditorApplication.isPlaying = false;
	    #else
	        Application.Quit();
	    #endif
	}

	IEnumerator LoadLevelDelayed(int sceneToLoad, float fadeTime)
	{
		if (canvasGroup != null)
		{
		    StartCoroutine(DoFade(false, fadeTime));
		}
        
        // delay before level loads
        yield return new WaitForSeconds(fadeTime);
        
		//Load the selected scene, by scene index number in build settings
		Application.LoadLevel (sceneToLoad);
	}

	IEnumerator DoFade(bool fadeIn, float fadeTime)
	{
        bool done = false;
        float fadeIncrement = (1 / (fadeTime * 60.0f));
	    canvasGroup.alpha = fadeIn ? 0 : 1;
	    while (!done)
	    {
    	    yield return new WaitForSeconds(fadeIncrement);
            canvasGroup.alpha += ((fadeIn ? 1.0f : -1.0f) * fadeIncrement);
            if (fadeIn)
                done = canvasGroup.alpha >= 1;
            else    
                done = canvasGroup.alpha <= 0;
	    }
	}

	IEnumerator WarpSpeedAhead(int sceneToLoad, float time)
	{
	    float fadeIncrement = (1 / (time * 60.0f));
	    float speedIncrement = (10.0f / (time * 60.0f));

	    if (canvasGroup != null)
	    {
	        canvasGroup.alpha = 1;
	    }

        // set the starting playback speed
	    warpSpeedParticleSystem.playbackSpeed = 5.0f;

	    while (canvasGroup.alpha > 0)
	    {
		    yield return new WaitForSeconds(fadeIncrement);

	        warpSpeedParticleSystem.playbackSpeed += speedIncrement;

	        canvasGroup.alpha -= fadeIncrement;
	    }

		//Load the selected scene, by scene index number in build settings
		Application.LoadLevel (sceneToLoad);
	}

    void Update()
    {
    }
}
