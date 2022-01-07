using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoBar : UIElement
{
    public TextMeshProUGUI _daysText;
    public TextMeshProUGUI _roundsText;
    public TextMeshProUGUI _timeLeftText;
    public TextMeshProUGUI _gainText;
    public TextMeshProUGUI _actionsText;
    public TextMeshProUGUI _sparksText;

    public override void Init()
    {
        base.Init();
    }

    public void SetDaysText(int value)
    {
        _daysText.text = value.ToString();
    }

    public void SetRoundsText(int value)
    {
        _roundsText.text = value.ToString();
    }

    public void SetActionsText(int value)
    {
        _actionsText.text = value.ToString();
    }

    public void SetSparksText(int value)
    {
        _sparksText.text = value.ToString();
    }

    public void SetTimeLeftText(int minutes, int seconds)
    {
        _timeLeftText.text = minutes + " mins " + seconds + " seconds"; 
    }

    public void SetGainText(int value)
    {
        _gainText.text = "Gain " + value + " extra action");
    }
}
