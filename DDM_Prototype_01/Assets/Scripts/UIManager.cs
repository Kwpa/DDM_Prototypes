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
    public Dictionary<string, GameObject> _baseSpawnedItems;

    bool _activeUpdate = false;

    public void Init()
    {
        _baseSpawnedItems = new Dictionary<string, GameObject>();
        _baseSpawnedItems.Add("stage", Instantiate(_stagePrefab, this.transform));
        _baseSpawnedItems.Add("infoBar", Instantiate(_infoBarPrefab, this.transform));
        _baseSpawnedItems.Add("chatFeed", Instantiate(_chatFeedPrefab, this.transform));
        _baseSpawnedItems.Add("broadcaster", Instantiate(_broadcasterPrefab, this.transform));
        _baseSpawnedItems.Add("broadcastFeed", _baseSpawnedItems["broadcaster"].GetComponentInChildren<BroadcastFeed>().gameObject);
        _baseSpawnedItems.Add("votingBooth", _baseSpawnedItems["broadcaster"].GetComponentInChildren<VotingBooth>().gameObject);
        _activeUpdate = true;
    }

    public void SetVariables(int days, int rounds, int actions, int sparks, int minutesLeft, int secondsLeft)
    {
        InfoBar infoBar = _baseSpawnedItems["infoBar"].GetComponent<InfoBar>();
        infoBar.SetDaysText(days);
        infoBar.SetRoundsText(rounds);
        infoBar.SetActionsText(actions);
        infoBar.SetSparksText(sparks);
        infoBar.SetTimeLeftText(minutesLeft, secondsLeft);
    }

    private void Update()
    {
        if(_activeUpdate)
        {
            foreach(KeyValuePair<string,GameObject> entry in _baseSpawnedItems)
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
