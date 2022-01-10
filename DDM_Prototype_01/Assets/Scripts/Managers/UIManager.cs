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
    public Dictionary<string, GameObject> _baseUI;
    public Dictionary<string, GameObject> _avatarsUI;
    public Dictionary<string, GameObject> _messagesUI;

    GameManager _gMgr;
    bool _activeUpdate = false;

    public void Init()
    {
        _gMgr = GameObject.Find("GameManager").GetComponent<GameManager>();

        SpawnBaseUI();

        _activeUpdate = true;
    }

    public void SpawnBaseUI()
    {
        _baseUI = new Dictionary<string, GameObject>();
        _baseUI.Add("stage", Instantiate(_stagePrefab, this.transform));
        _baseUI.Add("infoBar", Instantiate(_infoBarPrefab, this.transform));
        _baseUI.Add("chatFeed", Instantiate(_chatFeedPrefab, this.transform));
        _baseUI.Add("broadcaster", Instantiate(_broadcasterPrefab, this.transform));
        _baseUI.Add("broadcastFeed", _baseUI["broadcaster"].GetComponentInChildren<BroadcastFeed>().gameObject);
        _baseUI.Add("votingBooth", _baseUI["broadcaster"].GetComponentInChildren<VotingBooth>().gameObject);
    }

    public GameObject SpawnAvatar(Team team)
    {
        GameObject avatar = Instantiate(_avatarPrefab, _baseUI["stage"].transform);
        avatar.GetComponent<TeamAvatar>()._teamID = team._teamID;
        _avatarsUI.Add(team._teamID, avatar);
        
        return avatar;
    }

    public void UpdateInfoBar()
    {
        _baseUI["infoBar"].GetComponent<InfoBar>().SetInfoBarVariables(
            _gMgr._days,
            _gMgr._rounds,
            _gMgr._actionPoints,
            _gMgr.GetCurrentStateName(),
            _gMgr._sparkPoints,
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
            avatar.SetValues(
                teamRef.);
        }
    }

    public void UpdateUI()
    {
        if(_activeUpdate)
        {
            UpdateInfoBar();

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
        }
    }
}
