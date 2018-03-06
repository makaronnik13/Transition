using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour {

	public Slider musicSlider, soundSlider;

	void Awake()
	{
		musicSlider.onValueChanged.AddListener(MusicSliderValueChanged);
		soundSlider.onValueChanged.AddListener(SoundSliderValueChanged);
		musicSlider.value = SoundManager.Instance.MusicVolume;
		soundSlider.value = SoundManager.Instance.SoundVolume;
	}

	void MusicSliderValueChanged(float v)
	{
		SoundManager.Instance.MusicVolume = v;
	}

	void SoundSliderValueChanged(float v)
	{
		SoundManager.Instance.SoundVolume = v;
	}
}
