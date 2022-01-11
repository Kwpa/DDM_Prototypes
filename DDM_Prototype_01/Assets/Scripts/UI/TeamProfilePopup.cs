using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TeamProfilePopup : Popup
{
    public string _teamID;
    public TextMeshProUGUI _teamNameText;
    public TextMeshProUGUI _callToActionText;
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

    public void SetValues(string name, int requirement, string bio, List<PersonalityStatDef> stats, List<UpgradeDef> upgrades, List<StoryRevealDef> storyReveals, bool playerInFanClub)
    {
        SetTeamNameText(name);
        SetCTAText(requirement);
        SetBioText(bio);
        foreach(PersonalityStatDef stat in stats)
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
    }

    public void SetTeamNameText(string value)
    {
        _teamNameText.text = value;
    }

    public void SetCTAText(int value)
    {
        _callToActionText.text = "Needs " + value + " more health today";
    }

    public void SetBioText(string value)
    {
        _callToActionText.text = value;
    }

    public void CreatePersonalityStat(PersonalityStatDef stats)
    {
        _stats.Add(Instantiate(_statPrefab,_statParent.transform));
    }

    public void CreateUpgrade(UpgradeDef upgrade)
    {
        _upgrades.Add(Instantiate(_upgradePrefab, _upgradeParent.transform));
    }

    public void CreateStoryReveal(StoryRevealDef storyReveal)
    {
        _storyReveals.Add(Instantiate(_storyRevealPrefab, _storyRevealParent.transform));
    }

    public void UnlockExclusiveContent(bool unlocked)
    {
        _exclusiveContentLockPanel.SetActive(!unlocked);
    }
}
