
[System.Serializable]
public class ParamEffect
{
    public GameParameter parameter;
	public enum ParamEffectType
	{
		set,
		add
	}

	public ParamEffectType effectType = ParamEffectType.set;

    public float value;

	public ParamEffect(GameParameter p, float v, ParamEffectType type = ParamEffectType.set)
	{
		parameter = p;
		value = v;
	}
}
