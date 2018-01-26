using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using System.Linq;
using System;

public class ParamsManager : Singleton<ParamsManager>
{
	public Action<GameParameter, float> OnParamChanged = (GameParameter a, float b)=>{};

	private Dictionary<GameParameter, float> paramsValues = new Dictionary<GameParameter, float>();

	private void ChangeParam(GameParameter param, float value)
	{
		float v = GetParam (param) + value;
		OnParamChanged.Invoke (param, value);
		SetParam (param, v);
	}

	private void SetParam(GameParameter param, float value)
	{
		if(!paramsValues.ContainsKey(param))
		{
			paramsValues.Add (param, param.defaultValue);
		}
		paramsValues [param] = value;
	}

	public float GetParam(GameParameter param)
	{
		if(!paramsValues.ContainsKey(param))
		{
			paramsValues.Add (param, param.defaultValue);
		}

		return paramsValues [param];
	}

	public void ApplyEffect(ParamEffect effect)
	{
		ChangeParam (effect.parameter, effect.value);
	}
}
