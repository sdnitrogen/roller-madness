using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class UserPreferences : MonoBehaviour
{

    [SerializeField]
    public float _maxLivesLevel1;
    public float maxLivesLevel1
    {
        get { return PlayerPrefs.GetFloat("MaxLivesLevel1") ; }
        set { _maxLivesLevel1 = value; PlayerPrefs.SetFloat("MaxLivesLevel1", _maxLivesLevel1); PlayerPrefs.Save(); }
    }

    [SerializeField]
    public float _maxLevel1EnemySpawners;
    public float maxLevel1EnemySpawners
    {
        get { return PlayerPrefs.GetFloat("MaxLevel1EnemySpawners"); }
        set { _maxLevel1EnemySpawners = value; PlayerPrefs.SetFloat("MaxLevel1EnemySpawners", _maxLevel1EnemySpawners); PlayerPrefs.Save(); }
    }

    [SerializeField]
    public float _maxLevel1CoinSpawners;
    public float maxLevel1CoinSpawners
    {
        get { return PlayerPrefs.GetFloat("MaxLevel1CoinSpawners"); }
        set { _maxLevel1CoinSpawners = value; PlayerPrefs.SetFloat("MaxLevel1CoinSpawners", _maxLevel1CoinSpawners); PlayerPrefs.Save(); }
    }

    [SerializeField]
    public float _maxLevel1StaticCoins;
    public float maxLevel1StaticCoins
    {
        get { return PlayerPrefs.GetFloat("MaxLevel1StaticCoins"); }
        set { _maxLevel1StaticCoins = value; PlayerPrefs.SetFloat("MaxLevel1StaticCoins", _maxLevel1StaticCoins); PlayerPrefs.Save(); }
    }

    [SerializeField]
    public float _coinsRequiredToBeatLevel1;
    public float coinsRequiredToBeatLevel1
    {
        get { return PlayerPrefs.GetFloat("CoinsRequiredToBeatLevel1"); }
        set { _coinsRequiredToBeatLevel1 = value; PlayerPrefs.SetFloat("CoinsRequiredToBeatLevel1", _coinsRequiredToBeatLevel1); PlayerPrefs.Save(); }
    }

    [SerializeField]
    public float _maxLivesLevel2;
    public float maxLivesLevel2
    {
        get { return PlayerPrefs.GetFloat("MaxLivesLevel2"); }
        set { _maxLivesLevel2 = value; PlayerPrefs.SetFloat("MaxLivesLevel2", _maxLivesLevel2); PlayerPrefs.Save(); }
    }

    [SerializeField]
    public float _maxLevel2EnemySpawners;
    public float maxLevel2EnemySpawners
    {
        get { return PlayerPrefs.GetFloat("MaxLevel2EnemySpawners"); }
        set { _maxLevel2EnemySpawners = value; PlayerPrefs.SetFloat("MaxLevel2EnemySpawners", _maxLevel2EnemySpawners); PlayerPrefs.Save(); }
    }

    [SerializeField]
    public float _maxLevel2CoinSpawners;
    public float maxLevel2CoinSpawners
    {
        get { return PlayerPrefs.GetFloat("MaxLevel2CoinSpawners"); }
        set { _maxLevel2CoinSpawners = value; PlayerPrefs.SetFloat("MaxLevel2CoinSpawners", _maxLevel2CoinSpawners); PlayerPrefs.Save(); }
    }

    [SerializeField]
    public float _maxLevel2StaticCoins;
    public float maxLevel2StaticCoins
    {
        get { return PlayerPrefs.GetFloat("MaxLevel2StaticCoins"); }
        set { _maxLevel2StaticCoins = value; PlayerPrefs.SetFloat("MaxLevel2StaticCoins", _maxLevel2StaticCoins); PlayerPrefs.Save(); }
    }

    [SerializeField]
    public float _coinsRequiredToBeatLevel2;
    public float coinsRequiredToBeatLevel2
    {
        get { return PlayerPrefs.GetFloat("CoinsRequiredToBeatLevel2"); }
        set { _coinsRequiredToBeatLevel2 = value; PlayerPrefs.SetFloat("CoinsRequiredToBeatLevel2", _coinsRequiredToBeatLevel2); PlayerPrefs.Save(); }
    }

    [SerializeField]
    public int _selectedPlayer;
    public int selectedPlayer
    {
        get { return PlayerPrefs.GetInt("SelectedPlayer"); }
        set { _selectedPlayer = value; PlayerPrefs.SetInt("SelectedPlayer", _selectedPlayer); PlayerPrefs.Save(); }
    }

    [SerializeField]
    public bool _level2WallsEnabed;
    public bool level2WallsEnabed
    {
        get { return (PlayerPrefs.GetInt("Level2WallsEnabed") > 0); }
        set { _level2WallsEnabed = value; PlayerPrefs.SetInt("Level2WallsEnabed", (_level2WallsEnabed ? 1 : 0)); PlayerPrefs.Save(); }
    }

    private int[] defaultPreferences = {1, 1, 2, 6, 5, 2, 4, 4, 6, 10};

	// Use this for initialization
	void Start()
    {
        maxLivesLevel1 = ValidateUseserPreference(UserPrefType.MaxLivesLevel1);
        maxLevel1EnemySpawners = ValidateUseserPreference(UserPrefType.MaxLevel1EnemySpawners);
        maxLevel1CoinSpawners = ValidateUseserPreference(UserPrefType.MaxLevel1CoinSpawners);
        maxLevel1StaticCoins = ValidateUseserPreference(UserPrefType.MaxLevel1StaticCoins);
        coinsRequiredToBeatLevel1 = ValidateUseserPreference(UserPrefType.CoinsRequiredToBeatLevel1);
        maxLivesLevel2 = ValidateUseserPreference(UserPrefType.MaxLivesLevel2);
        maxLevel2EnemySpawners = ValidateUseserPreference(UserPrefType.MaxLevel2EnemySpawners);
        maxLevel2CoinSpawners = ValidateUseserPreference(UserPrefType.MaxLevel2CoinSpawners);
        maxLevel2StaticCoins = ValidateUseserPreference(UserPrefType.MaxLevel2StaticCoins);
        coinsRequiredToBeatLevel2 = ValidateUseserPreference(UserPrefType.CoinsRequiredToBeatLevel2);

        if (PlayerPrefs.HasKey("SelectedPlayer"))
        {
            selectedPlayer = PlayerPrefs.GetInt("SelectedPlayer");
        }
        else
        {
            selectedPlayer = 1;
        }

        if ((selectedPlayer < 1) || (selectedPlayer > 3))
        {
            selectedPlayer = 1;
        }

    	if (PlayerPrefs.HasKey("Level2WallsEnabed"))
    	{
    	    level2WallsEnabed = (PlayerPrefs.GetInt("Level2WallsEnabed") > 0);
    	}
    	else
    	{
    	    level2WallsEnabed = true;
    	}

	    if (Application.loadedLevel < 2)
	    {
	        level2WallsEnabed = true;
	    }
	}

	// Use this for initialization
	float ValidateUseserPreference(UserPrefType userPrefType)
	{
	    int userPrefIndex = ((int)System.Enum.Parse(typeof(UserPrefType), userPrefType.ToString()));
	    float preference = defaultPreferences[userPrefIndex];
	    if (PlayerPrefs.HasKey(userPrefType.ToString()))
	    {
		    preference = GetValueFromPlayerPrefs(userPrefType);
	    }
	    if (preference < defaultPreferences[userPrefIndex])
	    {
	        preference = defaultPreferences[userPrefIndex];
	    }
	    return preference;
	}
	
	// retrieve value from player preferences
	float GetValueFromPlayerPrefs(UserPrefType userPrefType)
	{
	    switch (userPrefType)
	    {
	        // level 1 variables
	        case UserPrefType.MaxLivesLevel1:
	        {
  	            return maxLivesLevel1;
	        }
	        case UserPrefType.MaxLevel1EnemySpawners:
	        {
	            return maxLevel1EnemySpawners;
	        }
	        case UserPrefType.MaxLevel1CoinSpawners:
	        {
   	            return maxLevel1CoinSpawners;
	        }
	        case UserPrefType.MaxLevel1StaticCoins:
	        {
   	            return maxLevel1StaticCoins;
	        }
	        case UserPrefType.CoinsRequiredToBeatLevel1:
	        {
   	            return coinsRequiredToBeatLevel1;
	        }

	        // level 2 variables
	        case UserPrefType.MaxLivesLevel2:
	        {
   	            return maxLivesLevel2;
	        }
	        case UserPrefType.MaxLevel2EnemySpawners:
	        {
   	            return maxLevel2EnemySpawners;
	        }
	        case UserPrefType.MaxLevel2CoinSpawners:
	        {
	            return maxLevel2CoinSpawners;
	        }
	        case UserPrefType.MaxLevel2StaticCoins:
	        {
   	            return maxLevel2StaticCoins;
	        }
	        case UserPrefType.CoinsRequiredToBeatLevel2:
	        {
   	            return coinsRequiredToBeatLevel2;
	        }
	        default:
                return 1;
	    }
	}
}