using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class TeamProfilePopup : Popup
{
    public string _teamID;
    public TextMeshProUGUI _teamNameText;
    public TextMeshProUGUI _callToActionText;
    public Slider _healthBar;
    public TextMeshProUGUI _bio;
    public Image _iconBackground;
    public Image _icon;
    public List<GameObject> _stats;
    public List<GameObject> _upgrades;
    public List<GameObject> _storyReveals;

    public GameObject _statParent;
    public GameObject _upgradeParent;
    public GameObject _storyRevealParent;

    public GameObject _exclusiveContentLockPanel;

    public GameObject _statPrefab;
    public GameObject _upgradePrefab;
    public GameObject _storyRevealPrefab;

    public override void InitPopup()
    {
        _stats = new List<GameObject>();
        _upgrades = new List<GameObject>();
        _storyReveals = new List<GameObject>();
    }

    public void SetValues(string name, int requirement, string bio, int maxHealth, int currentHealth, bool playerInFanClub, bool outOfCompetition)
    {
        SetTeamNameText(name);
        SetCTAText(requirement);
        SetBioText(bio);
        SetHealthBar(maxHealth, currentHealth);
        if(outOfCompetition)
        {
            KickTeamUI();
        }
    }

    public void CreateCards(List<PersonalityStatDef> stats, List<UpgradeDef> upgrades, List<StoryRevealDef> storyReveals)
    {
        foreach (PersonalityStatDef stat in stats)
        {
            CreatePersonalityStat(stat);
        }
        foreach (UpgradeDef upgrade in upgrades)
        {
            CreateUpgrade(upgrade);
        }
        foreach (StoryRevealDef reveal in storyReveals)
        {
            CreateStoryReveal(reveal);
        }
        SetUpgradeButtonStatuses();
    }

    public void SetCards(List<PersonalityStatDef> stats, List<UpgradeDef> upgrades, List<StoryRevealDef> storyReveals)
    {
        foreach (PersonalityStatDef stat in stats)
        {
            CreatePersonalityStat(stat);
        }
        foreach (UpgradeDef upgrade in upgrades)
        {
            CreateUpgrade(upgrade);
        }
        foreach (StoryRevealDef reveal in storyReveals)
        {
            CreateStoryReveal(reveal);
        }
        SetUpgradeButtonStatuses();
    }

    public void SetTeamNameText(string value)
    {
        _teamNameText.text = value;
    }

    public void SetCTAText(int value)
    {
        _callToActionText.text = "Needs " + value + " more health today";
    }

    public void SetHealthBar(int maxValue, int currentValue)
    {
        _healthBar.minValue = 0;
        _healthBar.maxValue = maxValue;
        _healthBar.value = currentValue;
    }

    public void SetBioText(string value)
    {
        _bio.text = value;
    }

    public void CreatePersonalityStat(PersonalityStatDef stat)
    {
        GameObject statGo = Instantiate(_statPrefab, _statParent.transform);
        statGo.GetComponent<PersonalityStatElement>().SetStatText(stat._statComparisonValueOne, stat._statComparisonValueTwo, stat.statComparison);
        _stats.Add(statGo);
    }

    public void SetPersonalityStatElement(PersonalityStatDef stat)
    {
        GameObject statGo = _stats.FirstOrDefault(p => p.GetComponent<PersonalityStatElement>()._statID == stat._statID);
        if (statGo == null) return;

        statGo.GetComponent<PersonalityStatElement>().SetStatText(stat._statComparisonValueOne, stat._statComparisonValueTwo, stat.statComparison);
    }

    public void CreateUpgrade(UpgradeDef upgrade)
    {
        GameObject upgradeGo = Instantiate(_upgradePrefab, _upgradeParent.transform);
        upgradeGo.GetComponent<UpgradeElement>().SetUpgradeElement(upgrade, _teamID);
        _upgrades.Add(upgradeGo);
    }

    public void SetUpgradeElements(UpgradeDef upgrade)
    {
        GameObject upgradeGo = _stats.FirstOrDefault(p => p.GetComponent<UpgradeElement>()._upgradeID == upgrade._upgradeID);
        if (upgradeGo == null) return;

        upgradeGo.GetComponent<UpgradeElement>().SetUpgradeElement(upgrade, _teamID);
    }

    public void SetUpgradeButtonStatuses()
    {
        List<UpgradeDef> upgradeDefs = _gMgr._teams[_teamID]._teamUpgrades;

        foreach(GameObject go in _upgrades)
        {
            UpgradeElement upgradeElement = go.GetComponent<UpgradeElement>();
            UpgradeDef upgradeDef = upgradeDefs.FirstOrDefault(p => p._upgradeID == upgradeElement._upgradeID);
            upgradeElement.SetUpgradeButton(upgradeDef);
        }
    }

    public void SetUpgradeButtonStatus(string upgradeID)
    {
        GameObject go = _upgrades.FirstOrDefault(p => p.GetComponent<UpgradeElement>()._upgradeID == upgradeID);
        UpgradeElement element = go.GetComponent<UpgradeElement>();
        List<UpgradeDef> upgradeDefs = _gMgr._teams[_teamID]._teamUpgrades;
        UpgradeDef upgradeDef = upgradeDefs.FirstOrDefault(p => p._upgradeID == element._upgradeID);
        element.SetUpgradeButton(upgradeDef);
    }

    public void CreateStoryReveal(StoryRevealDef storyReveal)
    {
        GameObject storyGo = Instantiate(_storyRevealPrefab, _storyRevealParent.transform);
        storyGo.GetComponent<StoryRevealElement>().SetStoryRevealElement(storyReveal, _teamID);
        _storyReveals.Add(storyGo);
    }

    public void SetStoryReveal(StoryRevealDef storyReveal)
    {
        GameObject storyGo = _storyReveals.FirstOrDefault(p => p.GetComponent<StoryRevealElement>()._storyRevealID == storyReveal._storyRevealID);
        if (storyGo == null) return;

        storyGo.GetComponent<StoryRevealElement>().SetStoryRevealElement(storyReveal, _teamID);
    }

    public void UnlockExclusiveContent(bool unlocked)
    {
        _exclusiveContentLockPanel.SetActive(!unlocked);
    }

    public void KickTeamUI()
    {

    }
}
