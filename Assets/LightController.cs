using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Light))]

public class LightController : MonoBehaviour
{
	[SerializeField] AudioClip lightToggleSound;
	// Start is called before the first frame update

	[SerializeField] float EnabledLightRange = 10f;
	[SerializeField] bool EnabledByDefault = true;
	private bool LightOn;
	private void OnValidate()
	{
		GetComponent<AudioSource>().clip = lightToggleSound;
		if (EnabledByDefault)
			GetComponent<Light>().range = EnabledLightRange;
		else
			GetComponent<Light>().range = 0;
	}

	private AudioSource soundPlayer;
	private Light lightEmitter;
	private void Start()
	{
		soundPlayer = GetComponent<AudioSource>();
		soundPlayer.playOnAwake = false; //just to make sure.
		lightEmitter = GetComponent<Light>();
		LightOn = EnabledByDefault;
	}

	public void ToggleLightOn()
    {
		soundPlayer.Play();
		lightEmitter.range = EnabledLightRange;
    }

	public void ToggleLightOff()
    {
		soundPlayer.Play();
		lightEmitter.range = 0;
    }

}
