using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Team : IUpgradeMaxHealth
{
    public string _teamID = "team id";
    public string _teamName = "team name";
    [TextArea] public string _teamBio = "bio";
    public int _teamHealth = 0;
    public int _teamMaxHealth = 0;
    public int _baseDonationAmount = 0;
    public int _donationAddModifier = 0;

    public List<PersonalityStatDef> _teamPersonalityStats;
    public List<UpgradeDef> _teamUpgrades;
    public List<StoryRevealDef> _teamStoryReveals;

    public GameObject _assignedAvatar;
    public GameObject _assignedProfilePopup;
    public int _donationNeeded = 0;

    public bool _outOfCompetition = false;

    public Team(string id, string name, string bio, int health, int maxHealth, int donationAmount, List<PersonalityStatDef> stats, List<UpgradeDef> upgrades, List<StoryRevealDef> reveals)
    {
        _teamID = id;
        _teamName = name;
        _teamBio = bio;
        _teamHealth = health;
        _teamMaxHealth = maxHealth;
        _baseDonationAmount = donationAmount;
        _teamPersonalityStats = stats;
        _teamUpgrades = upgrades;
        _teamStoryReveals = reveals;
        GetDonationNeeded();
    }

    public Team(TeamProfile profile)
    {
        _teamID = profile._teamID;
        _teamName = profile._teamName;
        _teamBio = profile._teamBio;
        _baseDonationAmount = profile._teamDonationAmount;
        _teamPersonalityStats = profile._teamPersonalityStats;
        _teamUpgrades = profile._teamUpgrades;
        _teamStoryReveals = profile._storyReveals;
        GetDonationNeeded();
    }

    public void UpdateTeam()
    {
        //
    }

    public UpgradeDef GetUpgrade(string id)
    {
        return _teamUpgrades.FirstOrDefault(p => p._upgradeID == id);
    }

    public PersonalityStatDef GetStat(string id)
    {
        return _teamPersonalityStats.FirstOrDefault(p => p._statID == id);
    }

    public StoryRevealDef GetStoryReveal(string id)
    {
        return _teamStoryReveals.FirstOrDefault(p => p._storyRevealID == id);
    }

    public void GetDonationNeeded()
    {
        _donationNeeded = _teamMaxHealth - _teamHealth;
    }

    public void SetHealth(int value)
    {
        _teamHealth = value;
    }

    public void SetHealthFull()
    {
        _teamHealth = _teamMaxHealth;
    }

    public void SetMaxHealth(int value)
    {
        _teamMaxHealth = value;
    }

    public void GainHealth(int value)
    {
        _teamHealth += value;
        _teamHealth = Mathf.Clamp(_teamHealth, 0, _teamMaxHealth);
        GetDonationNeeded();
    }

    public void LoseHealth(int value)
    {
        _teamHealth -= value;
        _teamHealth = Mathf.Clamp(_teamHealth, 0, _teamMaxHealth);
        GetDonationNeeded();
    }

    public void SetStat(PersonalityStatDef stat)
    {
        // todo
    }

    public int GetDonationAmount(int playerFactor)
    {
        return (_baseDonationAmount + _donationAddModifier) * playerFactor ;
    }

    public void UpgradeMaxHealth()
    {
        
    }

    public void EvaluateHealth()
    {
        _outOfCompetition = _teamHealth < _teamMaxHealth;
    }
}


public class TeamMember
{
    public string _name = "team member name";
}

[System.Serializable]
public class UpgradeDef
{
    public string _upgradeID = "upgrade id";
    public string _upgradeName = "upgrade name";
    public string _upgradeDescription = "upgrade description";
    public int _upgradeCost = 0;
    public bool _locked = false;
    public bool _acquired = false;

    public bool _requiresUpgrade = false;
    public string _requiredUpgradeID;

    public void LockUpgrade()
    {
        _locked = true;
    }
    public void UnlockUpgrade()
    {
        _locked = false;
    }

    public void GetUpgrade()
    {
        _acquired = true;
    }
}

[System.Serializable]
public class PersonalityStatDef
{
    public string _statID = "statID";
    public string _statComparisonValueOne = "comp 1";
    public PersonalityStatElement.StatComparison statComparison;
    public string _statComparisonValueTwo = "comp 2";
}

[System.Serializable]
public class StoryRevealDef
{
    public string _storyRevealID = "storyrevealID";
    public string _storySectionTitle = "story title";
    public string _storySectionDescription = "description";
    [TextArea] public string _storySectionContent = "story content";
    bool _locked;
    bool _acquired;

    public bool _requiresUpgrade = false;
    public string _requiredUpgradeID;

    public bool _requiresAchievement = false;
    public string _requiredAchievementID;

    public bool requiresStoryRevealID = false;
    public string _requiredStoryRevealID;
}
