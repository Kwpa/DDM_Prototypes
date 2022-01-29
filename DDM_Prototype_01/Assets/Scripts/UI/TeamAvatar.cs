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
    public TextMeshProUGUI _upgradeLevelText;

    public Slider _healthBar;
    public Image _iconBackground;
    public Image _icon;
    public GameObject _inGamePanel;
    public GameObject _kickedOutPanel;
    public GameObject _fanHand;
    public GameObject _upgradeStar;
    int _upgradeLevel = 0;

    public override void Init()
    {
        //...
    }

    public void SetValues(string name, int requirement, int donation, int maxHealth, int currentHealth, bool outOfCompetition)
    {
        SetTeamNameText(name);
        SetCTAText(requirement);
        SetDonateEnergyText(donation);
        SetHealthBar(maxHealth, currentHealth);
        if(outOfCompetition)
        {
            print("out of comp!");
            KickTeamUI();
        }
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
        _gMgr.SpendActionOnHealthDonation(_gMgr._activePlayer._playerID, _teamID);        
    }

    public void OpenTeamProfilePopup()
    {
        _uiMgr.OpenTeamProfilePopup(_teamID);
    }

    public void KickTeamUI()
    {
        _inGamePanel.SetActive(false);
        _kickedOutPanel.SetActive(true);
    }

    public void JoinFanClub()
    {
        _fanHand.SetActive(true);
        iTween.PunchScale(_fanHand.gameObject, new Vector3(1.2f, 1.2f, 1.2f), 1f);
    }

    public void UpgradeLevelIncrease()
    {
        _upgradeStar.SetActive(true);
        _upgradeLevel++;
        _upgradeLevelText.text = "+" + (_upgradeLevel).ToString();
        iTween.PunchScale(_upgradeLevelText.gameObject, new Vector3(1.2f, 1.2f, 1.2f), 1f);
    }
}
