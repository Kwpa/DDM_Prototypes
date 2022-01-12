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
    public Slider _healthBar;
    public Image _iconBackground;
    public Image _icon;

    public override void Init()
    {
        //...
    }

    public void SetValues(string name, int requirement, int donation, int maxHealth, int currentHealth)
    {
        SetTeamNameText(name);
        SetCTAText(requirement);
        SetDonateEnergyText(donation);
        SetHealthBar(maxHealth, currentHealth);
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

    public void SetHealthBar(int maxValue, int currentValue)
    {
        _healthBar.minValue = 0;
        _healthBar.maxValue = maxValue;
        _healthBar.value = currentValue;
    }

    public void ClickDonationButton()
    {
        _gMgr.SpendActionOnDonation(_gMgr._activePlayer._playerID, _teamID);        
    }

    public void OpenTeamProfilePopup()
    {
        _uiMgr.OpenTeamProfilePopup(_teamID);
    }
}
