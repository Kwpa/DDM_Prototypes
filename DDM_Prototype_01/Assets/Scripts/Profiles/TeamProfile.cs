using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TeamProfile", order = 2)]
public class TeamProfile : ScriptableObject
{
    public string _teamID = "team id";
    public string _teamName = "team name";
    [TextArea] public string _teamBio = "bio";
    public int _teamHealth = 0;
    public int _team = 0;
    public List<Stat> _teamStats;
    public List<Upgrade> _teamUpgrades;
    public List<StoryReveal> _storyReveals;
}
