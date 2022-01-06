using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject _stagePrefab;
    public GameObject _infoBarPrefab;
    public GameObject _chatFeedPrefab;
    public GameObject _chatMessagePrefab;
    public GameObject _broadcasterFeedPrefab;
    public GameObject _broadcasterMessagePrefab;
    public GameObject _avatarPrefab;
    public Dictionary<string, GameObject> _baseSpawnedItems;

    public void Init()
    {
        _baseSpawnedItems = new Dictionary<string, GameObject>();
        _baseSpawnedItems.Add("stage", Instantiate(_stagePrefab, this.transform));
        _baseSpawnedItems.Add("infoBar", Instantiate(_infoBarPrefab, this.transform));
        _baseSpawnedItems.Add("chatFeed", Instantiate(_chatFeedPrefab, this.transform));
        _baseSpawnedItems.Add("broadcasterFeed", Instantiate(_broadcasterFeedPrefab, this.transform));
    }

    public void LoadGameProfile()
    {
        
    }
}
