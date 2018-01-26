using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class ParamsManager : Singleton<ParamsManager>
{
	//public Action<GameParameter, float> OnParamChanged = (null, null)=>{};

	private Dictionary<GameParameter, float, float> paramsValues = new Dictionary<GameParameter, float, float>();

	private void ChangeParam(GameParameter param, float value)
	{
		float v = GetParam (param) + value;
		OnParamChanged.Invoke (param, value, v);
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

		return paramsValues (param);
	}

	public void ApplyEffect(ParamEffect effect)
	{
		ChangeParam (effect.parameter, effect.value);
	}
}
