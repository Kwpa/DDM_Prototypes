using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VotingBooth : UIElement
{
    public GameObject _globalBallotPrefab;
    public GameObject _globalBallotParent;
    public List<GameObject> _globalBallotsUI;

    public override void Init()
    {
        base.InitBase();
    }

    public void CreateGlobalBallot(BallotDef ballot)
    {
        print("Create ballot " + ballot._ballotTitle);
        GameObject ballotGo = Instantiate(_globalBallotPrefab, _globalBallotParent.transform);
        ballotGo.GetComponent<GlobalBallotElement>().SetBallotElement(ballot);
        ballotGo.GetComponent<GlobalBallotElement>().CreateOptions();
        ballotGo.GetComponent<GlobalBallotElement>().Init();

        _globalBallotsUI.Add(ballotGo);
    }

    public void SetBallot(BallotDef ballot)
    {
        GameObject ballotGo = _globalBallotsUI.FirstOrDefault(p => p.GetComponent<GlobalBallotElement>()._ballotID == ballot._ballotID);
        if (ballotGo == null) return;

        ballotGo.GetComponent<GlobalBallotElement>().SetBallotElement(ballot);
    }

    public void SetBallotStatuses()
    { 
        foreach (GameObject go in _globalBallotsUI)
        {
            GlobalBallotElement ballotElement = go.GetComponent<GlobalBallotElement>();
            BallotDef ballotDef = _gMgr._globalBallots[ballotElement._ballotID];
            ballotElement.SetBallotElement(ballotDef);
        }
    }

    public void SetBallotStatus(string ballotID)
    {
        GameObject go = _globalBallotsUI.FirstOrDefault(p => p.GetComponent<GlobalBallotElement>()._ballotID == ballotID);
        GlobalBallotElement element = go.GetComponent<GlobalBallotElement>();
        BallotDef upgradeDef = _gMgr._globalBallots[element._ballotID];
        element.SetBallotElement(upgradeDef);
    }

    public void UnlockBallot(int index)
    {
        if (index - 1 >= 0 && index-1 < _globalBallotsUI.Count)
        {
            _globalBallotsUI[index - 1].GetComponent<GlobalBallotElement>().UnlockBallot();
        }
    }

    public void EndBallot(int index)
    {
        if (index - 2 >= 0 && index-2 < _globalBallotsUI.Count) _globalBallotsUI[index - 2].GetComponent<GlobalBallotElement>().EndBallot();
    }
}
