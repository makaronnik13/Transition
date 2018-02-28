using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsAndParametersSync : MonoBehaviour {

	public class ParamSyncStruct
	{
		public PointAndClickItem item;
		public GameParameter parameter;
	}

	public List<ParamSyncStruct> syncList = new List<ParamSyncStruct> ();


	void Start()
	{
		ParamsManager.Instance.OnParamChanged += ParamChanged;
		Inventory.Instance.OnRemoveItem += RemoveItem;
		Inventory.Instance.OnAddItem += AddItem;
	}

	private void ParamChanged(GameParameter parameter, float value)
	{
		PointAndClickItem item = syncList.Find (p=>p.parameter == parameter).item;

		if(item)
		{
			for(int i = 0; i<Mathf.Abs(Mathf.RoundToInt(value));i++)
			{
				if(value>0)
				{
					Inventory.Instance.AddItem (item);
				}	
				else
				{
					Inventory.Instance.RemoveItem(item);
				}
			}
		}
	}

	private void RemoveItem(PointAndClickItem item)
	{
		GameParameter param = syncList.Find (p=>p.item == item).parameter;

		if(param)
		{
			ParamsManager.Instance.ApplyEffect (new ParamEffect(param, -1, ParamEffect.ParamEffectType.add));
		}
	}

	private void AddItem(PointAndClickItem item)
	{
		GameParameter param = syncList.Find (p=>p.item == item).parameter;

		if(param)
		{
			ParamsManager.Instance.ApplyEffect (new ParamEffect(param, 1, ParamEffect.ParamEffectType.add));
		}
	}
}
