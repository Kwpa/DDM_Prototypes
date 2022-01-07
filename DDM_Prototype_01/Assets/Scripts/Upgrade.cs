using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;

public class Upgrade
{
    enum UpgradeState
    {
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
        Level6,
        Level7,
        Level8,
        Level9,
        Level10
    }

    public string _upgradeName = "upgrade";
    public List<bool> _upgradeBool;
    public List<string> _upgradeString;
    public List<int> _upgradeInt;

    FiniteStateMachine<UpgradeState> _fsm = new FiniteStateMachine<UpgradeState>();

    public void Init()
    {
        SetupStates();
    }

    void SetupStates()
    {
        _fsm.Add(
            new State<UpgradeState>(UpgradeState.Level1, "Level1", Level1, null, null, null));

        _fsm.Add(
            new State<UpgradeState>(UpgradeState.Level2, "Level2", Level2, null, null, null));

        _fsm.Add(
            new State<UpgradeState>(UpgradeState.Level3, "Level3", Level3, null, null, null));

        _fsm.Add(
            new State<UpgradeState>(UpgradeState.Level4, "Level4", Level4, null, null, null));

        _fsm.Add(
            new State<UpgradeState>(UpgradeState.Level5, "Level5", Level5, null, null, null));

        _fsm.Add(
            new State<UpgradeState>(UpgradeState.Level6, "Level6", Level6, null, null, null));

        _fsm.Add(
            new State<UpgradeState>(UpgradeState.Level7, "Level7", Level7, null, null, null));

        _fsm.Add(
            new State<UpgradeState>(UpgradeState.Level8, "Level8", Level8, null, null, null));

        _fsm.Add(
            new State<UpgradeState>(UpgradeState.Level9, "Level9", Level9, null, null, null));

        _fsm.Add(
           new State<UpgradeState>(UpgradeState.Level10, "Level10", Level10, null, null, null));

        _fsm.SetCurrentState(UpgradeState.Level1);
    }

    public void LevelUpgrade()
    {
        int stateCount = 10;
        if((int)_fsm.GetCurrentState().ID < stateCount)
        {
            UpgradeState nextState = _fsm.GetCurrentState().ID + 1;
            _fsm.SetCurrentState(nextState);
        }
    }

    public void Level1()
    {
    }

    public void Level2()
    {
    }

    public void Level3()
    {
    }

    public void Level4()
    {
    }

    public void Level5()
    {
    }

    public void Level6()
    {
    }

    public void Level7()
    {
    }

    public void Level8()
    {
    }

    public void Level9()
    {
    }

    public void Level10()
    {
    }

}


