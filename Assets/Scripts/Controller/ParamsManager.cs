﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ParamsManager : Singleton<ParamsManager>
{
    public Dialog[] trackingDialogs;

	public Action<GameParameter, float> OnParamChanged = (GameParameter a, float b)=>{};

	private Dictionary<GameParameter, float> paramsValues = new Dictionary<GameParameter, float>();

    public Dictionary<GameParameter, float> Params
    {
        get
        {
            return paramsValues;
        }
    }

	public Dictionary<string, float> ParamsStrings
	{
		get
		{
			Dictionary<string, float> result = new Dictionary<string, float> ();
			foreach(KeyValuePair<GameParameter, float> pair in Params)
			{
				result.Add (pair.Key.name, pair.Value);
			}
			return result;
		}
	}


    private void Start()
	{
		TransmissionManager.Instance.OnPathGo += PathGo;
		TransmissionManager.Instance.OnNodeIn += NodeIn;
        GameScenesManager.Instance.OnSaveLoaded += SaveLoaded;
    }

    private void SaveLoaded(SaveStruct obj)
    {
        foreach (SaveStruct.StringPair pair in obj.savedParameters)
        {
            foreach(Dialog dialog in trackingDialogs)
            {
                if (dialog.paramCollection)
                {
                    foreach (GameParameter param in dialog.paramCollection.Parameters)
                    {
                        if (param.name == pair.S)
                        {
                            SetParam(param, pair.V);
                        }
                    }
                }
            }
        }
    }

    private void PathGo(DialogStatePath path)
	{
		foreach(ParamEffect pe in path.effects)
		{
			ApplyEffect (pe);
		}
	}

	private void NodeIn(DialogStateNode node)
	{
		foreach(ParamEffect pe in node.effects)
		{
			ApplyEffect (pe);
		}
	}

	private void ChangeParam(GameParameter param, float value)
	{
		float v = GetParam (param) + value;
		SetParam (param, v);
	}

	private void SetParam(GameParameter param, float value)
	{
		if(!paramsValues.ContainsKey(param))
		{
			paramsValues.Add (param, param.defaultValue);
		}

		OnParamChanged.Invoke (param, value-paramsValues [param]);

		paramsValues [param] = Mathf.Clamp(value, param.minValue, param.maxValue);
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
		switch (effect.effectType)
		{
		    case ParamEffect.ParamEffectType.set:
			    SetParam (effect.parameter, effect.value);
			    break;
		    case ParamEffect.ParamEffectType.add:
			    ChangeParam (effect.parameter, effect.value);
			    break;
		}
	}

	public bool CheckConditions(List<PathCondition> conditions)
	{
		bool result = true;

		foreach(PathCondition pc in conditions)
		{
			result = result && CheckCondition (pc);
		}

		return result;
	}

	public bool CheckCondition(PathCondition condition)
	{
		switch (condition.conditionType){
		case PathCondition.ConditionType.Equeal:
			return condition.value == GetParam (condition.parameter);
		case PathCondition.ConditionType.NotEqeal:
			return condition.value != GetParam (condition.parameter);
		case PathCondition.ConditionType.More:
			return condition.value < GetParam (condition.parameter);
		case PathCondition.ConditionType.Less:
			return condition.value > GetParam (condition.parameter);
		}
		return false;
	}
}
