using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BallotDef
{
    public string _ballotID;
    public string _ballotTitle;
    public string _ballotDescription;
    [SerializeField]
    public List<BallotOption> _ballotOptions;
    public string _chosenOptionID;
    public int _chosenIndex = -1;

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
    public int _totalVoteCount;
}
