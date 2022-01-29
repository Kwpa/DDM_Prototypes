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

    public BallotDef(BallotDef ballotDef)
    {
        _ballotID = ballotDef._ballotID;
        _ballotTitle = ballotDef._ballotTitle;
        _ballotDescription = ballotDef._ballotDescription;
        _ballotOptions = new List<BallotOption>();
        foreach(BallotOption option in ballotDef._ballotOptions)
        {
            _ballotOptions.Add(new BallotOption(option));
        }
        _dayActive = ballotDef._dayActive;
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

    public BallotOption(BallotOption option)
    {
        _ballotOptionID = option._ballotOptionID;
        _ballotID = option._ballotID;
        _ballotOptionTitle = option._ballotOptionTitle;
        _ballotOptionDescription = option._ballotOptionDescription;
        _iconID = option._iconID;
        _selectedOutcome = option._selectedOutcome;
        _unselectedOutcome = option._unselectedOutcome;
        _minVotesNeededToBeSelected = option._minVotesNeededToBeSelected;
        _totalVoteCount = 0;
    }

    public void IncreaseVote(int amount)
    {
        _totalVoteCount += amount;
        Debug.Log("TOTAL VOTE VALUE " + _totalVoteCount + " for " + _ballotOptionTitle);
        _totalVoteCount = Mathf.Clamp(_totalVoteCount, 0, 10000000);
    }

    public void DecreaseVote(int amount)
    {
        _totalVoteCount -= amount;
        Debug.Log("TOTAL VOTE VALUE " + _totalVoteCount + " for " + _ballotOptionTitle);
        _totalVoteCount = Mathf.Clamp(_totalVoteCount, 0, 10000000);
    }
}
