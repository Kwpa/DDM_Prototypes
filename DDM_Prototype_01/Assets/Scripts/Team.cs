using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public string _teamName = "team name";
    [TextArea] public string _teamBio = "bio";
    public int _teamHealth = 0;
    public int _team = 0;
    public List<Upgrade> _teamUpgrades;
    public List<Stat> _teamStats;
    public List<StoryReveal> _storyReveal;
}

public class TeamMember
{
    public string _name = "team member name";
}

public class Stat
{
    public string _statName = "stat name";
    public int _rangeMin = 0;
    public int _rangeMax = 10;
    public int _statValue = 5;
}

public class StoryReveal
{
    public string _storySectionTitle = "story title";
    [TextArea] public string _storySectionContent = "story content";
    public Achievement _achievement;
    public int _rangeMax = 10;
}
