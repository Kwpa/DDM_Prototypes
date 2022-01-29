using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeElement : UIElement
{
    public override void Init()
    {
        InitBase();
    }

    public string _upgradeID;
    public string _teamID;
    public TextMeshProUGUI _nameText;
    public TextMeshProUGUI _descriptionText;
    public TextMeshProUGUI _costText;

    public bool _upgradeAcquired = false;
    public GameObject _lockedUI;
    public GameObject _buyButton;
    public GameObject _acquiredUI;


    public void SetUpgradeElement(string upgradeID, string teamID, string name, string description, int cost)
    {
        _upgradeID = upgradeID;
        _teamID = teamID;
        _nameText.text = name;
        _descriptionText.text = description;
        _costText.text = cost + " action points";
    }

    public void SetUpgradeElement(UpgradeDef upgradeDef, string teamID)
    {
        Init();
        SetUpgradeElement(upgradeDef._upgradeID, teamID, upgradeDef._upgradeName, upgradeDef._upgradeDescription, upgradeDef._upgradeCost);
    }

    public void SetUpgradeButton(UpgradeDef upgradeDef)
    {
        List<string> acquiredUpgrades = _gMgr._activePlayer._playerToTeamData[_teamID]._acquiredUpgrades;
        print("***checkcard! " + acquiredUpgrades.Contains(_upgradeID));
        if (acquiredUpgrades.Contains(_upgradeID))
        {
            AcquiredUpgrade();
            _upgradeAcquired = true;
        }
        else
        {
            if(upgradeDef._requiresUpgrade)
            {
                if(acquiredUpgrades.Contains(upgradeDef._requiredUpgradeID))
                {
                    UnlockUpgrade();
                }
                else
                {
                    LockUpgrade();
                }
            }
            else
            {
                UnlockUpgrade();
            }
        }
    }

    public void LockUpgrade()
    {
        _acquiredUI.SetActive(false);
        _buyButton.SetActive(false);
        _lockedUI.SetActive(true);
    }

    public void UnlockUpgrade()
    {
        _acquiredUI.SetActive(false);
        _buyButton.SetActive(true);
        _lockedUI.SetActive(false);
    }

    public void AcquiredUpgrade()
    {
        _acquiredUI.SetActive(true);
        _buyButton.SetActive(false);
        _lockedUI.SetActive(false);
    }

    public void BuyUpgradeButton()
    {
        if(!_upgradeAcquired)
        {
            print("Active player upgrade!!!");
            _gMgr.ActivePlayerUpgrade(_teamID, _upgradeID);
        }
    }
}
