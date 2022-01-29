using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallotOptionElement : UIElement
{
    public string _ballotID;
    public string _ballotOptionID;
    public string _teamID;
    public TextMeshProUGUI _titleText;
    public TextMeshProUGUI _descriptionText;
    public TextMeshProUGUI _voteCount;

    public bool _optionWon = false;
    public bool _optionLost = false;
    public GameObject _unlockedUI;
    public GameObject _optionWonUI;
    public GameObject _optionLostUI;

    public int _currentValue = 0;

    public override void Init()
    {
        InitBase();
    }

    public void SetValues(string ballotOptionID, string ballotID, string teamID, string name, string description)
    {
        _ballotID = ballotID;
        _ballotOptionID = ballotOptionID;
        _teamID = teamID;
        _titleText.text = name;
        _descriptionText.text = description;
    }

    public void SetValues(BallotOption ballotDef, string teamID)
    {
        SetValues(ballotDef._ballotOptionID, ballotDef._ballotID, teamID, ballotDef._ballotOptionTitle, ballotDef._ballotOptionDescription);
    }

    public void UpdateNumber(int i)
    {
        _currentValue = i;
        _voteCount.text = i.ToString();
    }

    public void OptionWon()
    {
        _unlockedUI.SetActive(false);
        _optionLostUI.SetActive(false);
        _optionWonUI.SetActive(true);
    }

    public void OptionLost()
    {
        _unlockedUI.SetActive(false);
        _optionWonUI.SetActive(false);
        _optionLostUI.SetActive(true);
    }

    public void VoteUpButton()
    {
        _gMgr.SpendSparksOnBriefcaseVote(_gMgr._activePlayer._playerID, _teamID, _ballotID, _ballotOptionID);
    }

    public void VoteDownButton()
    {
        _gMgr.RefundSparksOnBriefcaseVote(_gMgr._activePlayer._playerID, _teamID, _ballotID, _ballotOptionID);
    }

}
