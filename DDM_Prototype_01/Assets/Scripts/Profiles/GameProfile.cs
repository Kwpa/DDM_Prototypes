using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameProfile", order = 1)]
public class GameProfile : ScriptableObject
{
    public List<TeamProfile> _teamProfiles;
    public List<PlayerProfile> _playerProfiles;
    public string _activePlayerID;
    public int _daysPerGame = 5;
    public int _roundsPerDay = 24;
    public int _roundTime = 60;
    public int _dancingTime = 50;
    public int _timeFactor = 1;
    public int _startingActionPoints = 12;
    public int _startingSparkPoints = 0;

}
