using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BriefcaseBallotElement : UIElement
{
    public string _ballotID;
    public string _teamID;
    public TextMeshProUGUI _titleText;
    public TextMeshProUGUI _descriptionText;

    public bool _storyRevealAcquired = false;
    public GameObject _lockedUI;
    public GameObject _unlockedUI;
    public GameObject _ballotEndedUI;

    public List<BallotOption> _options;
    public GameObject _optionPrefab;
    public GameObject _optionsParent;
    public List<GameObject> _optionElements;


    public override void Init()
    {
        InitBase();
        LockBallot();
    }

    public void SetBriefcaseBallotElement(string ballotID, string teamID, string name, string description, List<BallotOption> options)
    {
        _ballotID = ballotID;
        _teamID = teamID;
        _titleText.text = name;
        _descriptionText.text = description;
        _options = options;
    }

    public void SetBriefcaseBallotElement(BallotDef ballotDef, string teamID)
    {
        SetBriefcaseBallotElement(ballotDef._ballotID, teamID, ballotDef._ballotTitle, ballotDef._ballotDescription, ballotDef._ballotOptions);
    }

    public void CreateOptions()
    {
        foreach(BallotOption option in _options)
        {
            GameObject go = Instantiate(_optionPrefab, _optionsParent.transform);
            BallotOptionElement optionEl = go.GetComponent<BallotOptionElement>();
            optionEl.SetValues(option, _teamID);
            optionEl.Init();
            _optionElements.Add(go);
        }
    }

    public void LockBallot()
    {
        _unlockedUI.SetActive(false);
        _ballotEndedUI.SetActive(false);
        _lockedUI.SetActive(true);
    }

    public void UnlockBallot()
    {
        _lockedUI.SetActive(false);
        _ballotEndedUI.SetActive(false);
        _unlockedUI.SetActive(true);
    }

    public void EndBallot()
    {
        _lockedUI.SetActive(false);
        _unlockedUI.SetActive(false);
        _ballotEndedUI.SetActive(true);
    }



}
