using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class BridgeManager : MonoBehaviour
{
    public GameObject platform;
    public GameObject ramp1;
    public GameObject ramp2;

    public bool movePlatform;
    public bool moveRamps;
    
    public bool autoStartPlatform;
    public bool autoStartRamps;

    // hide in the inspector but make visible to other script files
	[HideInInspector]
	public float minPlatformY = 0.0f;

	// hide in the inspector but make visible to other script files
	[HideInInspector]
	public float maxPlatformY = 3.62f;
    
    // hide in the inspector but make visible to other script files
	[HideInInspector]
	public float minRampY = 0.0f;

	// hide in the inspector but make visible to other script files
	[HideInInspector]
	public float maxRampY = 1.829f;
    
    public void Awake()
    {
        if ((autoStartPlatform) && (platform != null) && (minPlatformY != maxPlatformY))
        {
            if (platform.GetComponent<UpDown>() == null)
            {
                platform.AddComponent<UpDown>();
            }
            if (platform.GetComponent<UpDown>() != null)
            {
                platform.GetComponent<UpDown>().StartPingPong(minPlatformY, maxPlatformY);
            }
        }
        if ((autoStartRamps) && (minRampY != maxRampY))
        {
            if (ramp1 != null)
            {
                if (ramp1.GetComponent<UpDown>() == null)
                {
                    ramp1.AddComponent<UpDown>();
                }
                if (ramp1.GetComponent<UpDown>() != null)
                {
                    ramp1.GetComponent<UpDown>().StartPingPong(minRampY, maxRampY);
                }
            }
            if (ramp2 != null)
            {
                if (ramp2.GetComponent<UpDown>() == null)
                {
                    ramp2.AddComponent<UpDown>();
                }
                if (ramp2.GetComponent<UpDown>() != null)
                {
                    ramp2.GetComponent<UpDown>().StartPingPong(minRampY, maxRampY);
                }
            }
        }
    }
}