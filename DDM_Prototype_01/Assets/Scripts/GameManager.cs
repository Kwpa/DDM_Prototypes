using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;

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
    public int _rounds = 0;
    public int _roundTime;
    public int _dancingTime = 0;
    public int _actionPoints = 0;
    public int _sparkPoints = 0;
    public int _timeFactor = 1;
    public System.DateTime _startTime;
    public System.DateTime _endTime;
    public System.TimeSpan _timeSpan;
    public System.DateTime _startRound;

    public bool _debug = false;

    enum GameStates
    {
        FirstVisit,
        NewDay,
        NewRound,
        Dancing,
    }

    FiniteStateMachine<GameStates> _fsm = new FiniteStateMachine<GameStates>();

    //***************************************************************************
    //functions
    //***************************************************************************

    void Awake()
    {
        _uiMgr = GameObject.Find("UIManager").GetComponent<UIManager>();
    }


    void Start()
    {
        //setup UI and initial variables
        _uiMgr.Init();
        LoadGameProfile(_gameProfile);

        //time
        _timeSpan = new System.TimeSpan(0, 0, 0, 0);
        _startTime = System.DateTime.UtcNow;

        //state machine
        SetupStates();
    }

    void Update()
    {
        //time
        _endTime = System.DateTime.UtcNow;
        _timeSpan = _endTime - _startTime;

        //state machine
        _fsm.Update();
    }

    void FixedUpdate()
    {
        _fsm.FixedUpdate();    
    }

    void SetupStates()
    {
        _fsm.Add(
            new State<GameStates>(GameStates.FirstVisit, "FirstVisit", FirstVisitEnter, null, null, null));

        _fsm.Add(
            new State<GameStates>(GameStates.NewDay, "NewDay", NewDayEnter, null, null, null));

        _fsm.Add(
            new State<GameStates>(GameStates.NewRound, "NewRound", NewRoundEnter, null, null, null));

        _fsm.Add(
            new State<GameStates>(GameStates.Dancing, "Dancing", DancingEnter, null, null, null));

        _fsm.SetCurrentState(GameStates.FirstVisit);
    }

    public void LoadGameProfile(GameProfile profile)
    {
        GainActionPoints(profile._startingActionPoints);
        GainSparkPoints(profile._startingSparkPoints);
        SetTimeFactor(profile._timeFactor);
        _uiMgr.SetVariables(_days, _rounds, _actionPoints, _sparkPoints);
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
        _fsm.SetCurrentState(GameStates.NewDay);
    }

    public void NewDayEnter()
    {
        GainDays(1);
        _fsm.SetCurrentState(GameStates.NewRound);
    }

    public void NewRoundEnter()
    {
        GainRounds(1);
        _fsm.SetCurrentState(GameStates.Dancing);
    }

    public void DancingEnter()
    {
        
    }

    public void DancingUpdate()
    {

        if(_endTime-_startRound)
    }
}
