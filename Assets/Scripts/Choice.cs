using System;

[System.Serializable]
public class Choice
{
    public Symbol[] symbolContent;
    public String textContent;

    public String textFeedback;

    public ParamEffect[] paramEffect;
    public Transmission nextTransmission;
    public Transmission[] addTransmissions;
    public Transmission[] discardTransmissions;
}
