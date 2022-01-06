using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameProfile", order = 1)]
public class GameProfile : ScriptableObject
{
    public List<TeamProfile> _teamProfiles;
    public List<PlayerProfile> _playerProfiles;
    public int _numberOfRounds = 24;
    public int _days = 5;
    public int _startingActionPoints = 12;
    public int _startingSparks = 0;

}
