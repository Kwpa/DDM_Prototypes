using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject _stagePrefab;
    public GameObject _infoBarPrefab;
    public GameObject _chatFeedPrefab;
    public GameObject _chatMessagePrefab;
    public GameObject _broadcasterPrefab;
    public GameObject _broadcasterMessagePrefab;
    public GameObject _avatarPrefab;
    public GameObject _teamProfilePopupPrefab;
    public Dictionary<string, GameObject> _baseUI;
    public Dictionary<string, GameObject> _avatarsUI;
    public Dictionary<string, GameObject> _messagesUI;
    public Dictionary<string, GameObject> _teamProfilePopupsUI;

    GameManager _gMgr;
    bool _activeUpdate = false;

    public void Init()
    {
        _gMgr = GameObject.Find("GameManager").GetComponent<GameManager>();
        _baseUI = new Dictionary<string, GameObject>();
        _avatarsUI = new Dictionary<string, GameObject>();
        _messagesUI = new Dictionary<string, GameObject>();
        _teamProfilePopupsUI = new Dictionary<string, GameObject>();

        SpawnBaseUI();

        _activeUpdate = true;
    }

    public void SpawnBaseUI()
    {
        _baseUI.Add("stage", Instantiate(_stagePrefab, this.transform));
        _baseUI.Add("infoBar", Instantiate(_infoBarPrefab, this.transform));
        _baseUI.Add("chatFeed", Instantiate(_chatFeedPrefab, this.transform));
        _baseUI.Add("broadcaster", Instantiate(_broadcasterPrefab, this.transform));
        _baseUI.Add("broadcastFeed", _baseUI["broadcaster"].GetComponentInChildren<BroadcastFeed>().gameObject);
        _baseUI.Add("votingBooth", _baseUI["broadcaster"].GetComponentInChildren<VotingBooth>().gameObject);
        foreach(KeyValuePair<string,GameObject> kvp in _baseUI)
        {
            UIElement uiElement = kvp.Value.GetComponent<UIElement>();
            uiElement.Init();
        }
    }

    public GameObject SpawnTeamPopup(Team team)
    {
        GameObject popupGo = Instantiate(_teamProfilePopupPrefab, this.transform);

        TeamProfilePopup popup = popupGo.GetComponent<TeamProfilePopup>();
        popup._teamID = team._teamID;
        popup.InitBase();
        _teamProfilePopupsUI.Add(team._teamID, popupGo);
        
        return popupGo;
    }

    public GameObject SpawnAvatar(Team team)
    {
        GameObject avatarGo = Instantiate(_avatarPrefab, _baseUI["stage"].transform);

        TeamAvatar teamAvatar = avatarGo.GetComponent<TeamAvatar>();
        teamAvatar._teamID = team._teamID;
        teamAvatar.InitBase();
        _avatarsUI.Add(team._teamID, avatarGo);
        
        
        return avatarGo;
    }

    public void UpdateInfoBar()
    {
        InfoBar infoBar = _baseUI["infoBar"].GetComponent<InfoBar>();
        infoBar.SetInfoBarVariables(
            _gMgr._currentDay,
            _gMgr._currentRound,
            _gMgr._activePlayer._actionPoints,
            _gMgr.GetCurrentStateName(),
            _gMgr._activePlayer._sparkPoints,
            _gMgr._roundMins,
            _gMgr._roundSecs
            );
    }

    public void UpdateAvatars()
    {
        foreach(KeyValuePair<string,GameObject> kvp in _avatarsUI)
        {
            TeamAvatar avatar = kvp.Value.GetComponent<TeamAvatar>();
            Team teamRef = _gMgr._teams[avatar._teamID];
            PlayerToTeamData data = _gMgr._activePlayer._playerToTeamData[avatar._teamID];

            avatar.SetValues(
                teamRef._teamName,
                teamRef._donationNeeded,
                data._teamDonationAmount,
                teamRef._teamMaxHealth,
                teamRef._teamHealth
                );
        }
    }

    public void SetTeamProfilePopups()
    {
        foreach (KeyValuePair<string, GameObject> kvp in _teamProfilePopupsUI)
        {
            TeamProfilePopup popup = kvp.Value.GetComponent<TeamProfilePopup>();
            Team teamRef = _gMgr._teams[popup._teamID];
            PlayerToTeamData data = _gMgr._activePlayer._playerToTeamData[popup._teamID];

            popup.SetValues(
                teamRef._teamName,
                teamRef._donationNeeded,
                teamRef._teamBio,
                data._playerIsInFanClub
                );

            popup.CreateCards(
                teamRef._teamPersonalityStats,
                teamRef._teamUpgrades,
                teamRef._teamStoryReveals
                );
        }
    }

    public void UpdateTeamProfilePopup(string teamID)
    {
        TeamProfilePopup popup = _teamProfilePopupsUI[teamID].GetComponent<TeamProfilePopup>();
        Team teamRef = _gMgr._teams[teamID];
        PlayerToTeamData data = _gMgr._activePlayer._playerToTeamData[teamID];

        popup.SetValues(
            teamRef._teamName,
            teamRef._donationNeeded,
            teamRef._teamBio,
            data._playerIsInFanClub
            );

        popup.SetCards(
            teamRef._teamPersonalityStats,
            teamRef._teamUpgrades,
            teamRef._teamStoryReveals
            );
    }

    public void UIVisibility(bool hidden, GameObject go)
    {
        go.SetActive(!hidden);
    }

    

    public void UpdateUI()
    {
        UpdateInfoBar();
        UpdateAvatars();
        SetTeamProfilePopups();

        if (_baseUI != null && _baseUI.Count > 0)
        foreach (KeyValuePair<string,GameObject> entry in _baseUI)
        {
            bool hidden = entry.Value.GetComponent<UIElement>()._hidden;
            UIVisibility(hidden, entry.Value);
        }
        if (_avatarsUI != null && _avatarsUI.Count > 0)
        foreach (KeyValuePair<string, GameObject> entry in _avatarsUI)
        {
            bool hidden = entry.Value.GetComponent<UIElement>()._hidden;
            UIVisibility(hidden, entry.Value);
        }

        if (_teamProfilePopupsUI != null && _teamProfilePopupsUI.Count > 0)
        foreach (KeyValuePair<string, GameObject> entry in _teamProfilePopupsUI)
        {
            bool hidden = entry.Value.GetComponent<UIElement>()._hidden;
            UIVisibility(hidden, entry.Value);
        }

        if(_messagesUI!=null && _messagesUI.Count >0)
        foreach (KeyValuePair<string, GameObject> entry in _messagesUI)
        {
            bool hidden = entry.Value.GetComponent<UIElement>()._hidden;
            UIVisibility(hidden, entry.Value);
        }
    }

    public void UpdateUpgrade(string teamID, string upgradeID)
    {
        TeamProfilePopup popup = _teamProfilePopupsUI[teamID].GetComponent<TeamProfilePopup>();
        popup.SetUpgradeButtonStatus(upgradeID);
    }

    public void OpenTeamProfilePopup(string teamID)
    {
        TeamProfilePopup popup = _teamProfilePopupsUI[teamID].GetComponent<TeamProfilePopup>();
        popup.Open();
    }

    public void CloseTeamProfilePopup(string teamID)
    {
        TeamProfilePopup popup = _teamProfilePopupsUI[teamID].GetComponent<TeamProfilePopup>();
        popup.Close();
    }

    public void OpenStoryRevealPopup(string teamID, string storyRevealID)
    {
        TeamProfilePopup popup = _teamProfilePopupsUI[teamID].GetComponent<TeamProfilePopup>();
        popup.Open();
    }

    public void CloseStoryRevealPopup(string teamID, string storyRevealID)
    {
        TeamProfilePopup popup = _teamProfilePopupsUI[teamID].GetComponent<TeamProfilePopup>();
        popup.Close();
    }
}
