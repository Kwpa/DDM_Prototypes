using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ballot
{
    public string _ballotID;
    public string _ballotTitle;
    [SerializeField]
    public List<BallotOption> _ballotOptions;
    public string _chosenOptionID;

    public void SetChosenOption(string id)
    {
        _chosenOptionID = id;
    }
}

[System.Serializable]
public class BallotOption
{
    public string _ballotOptionID;
    public string _ballotOptionTitle;
    public string _iconID;
    public string _selectedOutcome;
    public string _unselectedOutcome;
    public int _minVotesNeededToBeSelected = 1;
}
