using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TeamAvatar : UIElement
{
    public string _teamID;
    public TextMeshProUGUI _teamNameText;
    public TextMeshProUGUI _callToActionText;
    public TextMeshProUGUI _donateEnergyText;
    public Image _iconBackground;
    public Image _icon;

    public override void Init()
    {
        base.Init();
    }

    public void SetValues(string name, int requirement, int donation)
    {
        SetTeamNameText(name);
        SetCTAText(requirement);
        SetDonateEnergyText(donation);
    }

    public void SetTeamNameText(string value)
    {
        _teamNameText.text = value;
    }

    public void SetCTAText(int value)
    {
        _callToActionText.text = "Needs " + value + " more health today";
    }

    public void SetDonateEnergyText(int value)
    {
        _donateEnergyText.text = "+" + value + " energy";
    }
}
