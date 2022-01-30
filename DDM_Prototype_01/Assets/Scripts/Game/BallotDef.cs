using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class BallotDef
{
    public string _ballotID;
    public string _ballotTitle;
    public string _ballotDescription;
    [SerializeField]
    public List<BallotOption> _ballotOptions;
    public int _dayActive;
    public string _chosenOptionID;
    public int _chosenIndex = -1;

    public BallotDef(string ballotID, string ballotTitle, string ballotDescription, List<BallotOption> ballotOptions, int dayActive)
    {
        _ballotID = ballotID;
        _ballotTitle = ballotTitle;
        _ballotDescription = ballotDescription;
        _ballotOptions = new List<BallotOption>();
        foreach(BallotOption option in ballotOptions)
        {
            _ballotOptions.Add(new BallotOption(option._ballotOptionID, option._ballotID, option._ballotOptionTitle, option._ballotOptionDescription, option._iconID, option._minVotesNeededToBeSelected, option._selectedOutcome, option._unselectedOutcome));
            Debug.Log(option._totalVoteCount);
        }
        _dayActive = dayActive;
    }

    public int EvaluateBallot()
    {
        int winnerValue = _ballotOptions.Max(p => p._totalVoteCount);
        BallotOption winner = _ballotOptions.Find(p => p._totalVoteCount == winnerValue);
        SetChosenOption(winner._ballotOptionID);
        return _chosenIndex;
    }

    public List<int> GetFinalVoteAmounts()
    {
        List<int> scores = new List<int>();
        foreach(BallotOption ballotOption in _ballotOptions)
        {
            scores.Add(ballotOption._totalVoteCount);
            Debug.Log(_ballotID + " " + ballotOption._totalVoteCount);
        }
        return scores;
    }

    public void SetChosenOption(string id)
    {
        _chosenOptionID = id;
        for(int i = 0; i<_ballotOptions.Count; i++)
        {
            if(_ballotOptions[i]._ballotOptionID == _chosenOptionID)
            {
                _chosenIndex = i;
            }
        }
    }

    public BallotOption GetChosenBallotOption()
    {
        if (_chosenIndex != -1)
        {
            return _ballotOptions[_chosenIndex];
        }
        else return null;
    }

    public void IncreaseVote(string ballotOptionID)
    {
        BallotOption option = _ballotOptions.Find(p => p._ballotOptionID == ballotOptionID);
        option.IncreaseVote(1);
    }

    public void DecreaseVote(string ballotOptionID)
    {
        BallotOption option = _ballotOptions.Find(p => p._ballotOptionID == ballotOptionID);
        option.DecreaseVote(1);
    }
}

[System.Serializable]
public class BallotOption
{
    public string _ballotOptionID;
    public string _ballotID;
    public string _ballotOptionTitle;
    public string _ballotOptionDescription;
    public string _iconID;
    public string _selectedOutcome;
    public string _unselectedOutcome;
    public int _minVotesNeededToBeSelected = 1;
    public int _totalVoteCount = 0;

    public BallotOption(string ballotOptionID, string ballotID, string ballotOptionTitle, string ballotOptionDescription, string iconID, int minVotesNeededToBeSelected, string selectedOutcome, string unselectedOutcome)
    {
        _ballotOptionID = ballotOptionID;
        _ballotID = ballotID;
        _ballotOptionTitle = ballotOptionTitle;
        _ballotOptionDescription = ballotOptionDescription;
        _iconID = iconID;
        _selectedOutcome = selectedOutcome;
        _unselectedOutcome = unselectedOutcome;
        _minVotesNeededToBeSelected = minVotesNeededToBeSelected;
        _totalVoteCount = 0;
    }

    public void IncreaseVote(int amount)
    {
        _totalVoteCount += amount;
        _totalVoteCount = Mathf.Clamp(_totalVoteCount, 0, 10000000);
    }

    public void DecreaseVote(int amount)
    {
        _totalVoteCount -= amount;
        _totalVoteCount = Mathf.Clamp(_totalVoteCount, 0, 10000000);
    }
}
