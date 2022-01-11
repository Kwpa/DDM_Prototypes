using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
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
    }

    public Team(TeamProfile profile)
    {
        _teamID = profile._teamID;
        _teamName = profile._teamName;
        _teamBio = profile._teamBio;
        _teamHealth = profile._teamHealth;
        _teamMaxHealth = profile._teamMaxHealth;
        _baseDonationAmount = profile._teamDonationAmount;
        _teamPersonalityStats = profile._teamPersonalityStats;
        _teamUpgrades = profile._teamUpgrades;
        _teamStoryReveals = profile._storyReveals;
    }

    public void UpdateTeam()
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
    }

    public void LoseHealth(int value)
    {
        _teamHealth -= value;
        _teamHealth = Mathf.Clamp(_teamHealth, 0, _teamMaxHealth);
    }

    public void SetStat(PersonalityStatDef stat)
    {
        // todo
    }

    public int GetDonationAmount(int playerFactor)
    {
        return (_baseDonationAmount + _donationAddModifier) * playerFactor ;
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

}

[System.Serializable]
public class PersonalityStatDef
{
    public string _statID = "statID";
    public string _statName = "stat name";
    public string _statUpperName = "stat name";
    public string _statUpperLower = "stat name";
    public int _rangeMin = 0;
    public int _rangeMax = 10;
    public int _statValue = 5;
}

[System.Serializable]
public class StoryRevealDef
{
    public string _storySectionTitle = "story title";
    [TextArea] public string _storySectionContent = "story content";
    public Achievement _achievementTrigger;
    public Upgrade _upgradeTrigger;
}
