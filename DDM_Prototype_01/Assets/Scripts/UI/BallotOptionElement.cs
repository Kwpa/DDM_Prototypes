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
    public bool _global = false;

    public bool _optionWon = false;
    public bool _optionLost = false;
    public GameObject _unlockedUI;
    public GameObject _optionWonUI;
    public GameObject _optionLostUI;
    public List<GameObject> _votingButtons;

    public int _currentValue = 0;

    public override void Init()
    {
        InitBase();
    }

    public void SetValues(string ballotOptionID, string ballotID, string teamID, string name, string description, bool global)
    {
        _ballotID = ballotID;
        _ballotOptionID = ballotOptionID;
        _teamID = teamID;
        _titleText.text = name;
        _descriptionText.text = description;
        _global = global;
    }

    public void SetValues(BallotOption ballotDef, string teamID, bool global)
    {
        SetValues(ballotDef._ballotOptionID, ballotDef._ballotID, teamID, ballotDef._ballotOptionTitle, ballotDef._ballotOptionDescription, global);
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
        SetVotingButtonsActive(false);
    }

    public void OptionLost()
    {
        _unlockedUI.SetActive(false);
        _optionWonUI.SetActive(false);
        _optionLostUI.SetActive(true);
        SetVotingButtonsActive(false);
    }

    public void SetTotalCount(int count)
    {
        print("count = " + count + " for " + _titleText);
        _voteCount.text = count.ToString();
    }

    public void SetVotingButtonsActive(bool active)
    {
        foreach(GameObject button in _votingButtons)
        {
            button.SetActive(active);
        }
    }

    public void VoteUpButton()
    {
        if(_global)
        {
            _gMgr.SpendSparksOnGlobalVote(_gMgr._activePlayer._playerID, _ballotID, _ballotOptionID);
        }
        else
        {
            _gMgr.SpendSparksOnBriefcaseVote(_gMgr._activePlayer._playerID, _teamID, _ballotID, _ballotOptionID);
        }
    }

    public void VoteDownButton()
    {
        if (_global)
        {
            _gMgr.RefundSparksOnGlobalVote(_gMgr._activePlayer._playerID, _ballotID, _ballotOptionID);
        }
        else
        {
            _gMgr.RefundSparksOnBriefcaseVote(_gMgr._activePlayer._playerID, _teamID, _ballotID, _ballotOptionID);
        }
    }
}
