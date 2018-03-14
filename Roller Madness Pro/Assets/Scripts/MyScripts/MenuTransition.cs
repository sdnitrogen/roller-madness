using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuTransition : MonoBehaviour
{
    public GameObject mainMenuWindow;
    public GameObject settingsWindow;
    public PlayerSelection playerSelection;
    
    private AudioSource audioSource;
    private UserPreferences userPreferences;

	// Use this for initialization
	void Start()
    {
        audioSource = GetComponent<AudioSource>();

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

        if ((mainMenuWindow == null) && (GameObject.Find("Main Menu Window") != null))
        {
            mainMenuWindow = GameObject.Find("Main Menu Window");
        }

        if ((settingsWindow == null) && (GameObject.Find("Settings Window") != null))
        {
            settingsWindow = GameObject.Find("Settings Window");
        }

        if (settingsWindow == null)
        {
            Debug.LogError("Settings Window is missing");
        }
        else
        {
            settingsWindow.transform.position = new Vector3(settingsWindow.transform.position.x, 540, settingsWindow.transform.position.z);
            settingsWindow.SetActive(true);
//            if (settingsWindow.GetComponent<AudioSource>() != null)
        }

	    if ((playerSelection == null) && (GameObject.Find("Player Selection") != null))
	    {
	        GameObject go = GameObject.Find("Player Selection");
	        if ((go != null) && (go.GetComponent<PlayerSelection>() != null))
	        {
	            playerSelection = go.GetComponent<PlayerSelection>();
	        }
	    }

	}
	
	public void TransitionToSettingsWindow()
	{
    	if ((mainMenuWindow != null) && (settingsWindow != null))
    	{
            audioSource.Play();
    	    StartCoroutine(MoveWindow(mainMenuWindow, new Vector3(0, mainMenuWindow.transform.position.y, mainMenuWindow.transform.position.z), new Vector3(900, mainMenuWindow.transform.position.y, mainMenuWindow.transform.position.z)));
        	StartCoroutine(MoveWindow(settingsWindow, new Vector3(settingsWindow.transform.position.x, 540, settingsWindow.transform.position.z),  new Vector3(settingsWindow.transform.position.x, 0, mainMenuWindow.transform.position.z)));
            if (playerSelection != null)
            {
                playerSelection.SetSelectedPlayer();
            }
    	}
	}
	
	public void TransitionToMainMenuWindow()
	{
		audioSource.Play();
		if ((mainMenuWindow != null) && (settingsWindow != null))
		{
		    StartCoroutine(MoveWindow(settingsWindow, new Vector3(settingsWindow.transform.position.x, 0, settingsWindow.transform.position.z), new Vector3(settingsWindow.transform.position.x, 540, settingsWindow.transform.position.z)));
	    	StartCoroutine(MoveWindow(mainMenuWindow, new Vector3(900, mainMenuWindow.transform.position.y, mainMenuWindow.transform.position.z),  new Vector3(0, mainMenuWindow.transform.position.y, mainMenuWindow.transform.position.z)));
		}
	}

	IEnumerator MoveWindow(GameObject screenTomove, Vector3 startPosition, Vector3 endPosition)
	{
        float timer = Time.time;
        screenTomove.transform.position = startPosition;
        
        while (screenTomove.transform.position != endPosition)
        {
            screenTomove.transform.position = Vector3.Lerp(startPosition, endPosition, Time.time - timer);
            yield return new WaitForSeconds(0.04f);
        }
        screenTomove.transform.position = endPosition;
	}

	// Update is called once per frame
	void Update()
    {
	
	}
}
