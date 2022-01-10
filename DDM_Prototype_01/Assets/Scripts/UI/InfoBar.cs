using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoBar : UIElement
{
    public TextMeshProUGUI _daysText;
    public TextMeshProUGUI _roundsText;
    public TextMeshProUGUI _timeLeftText;
    public TextMeshProUGUI _roundSectionText;
    public TextMeshProUGUI _gainText;
    public TextMeshProUGUI _actionsText;
    public TextMeshProUGUI _sparksText;

    public override void Init()
    {
        base.Init();
    }

    public void SetInfoBarVariables(int days, int rounds, int actions, string currentStateName, int sparks, int minutesLeft, int secondsLeft)
    {
        InfoBar infoBar = this;
        infoBar.SetDaysText(days);
        infoBar.SetRoundsText(rounds);
        infoBar.SetRoundSectionText(currentStateName);
        infoBar.SetActionsText(actions);
        infoBar.SetSparksText(sparks);
        infoBar.SetTimeLeftText(minutesLeft, secondsLeft);
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
        string minsCount = minutes == 1 ? " min " : " mins ";
        string secsCount = seconds == 1 ? " second" : " seconds";
        _timeLeftText.text = minutes + minsCount + seconds + secsCount;
    }

    public void SetRoundSectionText(string roundSectionName)
    {
        if(roundSectionName=="Dancing" || roundSectionName == "Resting")
        {
            _roundSectionText.text = roundSectionName + " ends in:"; 
        }
    }

    public void SetGainText(int value)
    {
        _gainText.text = "Gain " + value + " extra action";
    }
}
