using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string _playerID = "player id";
    public string _username = "player name";
    public bool _playerIsBot = false;
    public int _baseActionPoints = 0;
    public int _actionPoints = 0;
    public int _sparkPoints = 0;
    public int _donationFactor = 1;
    public List<PlayerAttribute> _playerAttributes;

    public Dictionary<string, PlayerToTeamData> _playerToTeamData;

    public Player(string id, string name, int allocateBaseActionPoints, Dictionary<string,Team> teamsData)
    {
        _playerID = id;
        _username = name;
        _baseActionPoints = allocateBaseActionPoints;
        SetupPlayerToTeamData(teamsData);
    }

    public Player(PlayerProfile profile, Dictionary<string,Team> teamsData)
    {
        _playerID = profile._playerID;
        _username = profile._username;
        SetupPlayerToTeamData(teamsData);
    }

    public void UpdatePlayer(Dictionary<string, Team> teams)
    {
        Dictionary<string, PlayerToTeamData> dict = new Dictionary<string, PlayerToTeamData>();
        foreach (KeyValuePair<string, PlayerToTeamData> ptdVal in _playerToTeamData)
        {
            PlayerToTeamData data = ptdVal.Value;
            data._teamDonationAmount = teams[data._teamID].GetDonationAmount(data._donationFactor);
            dict.Add(ptdVal.Key, data);
        }
        _playerToTeamData.Clear();
        foreach(KeyValuePair<string, PlayerToTeamData> kvp in dict)
        {
            _playerToTeamData.Add(kvp.Key, kvp.Value);
        }
    }

    public void SetupPlayerToTeamData(Dictionary<string, Team> teams)
    {
        _playerToTeamData = new Dictionary<string, PlayerToTeamData>();
        foreach (KeyValuePair<string, Team> teamVal in teams)
        {
            Team team = teamVal.Value;
            _playerToTeamData.Add(team._teamID, new PlayerToTeamData(team._teamID));
        }
        Debug.Log("Player " + _username + " ** " + teams.Count);
        UpdatePlayer(teams);
    }

    public void GainTeamUpgrade(string _teamID, string _upgradeID)
    {
        _playerToTeamData[_teamID]._acquiredUpgrades.Add(_upgradeID);
    }

    public bool CheckTeamUpgrade(string _teamID, string _upgradeID)
    {
        List<string> upgrades = _playerToTeamData[_teamID]._acquiredUpgrades;
        return upgrades.Contains(_upgradeID); 
    }

    public void SetBaseActionPoints(int value)
    {
        _baseActionPoints = value;
    }

    public void ResetActionPoints()
    {
        _actionPoints = _baseActionPoints;
    }

    public void GainActionPoints(int value)
    {
        _actionPoints += value;
        _actionPoints = Mathf.Clamp(_actionPoints, 0, 1000);
    }

    public void LoseActionPoints(int value)
    {
        _actionPoints -= value;
        _actionPoints = Mathf.Clamp(_actionPoints, 0, 1000);
    }

    public void SpendActionPoints(int value)
    {
        LoseActionPoints(value);
    }

    public void GainSparkPoints(int value)
    {
        _sparkPoints += value;
        _sparkPoints = Mathf.Clamp(_sparkPoints, 0, 1000);
    }

    public void LoseSparkPoints(int value)
    {
        _sparkPoints -= value;
        _sparkPoints = Mathf.Clamp(_sparkPoints, 0, 1000);
    }

    public void SpendSparkPoints(int value)
    {
        LoseSparkPoints(value);
    }

    public void JoinFanClub(string teamID)
    {
        _playerToTeamData[teamID]._playerIsInFanClub = true;
    }

    public void UpgradeTeamRelationship(string teamID, string upgradeID)
    {
        _playerToTeamData[teamID]._acquiredUpgrades.Add(upgradeID);
    }
}

public class PlayerAttribute
{
    public string _attributeName;
    public string _attributedDetails;
}

public class PlayerToTeamData
{
    public string _teamID;
    public int _donationFactor = 1;
    public int _teamDonationAmount = 1;
    public bool _playerIsInFanClub = false;
    public List<string> _acquiredUpgrades;
    public List<string> _acquiredStoryReveals;
    public int _botLikeDislike = 0;

    public PlayerToTeamData(string id)
    {
        _acquiredUpgrades = new List<string>();
        _acquiredStoryReveals = new List<string>();
        _teamID = id;
    }
}
