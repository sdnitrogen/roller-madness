using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//public Enum SoundEffects {SlidingGlassDoor}
public class SoundManager : MonoBehaviour
{

	public AudioSource audioSource;
	public AudioClip knobClick;
	public AudioClip buttton1Pressed;
	public AudioClip buttton2Pressed;
	public AudioClip buttton3Pressed;
    
    private List<AudioClip> buttonClips = new List<AudioClip>();

    private bool awake = false;

	// Use this for initialization
	void Start()
    {
	    buttonClips.Add(buttton1Pressed);
	    buttonClips.Add(buttton2Pressed);
	    buttonClips.Add(buttton3Pressed);
	}
	
	// Update is called once per frame
	public void Awake()
	{
	}
	
	// Update is called once per frame
	public void SliderChanged()
	{
        if ((awake) && (audioSource != null) && (knobClick != null))
        {
            audioSource.PlayOneShot(knobClick);
        }
	}
	
	public void ButtonPressed(int which)
	{
	    if ((awake) && (audioSource != null) && (buttonClips[which] != null))
	    {
	        audioSource.PlayOneShot(buttonClips[which]);
	    }
	}
	
	// Update is called once per frame
	void Update()
    {
        awake = true;
	}
}
