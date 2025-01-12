using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

	public Audio[] sounds;
	
	public float minVolume = 0.0001f; // Minimum volume when the object is stationary
	public float maxVolume = 1.0f; // Maximum volume when the object is at maximum velocity
	public float maxVelocity = 100.0f; // Velocity at which the volume should be at max
	
	void Awake ()
	{
		if (instance != null)
		{
			Destroy(gameObject);
			return;
		} else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Audio a in sounds)
		{
			a.source = gameObject.AddComponent<AudioSource>();
			a.source.clip = a.clips[0];
			a.source.volume = a.volume;
			a.source.pitch = a.pitch;
			a.source.loop = a.loop;
			a.source.outputAudioMixerGroup = a.mixer;
		}

		Play("MainMenu");
	}

	public void Play(string audioName)
	{
		Audio a = Array.Find(sounds, item => item.name == audioName);

		if(a.clips.Count > 0)
		{
			a.source.clip = a.clips[UnityEngine.Random.Range(0, a.clips.Count)];
		}

		a.source.Play();
	}
	
	public void PlayAdaptive(string audioName, float intesity)
	{
		Audio a = Array.Find(sounds, item => item.name == audioName);
		
		if(a.clips.Count > 0)
		{
			a.source.clip = a.clips[UnityEngine.Random.Range(0, a.clips.Count)];
		}

		float volume = Mathf.Lerp(minVolume, maxVolume, intesity / maxVelocity);
		
		a.source.volume = volume;
		
		a.source.Play();
	}
	public void Stop(string sound)
	{
		Audio a = Array.Find(sounds, item => item.name == sound);
		a.source.Stop();
	}
}