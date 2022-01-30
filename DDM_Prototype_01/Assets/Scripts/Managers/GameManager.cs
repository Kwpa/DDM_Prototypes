using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;
using System;
using System.Linq;

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
    PlayerBotManager _pbMgr;

    public int _currentDay = 0;
    public int _daysPerGame = 0;
    public int _currentRound = 0;
    public int _roundsPerDay = 0;
    public List<RoundDef> _roundDefs;
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
    public bool _pauseTime = false;

    public Dictionary<string, Team> _teams;
    public Dictionary<string, Player> _players;
    [SerializeField]
    public Dictionary<string, BallotDef> _globalBallots;
    public Player _activePlayer;

    public bool _debug = false;
    public bool _activeUpdate = false;

    enum GameStates
    {
        FirstVisit,
        NewDay,
        NewRound,
        Dancing,
        Resting,
        EndRound,
        EndGame
    }

    FiniteStateMachine<GameStates> _fsm = new FiniteStateMachine<GameStates>();

    //***************************************************************************
    //Unity functions
    //***************************************************************************

    void Awake()
    {
        _uiMgr = GameObject.Find("UIManager").GetComponent<UIManager>();
        _pbMgr = GameObject.Find("PlayerBotManager").GetComponent<PlayerBotManager>();
    }

    void Start()
    {
        print("Start");

        //ui
        _uiMgr.Init();

        //profile
        LoadGameProfile(_gameProfile);
        
        //state machine
        SetupStates();

        //ui
        _uiMgr.UpdateUI();

        //time
        _timeSpan = new System.TimeSpan(0, 0, 0, 0);
        _startTime = DateTime.UtcNow;

        //set update to active
        _activeUpdate = true;
    }

    void Update()
    {
        if (_activeUpdate)
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
            //_uiMgr.UpdateUI();
        }
        
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

        _fsm.Add(
            new State<GameStates>(GameStates.EndRound, "EndRound", EndOfRoundEnter, null, null, null));

        _fsm.Add(
            new State<GameStates>(GameStates.EndGame, "EndGame", EndOfGameEnter, null, null, null));


        _fsm.SetCurrentState(GameStates.FirstVisit);
    }

    //***************************************************************************
    //game functions
    //***************************************************************************

    public void SkipToEndOfRound()
    {
        _fsm.SetCurrentState(GameStates.EndRound);
    }

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

        if (!_pauseTime)
        {
            _uiMgr._baseUI["infoBar"].GetComponent<InfoBar>().SetTimeLeftText(_roundMins, _roundSecs);
        }
    }

    public void UpdateTeams()
    {
        foreach (KeyValuePair<string, Team> kvp in _teams)
        {
            Team team = kvp.Value;
            team.UpdateTeam();
            //print(team._donationNeeded);
        }
    }

    public void UpdatePlayers()
    {
        if(_players != null)
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
        print("load game profile");
        SetTimeFactor(profile._timeFactor);
        _daysPerGame = profile._daysPerGame;
        _roundsPerDay = profile._roundsPerDay;
        _roundTime = profile._roundTime;
        _dancingTime = profile._dancingTime;

        CreateTeams(profile);
        _pbMgr.Init(profile._numberOfBotPlayers);
        CreatePlayers(profile);
        CreateGlobalBallots(profile);

        //temp: set active player
        _activePlayer = _players[profile._activePlayerID];

        _uiMgr.SetTeamProfilePopups();
    }

    public void CreatePlayers(GameProfile profile)
    {
        _players = new Dictionary<string, Player>();
        List<PlayerProfile> playerProfiles = profile._playerProfiles;
        foreach (PlayerProfile pp in playerProfiles)
        {
            _players.Add(pp._playerID, new Player(pp, _teams));
        }
        foreach(Player p in _pbMgr._generatedPlayers)
        {
            _players.Add(p._playerID, p);
        }
        foreach(KeyValuePair<string, Player> kvp in _players)
        {
            Player player = kvp.Value;

            if (!player._playerIsBot)
            {
                player.SetBaseActionPoints(profile._baseActionPoints); //set at player creation
            }
            player.GainSparkPoints(profile._startingSparkPoints);
            //player.SetupPlayerToTeamData(_teams);
            // to remove ^^^
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

        print("created teams");
    }

    public void CreateGlobalBallots(GameProfile profile)
    {
        _globalBallots = new Dictionary<string, BallotDef>();
        List<BallotDef> globalBallots = profile._globalBallots;
        foreach (BallotDef gb in globalBallots)
        {
            _globalBallots.Add(gb._ballotID, new BallotDef(gb._ballotID, gb._ballotTitle, gb._ballotDescription, gb._ballotOptions, gb._dayActive));
            _uiMgr.SpawnGlobalBallot(gb);
        }

        print("created ballots");
    }

    public void SpendActionOnHealthDonation(string playerID, string teamID)
    {
        Player player = _players[playerID];
        Team team = _teams[teamID];
        int actionPointCheck = player._actionPoints - 1;
        int donationNeededCheck = team._donationNeeded;
        if (actionPointCheck >= 0 && donationNeededCheck > 0)
        {
            int actionCost = 1;
            player.SpendActionPoints(actionCost);
            team.GainHealth(player._playerToTeamData[teamID]._teamDonationAmount);
            int reward = player.GainSparksFromTeam(teamID, team._baseSparkRewardAmount);

            print("Username: " + player._username + " donated health to " + team._teamName);
            print("Username: " + player._username + " gained " + reward + " sparks");

            if (playerID == _activePlayer._playerID)
            {
                ActivePlayerDonates(teamID);
            }
            else
            {
                BotPlayerDonates(teamID);
            }
        }
    }

    public void SpendActionOnJoiningFanClub(string playerID, string teamID)
    {
        Player player = _players[playerID];
        Team team = _teams[teamID];
        int actionPointCheck = player._actionPoints - 1;
        bool fanCheck = player._playerToTeamData[team._teamID]._playerIsInFanClub;
        if (actionPointCheck >= 0 && !fanCheck)
        {
            int actionCost = 1;
            player.SpendActionPoints(actionCost);
            player.JoinFanClub(team._teamID);
            print("Username: " + player._username + " joined fan club of " + team._teamName);

            if (playerID == _activePlayer._playerID)
            {
                ActivePlayerJoinsFanClub(teamID);
            }
            else
            {
                BotJoinsFanClub(teamID);
            }
        }
    }

    public void SpendActionOnUpgradingTeam(string playerID, string teamID, string upgradeID)
    {
        Player player = _players[playerID];
        Team team = _teams[teamID];
        int actionPointCheck = player._actionPoints - 1;
        bool fanCheck = player._playerToTeamData[team._teamID]._playerIsInFanClub;
        if (actionPointCheck >= 0 && fanCheck)
        {
            UpgradeDef upgrade = team._teamUpgrades.Find(p => p._upgradeID == upgradeID);
            team._teamUpgradeLevel++;
            player.SpendActionPoints(upgrade._upgradeCost);

            player.GainTeamUpgrade(team._teamID, upgrade);
            
            print("Username: " + player._username + " gained the " + team._teamName + " " + upgrade._upgradeName);

            if (playerID == _activePlayer._playerID)
            {
                ActivePlayerUpgradesRelationship(teamID, upgradeID);
            }
            else
            {
                BotUpgradesRelationship(teamID, upgradeID);
            }
        }
    }

    public void SpendActionOnNextUpgradeForTeam(string playerID, string teamID)
    {
        Player player = _players[playerID];
        Team team = _teams[teamID];
        if(team._teamUpgradeLevel + 1 < team._teamUpgrades.Count)
        {
            string nextUpgradeID = team._teamUpgrades[team._teamUpgradeLevel + 1]._upgradeID;
            SpendActionOnUpgradingTeam(playerID, teamID, nextUpgradeID);
        }
    }

    public void SpendSparksOnGlobalVote(string playerID, string ballotID, string ballotOptionID)
    {
        Player player = _players[playerID];
        int actionPointCheck = player._sparkPoints - 1;
        
        if (actionPointCheck >= 0)
        {
            BallotDef ballot = _globalBallots[ballotID];
            ballot.IncreaseVote(ballotOptionID);

            int sparkCost = 1;
            player.SpendSparkPoints(sparkCost);
            player.UpdateGlobalBallotContribution(ballotID, ballotOptionID, 1);
            int voteCount = player.GetOptionCurrentGlobalContribution(ballotID, ballotOptionID);
            print("Username: " + player._username + " voted for  " + ballotOptionID + " in the " + ballot._ballotTitle + " ballot");

            if (playerID == _activePlayer._playerID)
            {
                ActivePlayerGlobalVote(ballotID, ballotOptionID, voteCount);
            }
            else
            {
                BotGlobalVote(playerID, ballotID, ballotOptionID);
            }
        }
    }

    public void SpendSparksOnCurrentGlobalVote(string playerID)
    {
        BallotDef ballot = _globalBallots.FirstOrDefault(p=>p.Value._dayActive == _currentDay).Value;
        string ballotID = ballot._ballotID;
        var rand = new System.Random();
        int randIndex = rand.Next(0, ballot._ballotOptions.Count);
        string randomBallotOptionID = ballot._ballotOptions[randIndex]._ballotOptionID;

        SpendSparksOnGlobalVote(playerID, ballotID, randomBallotOptionID);
    }

    public void SpendSparksOnBriefcaseVote(string playerID, string teamID, string ballotID, string ballotOptionID)
    {
        Player player = _players[playerID];
        Team team = _teams[teamID];
        int sparkCheck = player._sparkPoints - 1;

        if (sparkCheck >= 0)
        {
            BallotDef ballot = team._teamBriefcaseBallots.Find(p => p._ballotID == ballotID);
            ballot.IncreaseVote(ballotOptionID);

            int sparkCost = 1;
            player.SpendSparkPoints(sparkCost);
            player._playerToTeamData[teamID].UpdateBallotContribution(ballotID, ballotOptionID, 1);
            int voteCount = player._playerToTeamData[teamID].GetOptionCurrentContribution(ballotID, ballotOptionID);
            
            // this happens upon evaluation!
            //BallotDef ballot = team._teamBriefcaseBallots.Find(p => p._ballotID == ballotID);
            //ballot.SetChosenOption(ballotOptionID);
            //BallotOption option = ballot.GetChosenBallotOption();

            print("Username: " + player._username + " voted for  " + team._teamName + "'s " + ballotOptionID + " briefcase item");

            if (playerID == _activePlayer._playerID)
            {
                ActivePlayerBriefcaseVote(teamID, ballotID, ballotOptionID, voteCount);
            }
            else
            {
                BotBriefcaseVote(teamID, ballotID, ballotOptionID);
            }
        }
    }

    public void SpendSparksOnCurrentBriefcaseVote(string playerID, string teamID)
    {
        Team team = _teams[teamID];
        BallotDef ballot = team._teamBriefcaseBallots[_currentDay - 1];
        string ballotID = ballot._ballotID;
        var rand = new System.Random();
        int randIndex = rand.Next(0, ballot._ballotOptions.Count);
        string randomBallotOptionID = ballot._ballotOptions[randIndex]._ballotOptionID;

        SpendSparksOnBriefcaseVote(playerID, teamID, ballotID, randomBallotOptionID);
    }

    public void RefundSparksOnBriefcaseVote(string playerID, string teamID, string ballotID, string ballotOptionID)
    {
        Player player = _players[playerID];
        Team team = _teams[teamID];
        int sparkCheck = player._playerToTeamData[teamID].GetOptionCurrentContribution(ballotID, ballotOptionID);

        if (sparkCheck > 0)
        {
            BallotDef ballot = team._teamBriefcaseBallots.Find(p => p._ballotID == ballotID);
            ballot.DecreaseVote(ballotOptionID);

            int sparkRefund = 1;
            player._playerToTeamData[teamID].UpdateBallotContribution(ballotID, ballotOptionID, -1);
            player.GainSparkPoints(sparkRefund);
            int voteCount = player._playerToTeamData[teamID].GetOptionCurrentContribution(ballotID, ballotOptionID);

            print("Username: " + player._username + " removed a vote for  " + team._teamName + "'s " + ballotOptionID + " briefcase item");

            if (playerID == _activePlayer._playerID)
            {
                ActivePlayerBriefcaseVote(teamID, ballotID, ballotOptionID, voteCount);
            }
            else
            {
                BotBriefcaseVote(teamID, ballotID, ballotOptionID);
            }
        }
    }

    public void RefundSparksOnGlobalVote(string playerID, string ballotID, string ballotOptionID)
    {
        Player player = _players[playerID];
        int sparkCheck = player.GetOptionCurrentGlobalContribution(ballotID, ballotOptionID);

        if (sparkCheck > 0)
        {
            BallotDef ballot = _globalBallots[ballotID];
            ballot.DecreaseVote(ballotOptionID);

            int sparkRefund = 1;
            player.UpdateGlobalBallotContribution(ballotID, ballotOptionID, -1);
            player.GainSparkPoints(sparkRefund);
            int voteCount = player.GetOptionCurrentGlobalContribution(ballotID, ballotOptionID);

            print("Username: " + player._username + " removed a vote for " + ballotOptionID + " glboal vote");

            if (playerID == _activePlayer._playerID)
            {
                ActivePlayerGlobalVote(ballotID, ballotOptionID, voteCount);
            }
            else
            {
                BotBriefcaseVote(playerID, ballotID, ballotOptionID);
            }
        }
    }

    public void ActivePlayerDonates(string teamID)
    {
        //ui
        int getActions = _activePlayer._actionPoints;
        int getRemaining = _teams[teamID]._donationNeeded;
        int getMaxHealth = _teams[teamID]._teamMaxHealth;
        int getHealth = _teams[teamID]._teamHealth;
        int getSparks = _activePlayer._sparkPoints;

        _uiMgr._baseUI["infoBar"].GetComponent<InfoBar>().SetActionsText(getActions);
        _uiMgr._baseUI["infoBar"].GetComponent<InfoBar>().SetSparksText(getSparks);
        _uiMgr._avatarsUI[teamID].GetComponent<TeamAvatar>().SetCTAText(getRemaining);
        _uiMgr._avatarsUI[teamID].GetComponent<TeamAvatar>().SetHealthBar(getMaxHealth, getHealth);
        _uiMgr._teamProfilePopupsUI[teamID].GetComponent<TeamProfilePopup>().SetCTAText(getRemaining);
        _uiMgr._teamProfilePopupsUI[teamID].GetComponent<TeamProfilePopup>().SetHealthBar(getMaxHealth, getHealth);
    }

    public void BotPlayerDonates(string teamID)
    {
        //ui
        int getRemaining = _teams[teamID]._donationNeeded;
        int getMaxHealth = _teams[teamID]._teamMaxHealth;
        int getHealth = _teams[teamID]._teamHealth;

        _uiMgr._avatarsUI[teamID].GetComponent<TeamAvatar>().SetCTAText(getRemaining);
        _uiMgr._avatarsUI[teamID].GetComponent<TeamAvatar>().SetHealthBar(getMaxHealth, getHealth);
        _uiMgr._teamProfilePopupsUI[teamID].GetComponent<TeamProfilePopup>().SetCTAText(getRemaining);
        _uiMgr._teamProfilePopupsUI[teamID].GetComponent<TeamProfilePopup>().SetHealthBar(getMaxHealth, getHealth);
    }

    public void ActivePlayerJoinsFanClub(string teamID)
    {
        int getActions = _activePlayer._actionPoints;
        _uiMgr._baseUI["infoBar"].GetComponent<InfoBar>().SetActionsText(getActions);
        _uiMgr._avatarsUI[teamID].GetComponent<TeamAvatar>().JoinFanClub();
        _uiMgr._teamProfilePopupsUI[teamID].GetComponent<TeamProfilePopup>().JoinFanClub();
    }

    public void BotJoinsFanClub(string id)
    {
        //
    }

    public void ActivePlayerUpgradesRelationship(string teamID, string upgradeID)
    {
        int getActions = _activePlayer._actionPoints;
        
        _uiMgr._baseUI["infoBar"].GetComponent<InfoBar>().SetActionsText(getActions);
        _uiMgr._teamProfilePopupsUI[teamID].GetComponent<TeamProfilePopup>().SetUpgradeButtonStatuses();
        _uiMgr._avatarsUI[teamID].GetComponent<TeamAvatar>().UpgradeLevelIncrease();
        _uiMgr._teamProfilePopupsUI[teamID].GetComponent<TeamProfilePopup>().UpgradeLevelIncrease();
    }

    public void BotUpgradesRelationship(string teamID, string upgradeID)
    {
        //
    }

    public void ActivePlayerGlobalVote(string ballotID, string optionID, int count)
    {
        int getSparks = _activePlayer._sparkPoints;
        _uiMgr.UpdateGlobalBallotOption(ballotID, optionID, count);
        _uiMgr._baseUI["infoBar"].GetComponent<InfoBar>().SetSparksText(getSparks);
    }

    public void BotGlobalVote(string playerID, string ballotID, string choiceID)
    {
        //
    }

    public void ActivePlayerBriefcaseVote(string teamID, string ballotID, string optionID, int count)
    {
        int getSparks = _activePlayer._sparkPoints;
        _uiMgr.UpdateBriefcaseOption(teamID, ballotID, optionID, count);
        _uiMgr._baseUI["infoBar"].GetComponent<InfoBar>().SetSparksText(getSparks);
    }

    public void BotBriefcaseVote(string teamID, string ballotID, string choiceID)
    {
        //
    }

    public void ActivePlayerUpgrade(string teamID, string upgradeID)
    {
        SpendActionOnUpgradingTeam(_activePlayer._playerID, teamID, upgradeID);
        _uiMgr.UpdateTeamProfilePopup(teamID);
    }

    public void GainDays(int value)
    {
        _currentDay += value;
    }

    public void GainRounds(int value)
    {
        _currentRound += value;
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

    public void SetTeamsMaxHealthPerPlayerCountPerRound()
    {
        foreach (KeyValuePair<string, Team> kvp in _teams)
        {
            Team team = kvp.Value;
            
            RoundDef defFromRound = _gameProfile._roundDefs.FirstOrDefault(p => p._roundNumber == _currentRound);
            //print("Rounds values" + defFromRound._setMaxHealth + " " + defFromRound._setHealth);
            if (defFromRound != null)
            {
                team.SetMaxHealth(defFromRound._setMaxHealth);
                team.SetHealth(defFromRound._setHealth);
                team.GetDonationNeeded();
                _uiMgr.UpdateAvatar(team._teamID);
                _uiMgr.UpdateTeamProfilePopup(team._teamID);
            }
        }
    }

    public void ResetPlayersActionPoints()
    {
        foreach(KeyValuePair<string, Player> kvp in _players)
        {
            Player player = kvp.Value;
            player.ResetActionPoints();
            _uiMgr.UpdateInfoBar();
        }
    }

    public void KickLosingTeams()
    {

        foreach (KeyValuePair<string, Team> kvp in _teams.OrderByDescending(p => p.Value._donationNeeded))
        {
            Team team = kvp.Value;
            if(!team._outOfCompetition)
            {
                team.EvaluateHealth();
                if(team._outOfCompetition)
                {
                    print("Get them out! " + team._teamName);
                    _uiMgr.UpdateAvatar(team._teamID);
                    _uiMgr.UpdateTeamProfilePopup(team._teamID);
                    return;
                }
                
            }
        }
    }

    public List<string> GetLosersIDs()
    {
        List<string> loserIDs = new List<string>();
        
        foreach(KeyValuePair<string,Team> kvp in _teams)
        {
            Team team = kvp.Value;
            if (team._outOfCompetition) loserIDs.Add(team._teamID);
        }

        return loserIDs;
    }

    public void UnlockRoundDependantElements()
    {
        //_uiMgr.
    }

    public void UnlockDayDependantElements()
    {
        _uiMgr.UnlockBriefcaseBasedOnDay(_currentDay);
        _uiMgr.UnlockGlobalBallotBasedOnDay(_currentDay);
    }

    public void EvaluateVotes(int day)
    {
        BallotDef globalBallot = _globalBallots.FirstOrDefault(p => p.Value._dayActive == _currentDay).Value;
        int winnerIndexGlobal = globalBallot.EvaluateBallot();
        List<int> globalBallotScores = globalBallot.GetFinalVoteAmounts();
        _uiMgr.EvaluateGlobalBallotUI(globalBallot._ballotID, winnerIndexGlobal, globalBallotScores);

        foreach (KeyValuePair<string, Team> kvp in _teams)
        {
            Team team = kvp.Value;
            BallotDef briefcaseBallot = team._teamBriefcaseBallots[day - 1];
            int winnerIndexBriefcase = briefcaseBallot.EvaluateBallot();
            List<int> briefcaseBallotScores = briefcaseBallot.GetFinalVoteAmounts();
            _uiMgr.EvalauteBriefcaseBallotUI(team._teamID, briefcaseBallot._ballotID, winnerIndexBriefcase, briefcaseBallotScores);
        }
    }

    //***************************************************************************
    //state delegate definitions
    //***************************************************************************

    public void FirstVisitEnter()
    {
        print("First Visit");
        _uiMgr.SpawnFirstTimePopup(); 
        _fsm.SetCurrentState(GameStates.NewDay);
    }

    public void NewDayEnter()
    {
        print("New Day");
        GainDays(1);
        UnlockDayDependantElements();
        //ui
        _uiMgr._baseUI["infoBar"].GetComponent<InfoBar>().SetDaysText(_currentDay);

        _fsm.SetCurrentState(GameStates.NewRound);
    }

    public void NewRoundEnter()
    {
        print("New Round");
        GainRounds(1);
        SetTeamsMaxHealthPerPlayerCountPerRound(); // todo: could be put into new day at this stage?
        UnlockRoundDependantElements();
        ResetPlayersActionPoints();
        _pbMgr.BotsPerformActions();
        //ui
        _uiMgr._baseUI["infoBar"].GetComponent<InfoBar>().SetRoundsText(_currentRound);

        _fsm.SetCurrentState(GameStates.Dancing);
    }

    public void DancingEnter()
    {
        print("Dancing");
        _startRound = DateTime.UtcNow;

        //ui
        _uiMgr._baseUI["infoBar"].GetComponent<InfoBar>().SetRoundSectionText(GetCurrentStateName());
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

        //ui
        _uiMgr._baseUI["infoBar"].GetComponent<InfoBar>().SetRoundSectionText(GetCurrentStateName());
    }

    public void RestingUpdate()
    {
        _roundSpan = _endTime - _startRound;
        if (_roundSpan.TotalMinutes >= _roundTime)
        {
            _fsm.SetCurrentState(GameStates.EndRound);
        }
    }

    public void EndOfRoundEnter()
    {
        print("EndOfRound");
        KickLosingTeams();
        EvaluateVotes(_currentDay);
        //_uiMgr.ShowEndOfRoundPopup();

        if (_currentRound >= _roundsPerDay)
        {
            if (_currentDay > _daysPerGame)
            {
                _fsm.SetCurrentState(GameStates.EndGame);
            }
            else
            {
                _fsm.SetCurrentState(GameStates.NewDay);
            }
        }
        else
        {
            _fsm.SetCurrentState(GameStates.NewRound);
        }
    }

    public void EndOfGameEnter()
    {
        print("EndOfGame");
    }
}
