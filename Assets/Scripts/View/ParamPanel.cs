using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParamPanel : MonoBehaviour {

	private GameParameter parameter;

	public Image icon;
	public Slider slider;
	public Text valueText;

	public void Init(GameParameter p)
	{
		parameter = p;
		icon.sprite = p.image;
		slider.minValue = p.minValue;
		slider.maxValue = p.maxValue;
		slider.value = p.defaultValue;
		ParamsManager.Instance.OnParamChanged += ParamChanged;
		ParamsManager.Instance.ApplyEffect (new ParamEffect(parameter, 0));
	}

	private void ParamChanged(GameParameter p, float value)
	{
		if (p == parameter) 
		{
			slider.value += value;
			valueText.text = ParamsManager.Instance.GetParam (parameter)+"";
		}
	}
}
