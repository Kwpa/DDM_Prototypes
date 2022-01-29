using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GlobalBallotElement : UIElement
{
    public string _ballotID;
    public TextMeshProUGUI _titleText;
    public TextMeshProUGUI _descriptionText;

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

    public void SetBallotElement(string ballotID, string name, string description, List<BallotOption> options)
    {
        _ballotID = ballotID;
        _titleText.text = name;
        _descriptionText.text = description;
        _options = options;
    }

    public void SetBallotElement(BallotDef ballotDef)
    {
        SetBallotElement(ballotDef._ballotID, ballotDef._ballotTitle, ballotDef._ballotDescription, ballotDef._ballotOptions);
    }

    public void CreateOptions()
    {
        foreach (BallotOption option in _options)
        {
            GameObject go = Instantiate(_optionPrefab, _optionsParent.transform);
            BallotOptionElement optionEl = go.GetComponent<BallotOptionElement>();
            optionEl.SetValues(option, "", true);
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

    public void SetBallotResult(int winnerIndex, List<int> voteCounts)
    {
        for (int i = 0; i < _optionElements.Count; i++)
        {
            BallotOptionElement optionEl = _optionElements[i].GetComponent<BallotOptionElement>();
            if (i == winnerIndex)
            {
                optionEl.OptionWon();
            }
            else
            {
                optionEl.OptionLost();
            }
            optionEl.SetTotalCount(voteCounts[i]);
        }
    }

    public void EndBallot()
    {
        _lockedUI.SetActive(false);
        _unlockedUI.SetActive(false);
        _ballotEndedUI.SetActive(true);
    }



}