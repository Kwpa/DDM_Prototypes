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
    public int _teamDonationNeeded = 0;

    public List<Stat> _teamStats;
    public List<Upgrade> _teamUpgrades;
    public List<StoryReveal> _storyReveals;

    public GameObject _assignedAvatar;

    public Team(string id, string name, string bio, int health, List<Stat> stats, List<Upgrade> upgrades, List<StoryReveal> reveals)
    {
        _teamID = id;
        _teamName = name;
        _teamBio = bio;
        _teamHealth = health;
        _teamStats = stats;
        _teamUpgrades = upgrades;
        _storyReveals = reveals;
    }

    public Team(TeamProfile profile)
    {
        _teamID = profile._teamID;
        _teamName = profile._teamName;
        _teamBio = profile._teamBio;
        _teamHealth = profile._teamHealth;
        _teamStats = profile._teamStats;
        _teamUpgrades = profile._teamUpgrades;
        _storyReveals = profile._storyReveals;
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

    public void SetStat(Stat stat)
    {
        
    }
}


public class TeamMember
{
    public string _name = "team member name";
}

public class Stat
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
