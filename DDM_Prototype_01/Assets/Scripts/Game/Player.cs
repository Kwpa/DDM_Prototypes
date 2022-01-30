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
    public List<BallotContribution> _globalBallotContributions;

    public Dictionary<string, PlayerToTeamData> _playerToTeamData;

    public Player(string id, string name, int allocateBaseActionPoints, Dictionary<string,Team> teamsData)
    {
        _playerID = id;
        _username = name;
        _baseActionPoints = allocateBaseActionPoints;
        _globalBallotContributions = new List<BallotContribution>();
        SetupPlayerToTeamData(teamsData);
    }

    public Player(PlayerProfile profile, Dictionary<string,Team> teamsData)
    {
        _playerID = profile._playerID;
        _username = profile._username;
        _globalBallotContributions = new List<BallotContribution>();
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
        UpdatePlayer(teams);
    }

    public bool CheckTeamUpgrade(string teamID, string upgradeID)
    {
        List<string> upgrades = _playerToTeamData[teamID]._acquiredUpgrades;
        return upgrades.Contains(upgradeID); 
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

    public void GainTeamUpgrade(string teamID, UpgradeDef upgrade)
    {
        _playerToTeamData[teamID]._acquiredUpgrades.Add(upgrade._upgradeID);
        switch (upgrade._upgradeType)
        {
            case UpgradeType.MaxHealth:
                //_playerToTeamData[teamID]._sparkRewardBonus += upgrade._upgradeBonus;
                break;
            case UpgradeType.SparkBonus:
                _playerToTeamData[teamID]._sparkRewardBonus += upgrade._upgradeBonus;
                break;
        }
    }

    public int GainSparksFromTeam(string teamID, int baseReward)
    {
        int amount = baseReward + _playerToTeamData[teamID]._sparkRewardBonus;
        GainSparkPoints(amount);
        return amount;
    }


    public void AddGlobalBallotContribution(string ballotID, string ballotOptionID, int voteContribution)
    {
        BallotContribution contribution = new BallotContribution(ballotID, ballotOptionID);
        contribution._playerVoteTotal += voteContribution;
        contribution._playerVoteTotal = Mathf.Clamp(contribution._playerVoteTotal, 0, 1000000000);
        _globalBallotContributions.Add(contribution);
    }

    public void UpdateGlobalBallotContribution(string ballotID, string ballotOptionID, int voteContribution)
    {
        BallotContribution contribution = _globalBallotContributions.Find(p => p._ballotID == ballotID && p._ballotOptionID == ballotOptionID);
        if (contribution != null)
        {
            contribution._playerVoteTotal += voteContribution;
            contribution._playerVoteTotal = Mathf.Clamp(contribution._playerVoteTotal, 0, 1000000000);
        }
        else
        {
            AddGlobalBallotContribution(ballotID, ballotOptionID, voteContribution);
        }
    }

    public int GetOptionCurrentGlobalContribution(string ballotID, string ballotOptionID)
    {
        BallotContribution contribution = _globalBallotContributions.Find(p => p._ballotID == ballotID && p._ballotOptionID == ballotOptionID);
        return contribution._playerVoteTotal;
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
    public int _sparkRewardBonus = 0;
    public List<string> _acquiredUpgrades;
    public List<string> _acquiredStoryReveals;
    public int _botLikeDislike = 0;
    public List<BallotContribution> _ballotContributions; 

    public PlayerToTeamData(string id)
    {
        _acquiredUpgrades = new List<string>();
        _acquiredStoryReveals = new List<string>();
        _ballotContributions = new List<BallotContribution>();
        _teamID = id;
    }

    public void AddBallotContribution(string ballotID, string ballotOptionID, int voteContribution)
    {
        BallotContribution contribution = new BallotContribution(ballotID, ballotOptionID);
        contribution._playerVoteTotal += voteContribution;
        contribution._playerVoteTotal = Mathf.Clamp(contribution._playerVoteTotal, 0, 1000000000);
        Debug.Log("*ballot " + ballotID + " " + ballotOptionID + contribution._playerVoteTotal);
        _ballotContributions.Add(contribution);
    }

    public void UpdateBallotContribution(string ballotID, string ballotOptionID, int voteContribution)
    {
        BallotContribution contribution = _ballotContributions.Find(p => p._ballotID == ballotID && p._ballotOptionID == ballotOptionID);
        if(contribution != null)
        {
            contribution._playerVoteTotal += voteContribution;
            contribution._playerVoteTotal = Mathf.Clamp(contribution._playerVoteTotal, 0, 1000000000);
            Debug.Log("*ballot " + ballotID + " " + ballotOptionID + contribution._playerVoteTotal);
        }
        else
        {
            AddBallotContribution(ballotID, ballotOptionID, voteContribution);
        }
    }

    public int GetOptionCurrentContribution(string ballotID, string ballotOptionID)
    {
        BallotContribution contribution = _ballotContributions.Find(p => p._ballotID == ballotID && p._ballotOptionID == ballotOptionID);
        return contribution._playerVoteTotal;
    }
}

public class BallotContribution
{
    public string _ballotID;
    public string _ballotOptionID;
    public int _playerVoteTotal = 0;

    public BallotContribution(string ballotID, string ballotOptionID)
    {
        _ballotID = ballotID;
        _ballotOptionID = ballotOptionID;
    }
}