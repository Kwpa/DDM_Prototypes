using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoryRevealElement : UIElement
{
    public string _storyRevealID;
    public string _teamID;
    public TextMeshProUGUI _titleText;
    public TextMeshProUGUI _descriptionText;

    public bool _storyRevealAcquired = false;
    public GameObject _lockedUI;
    public GameObject _acquiredUI;

    public override void Init()
    {
        //
    }

    public void SetStoryRevealElement(string storyRevealID, string teamID, string name, string description)
    {
        _storyRevealID = storyRevealID;
        _teamID = teamID;
        _titleText.text = name;
        _descriptionText.text = description;
    }

    public void SetStoryRevealElement(StoryRevealDef storyRevealDef, string teamID)
    {
        SetStoryRevealElement(storyRevealDef._storyRevealID, teamID, storyRevealDef._storySectionTitle, storyRevealDef._storySectionDescription);
    }

    public void LockUpgrade()
    {
        _acquiredUI.SetActive(false);
        _lockedUI.SetActive(true);
    }

    public void AcquiredUpgrade()
    {
        _acquiredUI.SetActive(true);
        _lockedUI.SetActive(false);
    }

    public void ViewStoryContent()
    {
        if (!_storyRevealAcquired)
        {
            _uiMgr.OpenStoryRevealPopup(_teamID, _storyRevealID);
        }
    }
}
