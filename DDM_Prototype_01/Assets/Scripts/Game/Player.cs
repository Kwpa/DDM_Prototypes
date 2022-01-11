using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string _playerID = "player id";
    public string _username = "player name";

    public int _actionPoints = 0;
    public int _sparkPoints = 0;
    public int _donationFactor = 1;
    public List<PlayerAttribute> _playerAttributes;

    public Dictionary<string, PlayerToTeamData> _playerToTeamData;

    public Player(string id, string name, Dictionary<string,Team> teamData)
    {
        _playerID = id;
        _username = name;
        SetupPlayerToTeamData(teamData);
    }

    public Player(PlayerProfile profile, Dictionary<string,Team> teamData)
    {
        _playerID = profile._playerID;
        _username = profile._username;
        SetupPlayerToTeamData(teamData);

    }

    public void UpdatePlayer(Dictionary<string, Team> teams)
    {
        foreach (KeyValuePair<string, PlayerToTeamData> ptdVal in _playerToTeamData)
        {
            PlayerToTeamData data = ptdVal.Value;
            data._teamDonationAmount = teams[data._teamID].GetDonationAmount(data._donationFactor);
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
        UpdatePlayer(teams);
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

    public PlayerToTeamData(string id)
    {
        _teamID = id;
    }
}
