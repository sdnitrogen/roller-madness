using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerSelection : MonoBehaviour
{
    public UserPreferences userPreferences;
    
    public List<Toggle> playersToSelect = new List<Toggle>();
    
    [Range(1, 3)]
    public int selectedPlayer;
    
    private int saveSelectedPlayer = 1;
    private List<Image> playersImages = new List<Image>();
    private Color defaultColor = new Color(0, 200, 255, 255);
    
    private SoundManager soundManager = null;
    
    private bool awake = false;

    // Use this for initialization
    void Start()
    {
        if ((GameObject.Find("Settings Window")) && (GameObject.Find("Settings Window").GetComponent<SoundManager>()))
        {
            soundManager = GameObject.Find("Settings Window").GetComponent<SoundManager>();
        }
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

        for (int i = 0; i < playersToSelect.Count; i++)
        {
            playersImages.Add(GetChildObjectByName(playersToSelect[i].transform.gameObject, "Background").GetComponent<UnityEngine.UI.Image>());
            SetDefaultColor(i);
        }
        if ((playersToSelect != null) && (PlayerPrefs.HasKey("SelectedPlayer")) && ((userPreferences.selectedPlayer - 1) < playersToSelect.Count))
        {
            selectedPlayer = userPreferences.selectedPlayer;
            saveSelectedPlayer = selectedPlayer;
            int index = (selectedPlayer - 1);
            selectedPlayer = userPreferences.selectedPlayer;
            playersToSelect[index].isOn = true;
            playersImages[index].canvasRenderer.SetColor(playersToSelect[index].colors.highlightedColor);
        }
        else
        {
            selectedPlayer = 1;
            playersImages[selectedPlayer - 1].canvasRenderer.SetColor(playersToSelect[selectedPlayer - 1].colors.highlightedColor);
            userPreferences.selectedPlayer = selectedPlayer;
        }
    }                   
    
    public void SetDefaultColor(int index)
    {
        playersImages[index].canvasRenderer.SetColor(defaultColor);
    }                   
    
    public GameObject GetChildObjectByName(GameObject go, String childName)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (go.transform.GetChild(i).gameObject.name == childName)
            {
                return  go.transform.GetChild(i).gameObject;
            }
        }
        // must return something
        return go;
    }
    
    public void SetSelectedPlayer()
    {
        if ((playersToSelect != null) && ((selectedPlayer - 1) < playersToSelect.Count))
        {
            playersToSelect[(selectedPlayer - 1)].isOn = true;
        }
    }
    
    public void Player1Selected()
    {
        if ((soundManager != null) && (selectedPlayer != 1))
        {
            soundManager.ButtonPressed(0);
        }
        SaveSelectedPlayer(1);
    }
	
    public void Player2Selected()
    {
        if ((soundManager != null) && (selectedPlayer != 2))
        {
            soundManager.ButtonPressed(1);
        }
        SaveSelectedPlayer(2);
    }
    
    public void Player3Selected()
    {
        if ((soundManager != null) && (selectedPlayer != 3))
        {
            soundManager.ButtonPressed(2);
        }
        SaveSelectedPlayer(3);
    }
    
    void SaveSelectedPlayer(int whichPlayer)
    {
        SetDefaultColor(saveSelectedPlayer - 1);
        userPreferences.selectedPlayer = whichPlayer;
        selectedPlayer = whichPlayer;
        saveSelectedPlayer = whichPlayer;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (playersImages[(selectedPlayer - 1)].canvasRenderer.GetColor() != playersToSelect[(selectedPlayer - 1)].colors.highlightedColor)
        {
            playersImages[(selectedPlayer - 1)].canvasRenderer.SetColor(playersToSelect[(selectedPlayer - 1)].colors.highlightedColor);
        }
        awake = true;
    }
}
