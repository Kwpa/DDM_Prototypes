using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;
using System;

//***************************************************************************
//game manager with finite state machine and time tracking
//***************************************************************************

public class GameManager : MonoBehaviour
{

    //***************************************************************************
    //variables
    //***************************************************************************

    public GameProfile _gameProfile;
    UIManager _uiMgr;

    public int _days = 0;
    public int _daysPerGame = 0;
    public int _rounds = 0;
    public int _roundsPerDay = 0;
    public int _roundTime;
    public int _dancingTime = 0;
    public int _actionPoints = 0;
    public int _sparkPoints = 0;
    public int _timeFactor = 1;

    public DateTime _startTime;
    public DateTime _endTime;
    public TimeSpan _timeSpan;
    public DateTime _startRound;
    public TimeSpan _roundSpan;

    public bool _debug = false;

    enum GameStates
    {
        FirstVisit,
        NewDay,
        NewRound,
        Dancing,
        Resting
    }

    FiniteStateMachine<GameStates> _fsm = new FiniteStateMachine<GameStates>();

    //***************************************************************************
    //Unity functions
    //***************************************************************************

    void Awake()
    {
        _uiMgr = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    void Start()
    {
        //state machine
        SetupStates();

        //setup UI and initial variables
        _uiMgr.Init();
        LoadGameProfile(_gameProfile);

        //time
        _timeSpan = new System.TimeSpan(0, 0, 0, 0);
        _startTime = DateTime.UtcNow;
    }

    void Update()
    {
        //time
        _endTime = DateTime.UtcNow;
        _timeSpan = _endTime - _startTime;
        int roundMins = Mathf.Clamp(_roundTime-_roundSpan.Minutes-1, 0, 100000);
        int roundSecs = Mathf.Clamp(59 - _roundSpan.Seconds, 0, 100000);
        if(GetCurrentStateName()=="Dancing")
        {
            roundMins -= _roundTime - _dancingTime;
        }
        else if (GetCurrentStateName()=="Resting")
        {
            roundMins -= _dancingTime;
        }
        roundMins = Mathf.Clamp(roundMins, 0, 100000);

        //state machine
        _fsm.Update();

        //ui
        _uiMgr.SetVariables(_days, _rounds, _actionPoints, _sparkPoints, roundMins, roundSecs);
    }

    void FixedUpdate()
    {
        _fsm.FixedUpdate();    
    }

    //***************************************************************************
    //state setup
    //***************************************************************************

    void SetupStates()
    {
        _fsm.Add(
            new State<GameStates>(GameStates.FirstVisit, "FirstVisit", FirstVisitEnter, null, null, null));

        _fsm.Add(
            new State<GameStates>(GameStates.NewDay, "NewDay", NewDayEnter, null, null, null));

        _fsm.Add(
            new State<GameStates>(GameStates.NewRound, "NewRound", NewRoundEnter, null, null, null));

        _fsm.Add(
            new State<GameStates>(GameStates.Dancing, "Dancing", DancingEnter, null, DancingUpdate, null));

        _fsm.Add(
            new State<GameStates>(GameStates.Resting, "Resting", RestingEnter, null, RestingUpdate, null));


        _fsm.SetCurrentState(GameStates.FirstVisit);
    }

    //***************************************************************************
    //game functions
    //***************************************************************************

    public string GetCurrentStateName()
    {
        return _fsm.GetCurrentState().Name;
    }

    public void LoadGameProfile(GameProfile profile)
    {
        GainActionPoints(profile._startingActionPoints);
        GainSparkPoints(profile._startingSparkPoints);
        SetTimeFactor(profile._timeFactor);
        _daysPerGame = profile._daysPerGame;
        _roundsPerDay = profile._roundsPerDay;
        _roundTime = profile._roundTime;
        _dancingTime = profile._dancingTime;

        SpawnAvatars(profile);

        _uiMgr.SetVariables(_days, _rounds, _actionPoints, _sparkPoints,_roundTime,0);
    }

    public void SpawnAvatars(GameProfile profile)
    {
        
    }

    public void GainDays(int value)
    {
        _days += value;
    }

    public void GainRounds(int value)
    {
        _rounds += value;
    }

    public void GainActionPoints(int value)
    {
        _actionPoints += value;
        _actionPoints = Mathf.Clamp(_actionPoints, 0, 1000);
    }

    public void LoseActionPoints(int value)
    {
        _actionPoints -= value;
        _actionPoints = Mathf.Clamp(_actionPoints, 0, 1000);
    }

    public void SpendActionPoints(int value)
    {
        LoseActionPoints(value);
    }

    public void GainSparkPoints(int value)
    {
        _sparkPoints += value;
        _sparkPoints = Mathf.Clamp(_sparkPoints, 0, 1000);
    }

    public void LoseSparkPoints(int value)
    {
        _sparkPoints -= value;
        _sparkPoints = Mathf.Clamp(_sparkPoints, 0, 1000);
    }

    public void SpendSparkPoints(int value)
    {
        LoseSparkPoints(value);
    }

    public void SetTimeFactor(int value)
    {
        _timeFactor = value;
    }

    public void SetRoundTime(int value)
    {
        _roundTime = value;
    }

    public void SetDancingTime(int value)
    {
        _dancingTime = value;
    }

    //***************************************************************************
    //state delegate definitions
    //***************************************************************************

    public void FirstVisitEnter()
    {
        print("First Visit");
        _fsm.SetCurrentState(GameStates.NewDay);
    }

    public void NewDayEnter()
    {
        print("New Day");
        GainDays(1);

        //show ui

        _fsm.SetCurrentState(GameStates.NewRound);
    }

    public void NewRoundEnter()
    {
        print("New Round");
        GainRounds(1);
        _fsm.SetCurrentState(GameStates.Dancing);
    }

    public void DancingEnter()
    {
        print("Dancing");
        _startRound = DateTime.UtcNow;

        //show ui
    }

    public void DancingUpdate()
    {
        //print("DancingUpdate");
        _roundSpan = _endTime - _startRound;

        if (_roundSpan.TotalMinutes >= _dancingTime)
        {
            _fsm.SetCurrentState(GameStates.Resting);
        }
    }

    public void RestingEnter()
    {
        print("Resting");
        
        // show ui
    }

    public void RestingUpdate()
    {
        _roundSpan = _endTime - _startRound;
        if (_roundSpan.TotalMinutes >= _roundTime)
        {
            if(_rounds >= _roundsPerDay)
            {
                _fsm.SetCurrentState(GameStates.NewDay);
            }
            else
            {
                _fsm.SetCurrentState(GameStates.NewRound);
            }
        }
    }
}
