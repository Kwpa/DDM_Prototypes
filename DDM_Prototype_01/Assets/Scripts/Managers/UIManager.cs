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
            _gMgr._days,
            _gMgr._rounds,
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
                data._teamDonationAmount
                );
        }
    }

    public void UpdateTeamProfilePopups()
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
                teamRef._teamPersonalityStats,
                teamRef._teamUpgrades,
                teamRef._teamStoryReveals,
                data._playerIsInFanClub
                );
        }
    }

    public void UpdateUI()
    {
        if(_activeUpdate)
        {
            UpdateInfoBar();
            UpdateAvatars();
            UpdateTeamProfilePopups();

            foreach (KeyValuePair<string,GameObject> entry in _baseUI)
            {
                if(entry.Value.GetComponent<UIElement>()._hidden)
                {
                    entry.Value.SetActive(false);
                }
                else
                {
                    entry.Value.SetActive(true);
                }
            }

            foreach (KeyValuePair<string, GameObject> entry in _avatarsUI)
            {
                if (entry.Value.GetComponent<UIElement>()._hidden)
                {
                    entry.Value.SetActive(false);
                }
                else
                {
                    entry.Value.SetActive(true);
                }
            }

            foreach (KeyValuePair<string, GameObject> entry in _teamProfilePopupsUI)
            {
                if (entry.Value.GetComponent<UIElement>()._hidden)
                {
                    entry.Value.SetActive(false);
                }
                else
                {
                    entry.Value.SetActive(true);
                }
            }

            foreach (KeyValuePair<string, GameObject> entry in _messagesUI)
            {
                if (entry.Value.GetComponent<UIElement>()._hidden)
                {
                    entry.Value.SetActive(false);
                }
                else
                {
                    entry.Value.SetActive(true);
                }
            }
        }
    }

    public void OpenTeamProfilePopup(string teamID)
    {
        print(teamID);
        _teamProfilePopupsUI[teamID].GetComponent<TeamProfilePopup>().Open();
    }
}