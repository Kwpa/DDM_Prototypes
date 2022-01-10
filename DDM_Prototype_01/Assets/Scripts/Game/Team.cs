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

    public List<PersonalityStat> _teamPersonalityStats;
    public List<Upgrade> _teamUpgrades;
    public List<StoryReveal> _storyReveals;

    public GameObject _assignedAvatar;
    public int _donationNeeded = 0;

    public Team(string id, string name, string bio, int health, int maxHealth, int donationAmount, List<PersonalityStat> stats, List<Upgrade> upgrades, List<StoryReveal> reveals)
    {
        _teamID = id;
        _teamName = name;
        _teamBio = bio;
        _teamHealth = health;
        _teamMaxHealth = maxHealth;
        _baseDonationAmount = donationAmount;
        _teamPersonalityStats = stats;
        _teamUpgrades = upgrades;
        _storyReveals = reveals;
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
        _storyReveals = profile._storyReveals;
    }

    public void UpdateTeam()
    {
        //
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

    public void SetStat(PersonalityStat stat)
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

public class PersonalityStat
{
    public string _statID = "statID";
    public string _statName = "stat name";
    public int _rangeMin = 0;
    public int _rangeMax = 10;
    public int _statValue = 5;
}

public class StoryReveal
{
    public string _storySectionTitle = "story title";
    [TextArea] public string _storySectionContent = "story content";
    public Achievement _achievementTrigger;
    public Upgrade _upgradeTrigger;
}
