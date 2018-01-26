
[System.Serializable]
public class ParamEffect
{
    public GameParameter parameter;
    public float value;

	public ParamEffect(GameParameter p, float v)
	{
		parameter = p;
		value = v;
	}
}
