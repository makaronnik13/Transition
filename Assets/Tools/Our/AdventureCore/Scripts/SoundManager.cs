using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : Singleton<SoundManager> 
{
	private AudioSource musicSource, soundsSource;
	public Action<float> onMusicVolumeChanged = (float f)=>{};
	public Action<float> onSoundVolumeChanged = (float f) => {};

	public float SoundVolume
	{
		get
		{
			if(!PlayerPrefs.HasKey("SoundVolume"))
			{
				PlayerPrefs.SetFloat ("SoundVolume", 0.5f);
			}
			return PlayerPrefs.GetFloat ("SoundVolume");
		}
		set
		{
			if(!PlayerPrefs.HasKey("SoundVolume"))
			{
				PlayerPrefs.SetFloat ("SoundVolume", 0.5f);
			}
			if (PlayerPrefs.GetFloat ("SoundVolume") != value) 
			{
				PlayerPrefs.SetFloat ("SoundVolume", value);
				soundsSource.volume = value;
				onSoundVolumeChanged.Invoke (value);
			}
		}
	}

	public float MusicVolume
	{
		get
		{
			if(!PlayerPrefs.HasKey("MusicVolume"))
			{
				PlayerPrefs.SetFloat ("MusicVolume", 0.5f);
			}

			return PlayerPrefs.GetFloat ("MusicVolume");
		}
		set
		{
			if(!PlayerPrefs.HasKey("MusicVolume"))
			{
				PlayerPrefs.SetFloat ("MusicVolume", 0.5f);
			}
			if(PlayerPrefs.GetFloat("MusicVolume")!= value)
			{
				PlayerPrefs.SetFloat ("MusicVolume", value);
				musicSource.volume = value;
				onMusicVolumeChanged.Invoke (value);
			}
		}
	}

	void Start()
	{
		musicSource = transform.GetChild (0).GetComponent<AudioSource> ();
		soundsSource = transform.GetChild (1).GetComponent<AudioSource> (); 
		soundsSource.volume = SoundVolume;
		musicSource.volume = MusicVolume;
	}

	public void PlaySoundOnce(AudioClip clip)
	{
		soundsSource.PlayOneShot (clip);
	}

	public void SetMusic(AudioClip clip)
	{
		musicSource.Stop ();
		musicSource.clip = clip;
		musicSource.Play ();
	}


}
