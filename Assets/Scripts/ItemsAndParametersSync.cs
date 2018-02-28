using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsAndParametersSync : MonoBehaviour {

    [System.Serializable]
	public class ParamSyncStruct
	{
		public PointAndClickItem item;
		public GameParameter parameter;
	}

	public List<ParamSyncStruct> syncList = new List<ParamSyncStruct> ();

    private void Awake()
    {
        Inventory.Instance.OnRemoveItem += RemoveItem;
        Inventory.Instance.OnAddItem += AddItem;
    }

    void Start()
	{
		ParamsManager.Instance.OnParamChanged += ParamChanged;
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
        ParamSyncStruct syncStruct = syncList.Find (p=>p.item == item);

        if (syncStruct != null)
        {
            GameParameter param = syncStruct.parameter;
            ParamsManager.Instance.ApplyEffect (new ParamEffect(param, -1, ParamEffect.ParamEffectType.add));
		}
	}

	private void AddItem(PointAndClickItem item)
	{
        Debug.Log(syncList);

        ParamSyncStruct syncStruct = syncList.Find(p => p.item == item);

		if(syncStruct!=null)
		{
            GameParameter param = syncStruct.parameter;
            ParamsManager.Instance.ApplyEffect (new ParamEffect(param, 1, ParamEffect.ParamEffectType.add));
		}
	}
}
