﻿using System;

[System.Serializable]
public class Choice
{
    public Symbol[] symbolContent;
    public String textContent;

    public String textFeedback;

	public ParamEffect[] paramEffects = new ParamEffect[0];
    public Transmission nextTransmission;
	public Transmission[] addTransmissions = new Transmission[0];
    //public Transmission[] discardTransmissions;
}
