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
    public int _roundMins;
    public int _roundSecs;
    public int _dancingTime = 0;
    public int _timeFactor = 1;

    public DateTime _startTime;
    public DateTime _endTime;
    public TimeSpan _timeSpan;
    public DateTime _startRound;
    public TimeSpan _roundSpan;

    public Dictionary<string, Team> _teams;
    public Dictionary<string, Player> _players;
    public Player _activePlayer;

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

        //setup UI and initial variables (create teams, players etc)
        _uiMgr.Init();
        LoadGameProfile(_gameProfile);

        //time
        _timeSpan = new System.TimeSpan(0, 0, 0, 0);
        _startTime = DateTime.UtcNow;
    }

    void Update()
    {
        //time
        UpdateTime();

        //state machine
        _fsm.Update();

        //players
        UpdatePlayers();

        //teams
        UpdateTeams();

        //ui
        _uiMgr.UpdateUI();
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

    public void UpdateTime()
    {
        _endTime = DateTime.UtcNow;
        _timeSpan = _endTime - _startTime;
        _roundMins = Mathf.Clamp(_roundTime - _roundSpan.Minutes - 1, 0, 100000);
        _roundSecs = Mathf.Clamp(59 - _roundSpan.Seconds, 0, 100000);
        if (GetCurrentStateName() == "Dancing")
        {
            _roundMins -= _roundTime - _dancingTime;
        }
        else if (GetCurrentStateName() == "Resting")
        {
            _roundMins -= _dancingTime;
        }
        _roundMins = Mathf.Clamp(_roundMins, 0, 100000);
    }

    public void UpdateTeams()
    {
        foreach (KeyValuePair<string, Team> kvp in _teams)
        {
            kvp.Value.UpdateTeam();
        }
    }

    public void UpdatePlayers()
    {
        foreach(KeyValuePair<string,Player> kvp in _players)
        {
            kvp.Value.UpdatePlayer(_teams);
        }
    }

    public string GetCurrentStateName()
    {
        return _fsm.GetCurrentState().Name;
    }

    public void LoadGameProfile(GameProfile profile)
    {
        SetTimeFactor(profile._timeFactor);
        _daysPerGame = profile._daysPerGame;
        _roundsPerDay = profile._roundsPerDay;
        _roundTime = profile._roundTime;
        _dancingTime = profile._dancingTime;

        CreateTeams(profile);
        CreatePlayers(profile);

        //temp: set active player
        _activePlayer = _players[profile._activePlayerID];
    

        _uiMgr.UpdateUI();
    }

    public void CreatePlayers(GameProfile profile)
    {
        _players = new Dictionary<string, Player>();
        List<PlayerProfile> playerProfiles = profile._playerProfiles;
        foreach (PlayerProfile pp in playerProfiles)
        {
            _players.Add(pp._playerID, new Player(pp, _teams));
        }
        foreach(KeyValuePair<string, Player> kvp in _players)
        {
            Player player = kvp.Value;
            player.GainActionPoints(profile._startingActionPoints);
            player.GainSparkPoints(profile._startingSparkPoints);
            player.SetupPlayerToTeamData(_teams);
        }
    }

    public void CreateTeams(GameProfile profile)
    {
        _teams = new Dictionary<string, Team>();
        List<TeamProfile> teamProfiles = profile._teamProfiles;
        foreach(TeamProfile tp in teamProfiles)
        {
            _teams.Add(tp._teamID, new Team(tp));
        }

        foreach(KeyValuePair<string, Team> kvp in _teams)
        {
            Team t = kvp.Value;
            t._assignedAvatar = _uiMgr.SpawnAvatar(t);
            t._assignedProfilePopup = _uiMgr.SpawnTeamPopup(t);
        }
    }

    public void SpendActionOnDonation(string playerID, string teamID)
    {
        Player player = _players[playerID];
        Team team = _teams[teamID];
        int substractionCheck = player._actionPoints - 1;
        if (substractionCheck >= 0)
        {
            int actionCost = 1;
            player.SpendActionPoints(actionCost);
            team.GainHealth(player._playerToTeamData[teamID]._teamDonationAmount);
        }
    }

    public void GainDays(int value)
    {
        _days += value;
    }

    public void GainRounds(int value)
    {
        _rounds += value;
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
