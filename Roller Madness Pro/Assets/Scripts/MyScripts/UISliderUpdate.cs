using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum UserPrefType {MaxLivesLevel1, MaxLevel1EnemySpawners, MaxLevel1CoinSpawners, MaxLevel1StaticCoins, CoinsRequiredToBeatLevel1, 
                          MaxLivesLevel2, MaxLevel2EnemySpawners, MaxLevel2CoinSpawners, MaxLevel2StaticCoins, CoinsRequiredToBeatLevel2}

public class UISliderUpdate : MonoBehaviour
{
    public UserPreferences userPreferences;
    
    public Slider slider;

    public UserPrefType userPrefType;

    private Text sliderText;
    
    private float lastValue = 0.0f;

    private int[] defaultSliderValues = {1, 1, 2, 6, 5, 2, 4, 4, 6, 10};

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

        sliderText = transform.GetComponent<Text>();
        if (slider == null)
        {
            slider = transform.parent.transform.GetComponent<Slider>();
        }

		if (slider != null)
		{
    		if (PlayerPrefs.HasKey(userPrefType.ToString()))
        		UpdateSliderValueFromPlayerPrefs();
    		else
            {
                slider.value = defaultSliderValues[((int)System.Enum.Parse(typeof(UserPrefType), userPrefType.ToString()))];
                // there was no key for this value so update player preferences with the new value, this will create a key
                UpdatePlayerPreferences();
            }
		}
	}
	
	// retrieve value from player preferences
	void UpdateSliderValueFromPlayerPrefs()
	{
	    switch (userPrefType)
	    {
	        // level 1 variables
	        case UserPrefType.MaxLivesLevel1:
	        {
  	            slider.value = userPreferences.maxLivesLevel1;
	            break;
	        }
	        case UserPrefType.MaxLevel1EnemySpawners:
	        {
	            slider.value = userPreferences.maxLevel1EnemySpawners;
	            break;
	        }
	        case UserPrefType.MaxLevel1CoinSpawners:
	        {
   	            slider.value = userPreferences.maxLevel1CoinSpawners;
	            break;
	        }
	        case UserPrefType.MaxLevel1StaticCoins:
	        {
   	            slider.value = userPreferences.maxLevel1StaticCoins;
	            break;
	        }
	        case UserPrefType.CoinsRequiredToBeatLevel1:
	        {
   	            slider.value = userPreferences.coinsRequiredToBeatLevel1;
	            break;
	        }

	        // level 2 variables
	        case UserPrefType.MaxLivesLevel2:
	        {
   	            slider.value = userPreferences.maxLivesLevel2;
	            break;
	        }
	        case UserPrefType.MaxLevel2EnemySpawners:
	        {
   	            slider.value = userPreferences.maxLevel2EnemySpawners;
	            break;
	        }
	        case UserPrefType.MaxLevel2CoinSpawners:
	        {
	            slider.value = userPreferences.maxLevel2CoinSpawners;
	            break;
	        }
	        case UserPrefType.MaxLevel2StaticCoins:
	        {
   	            slider.value = userPreferences.maxLevel2StaticCoins;
	            break;
	        }
	        case UserPrefType.CoinsRequiredToBeatLevel2:
	        {
   	            slider.value = userPreferences.coinsRequiredToBeatLevel2;
	            break;
	        }
	        default:
	            break;
	    }
	}
	
	 // Update player preference based on current slider
	void UpdatePlayerPreferences()
	{
	    switch (userPrefType)
	    {
	        // level 1 variables
	        case UserPrefType.MaxLivesLevel1:
	        {
	            userPreferences.maxLivesLevel1 = slider.value;
	            break;
	        }
	        case UserPrefType.MaxLevel1EnemySpawners:
	        {
	            userPreferences.maxLevel1EnemySpawners = slider.value;
	            break;
	        }
	        case UserPrefType.MaxLevel1CoinSpawners:
	        {
	            userPreferences.maxLevel1CoinSpawners = slider.value;
	            break;
	        }
	        case UserPrefType.MaxLevel1StaticCoins:
	        {
	            userPreferences.maxLevel1StaticCoins = slider.value;
	            break;
	        }
	        case UserPrefType.CoinsRequiredToBeatLevel1:
	        {
	            userPreferences.coinsRequiredToBeatLevel1 = slider.value;
	            break;
	        }

	        // level 2 variables
	        case UserPrefType.MaxLivesLevel2:
	        {
	            userPreferences.maxLivesLevel2 = slider.value;
	            break;
	        }
	        case UserPrefType.MaxLevel2EnemySpawners:
	        {
	            userPreferences.maxLevel2EnemySpawners = slider.value;
	            break;
	        }
	        case UserPrefType.MaxLevel2CoinSpawners:
	        {
	            userPreferences.maxLevel2CoinSpawners = slider.value;
	            break;
	        }
	        case UserPrefType.MaxLevel2StaticCoins:
	        {
	            userPreferences.maxLevel2StaticCoins = slider.value;
	            break;
	        }
	        case UserPrefType.CoinsRequiredToBeatLevel2:
	        {
	            userPreferences.coinsRequiredToBeatLevel2 = slider.value;
	            break;
	        }
	        default:
	            break;
	    }
	}
	
     // Update is called once per frame
    void Update()
    {
        // only call this blick if the slider value changed
        if ((slider != null) && (sliderText != null) && (slider.value != lastValue))
        {
            // set this to the current slider value for comparison puposes
            lastValue = slider.value;
            sliderText.text = slider.value.ToString();
            UpdatePlayerPreferences();
        }
    }
}