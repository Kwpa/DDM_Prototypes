using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class TeamProfilePopup : Popup
{
    public string _teamID;
    public TextMeshProUGUI _teamNameText;
    public TextMeshProUGUI _callToActionText;
    public Slider _healthBar;
    public TextMeshProUGUI _bio;
    public Image _iconBackground;
    public Image _icon;
    public List<GameObject> _stats;
    public List<GameObject> _upgrades;
    public List<GameObject> _storyReveals;
    public List<GameObject> _briefcaseBallots;


    public GameObject _statParent;
    public GameObject _upgradeParent;
    public GameObject _storyRevealParent;
    public GameObject _briefcaseBallotParent;

    public GameObject _fanClubContentBlocker;

    public GameObject _statPrefab;
    public GameObject _upgradePrefab;
    public GameObject _storyRevealPrefab;
    public GameObject _briefcaseBallotPrefab;

    public override void InitPopup()
    {
        _stats = new List<GameObject>();
        _upgrades = new List<GameObject>();
        _storyReveals = new List<GameObject>();
        _briefcaseBallots = new List<GameObject>();
    }

    public void SetValues(string name, int requirement, string bio, int maxHealth, int currentHealth, bool playerInFanClub, bool outOfCompetition)
    {
        SetTeamNameText(name);
        SetCTAText(requirement);
        SetBioText(bio);
        SetHealthBar(maxHealth, currentHealth);
        if(outOfCompetition)
        {
            KickTeamUI();
        }
    }

    public void CreateCards(List<PersonalityStatDef> stats, List<UpgradeDef> upgrades, List<StoryRevealDef> storyReveals, List<BallotDef> briefcaseBallots)
    {
        foreach (PersonalityStatDef stat in stats)
        {
            CreatePersonalityStat(stat);
        }
        foreach (UpgradeDef upgrade in upgrades)
        {
            CreateUpgrade(upgrade);
        }
        foreach (StoryRevealDef reveal in storyReveals)
        {
            CreateStoryReveal(reveal);
        }
        print("!!! - " + _teamID + "briefcount here ****000" + briefcaseBallots.Count);
        foreach (BallotDef ballot in briefcaseBallots)
        {
            CreateBriefcaseBallot(ballot);
        }
        SetUpgradeButtonStatuses();
        SetBriefcaseBallotStatuses();
    }

    public void SetCards(List<PersonalityStatDef> stats, List<UpgradeDef> upgrades, List<StoryRevealDef> storyReveals, List<BallotDef> briefcaseBallots)
    {
        //foreach (PersonalityStatDef stat in stats)
        //{
        //    //CreatePersonalityStat(stat);
        //}
        //foreach (UpgradeDef upgrade in upgrades)
        //{
        //    CreateUpgrade(upgrade);
        //}
        //foreach (StoryRevealDef reveal in storyReveals)
        //{
        //    //CreateStoryReveal(reveal);
        //}
        SetUpgradeButtonStatuses();
        SetBriefcaseBallotStatuses();
    }

    public void SetTeamNameText(string value)
    {
        _teamNameText.text = value;
    }

    public void SetCTAText(int value)
    {
        _callToActionText.text = "Needs " + value + " more health today";
    }

    public void SetHealthBar(int maxValue, int currentValue)
    {
        _healthBar.minValue = 0;
        _healthBar.maxValue = maxValue;
        _healthBar.value = currentValue;
    }

    public void SetBioText(string value)
    {
        _bio.text = value;
    }

    public void CreatePersonalityStat(PersonalityStatDef stat)
    {
        GameObject statGo = Instantiate(_statPrefab, _statParent.transform);
        statGo.GetComponent<PersonalityStatElement>().SetStatText(stat._statComparisonValueOne, stat._statComparisonValueTwo, stat.statComparison);
        _stats.Add(statGo);
    }

    public void SetPersonalityStatElement(PersonalityStatDef stat)
    {
        GameObject statGo = _stats.FirstOrDefault(p => p.GetComponent<PersonalityStatElement>()._statID == stat._statID);
        if (statGo == null) return;

        statGo.GetComponent<PersonalityStatElement>().SetStatText(stat._statComparisonValueOne, stat._statComparisonValueTwo, stat.statComparison);
    }

    public void CreateUpgrade(UpgradeDef upgrade)
    {
        GameObject upgradeGo = Instantiate(_upgradePrefab, _upgradeParent.transform);
        upgradeGo.GetComponent<UpgradeElement>().SetUpgradeElement(upgrade, _teamID);
        _upgrades.Add(upgradeGo);
    }

    public void SetUpgradeElements(UpgradeDef upgrade)
    {
        GameObject upgradeGo = _stats.FirstOrDefault(p => p.GetComponent<UpgradeElement>()._upgradeID == upgrade._upgradeID);
        if (upgradeGo == null) return;

        upgradeGo.GetComponent<UpgradeElement>().SetUpgradeElement(upgrade, _teamID);
    }

    public void SetUpgradeButtonStatuses()
    {
        List<UpgradeDef> upgradeDefs = _gMgr._teams[_teamID]._teamUpgrades;

        foreach(GameObject go in _upgrades)
        {
            UpgradeElement upgradeElement = go.GetComponent<UpgradeElement>();
            UpgradeDef upgradeDef = upgradeDefs.FirstOrDefault(p => p._upgradeID == upgradeElement._upgradeID);
            upgradeElement.SetUpgradeButton(upgradeDef);
        }
    }

    public void SetUpgradeButtonStatus(string upgradeID)
    {
        GameObject go = _upgrades.FirstOrDefault(p => p.GetComponent<UpgradeElement>()._upgradeID == upgradeID);
        UpgradeElement element = go.GetComponent<UpgradeElement>();
        List<UpgradeDef> upgradeDefs = _gMgr._teams[_teamID]._teamUpgrades;
        UpgradeDef upgradeDef = upgradeDefs.FirstOrDefault(p => p._upgradeID == element._upgradeID);
        element.SetUpgradeButton(upgradeDef);
    }

    public void CreateStoryReveal(StoryRevealDef storyReveal)
    {
        GameObject storyGo = Instantiate(_storyRevealPrefab, _storyRevealParent.transform);
        storyGo.GetComponent<StoryRevealElement>().SetStoryRevealElement(storyReveal, _teamID);
        _storyReveals.Add(storyGo);
    }

    public void SetStoryReveal(StoryRevealDef storyReveal)
    {
        GameObject storyGo = _storyReveals.FirstOrDefault(p => p.GetComponent<StoryRevealElement>()._storyRevealID == storyReveal._storyRevealID);
        if (storyGo == null) return;

        storyGo.GetComponent<StoryRevealElement>().SetStoryRevealElement(storyReveal, _teamID);
    }

    public void CreateBriefcaseBallot(BallotDef ballot)
    {
        GameObject ballotGo = Instantiate(_briefcaseBallotPrefab, _briefcaseBallotParent.transform);
        ballotGo.GetComponent<BriefcaseBallotElement>().SetBriefcaseBallotElement(ballot, _teamID);
        ballotGo.GetComponent<BriefcaseBallotElement>().CreateOptions();
        _briefcaseBallots.Add(ballotGo);
    }

    public void SetBriefcaseBallot(BallotDef ballot)
    {
        GameObject ballotGo = _briefcaseBallots.FirstOrDefault(p => p.GetComponent<BriefcaseBallotElement>()._ballotID == ballot._ballotID);
        if (ballotGo == null) return;

        ballotGo.GetComponent<BriefcaseBallotElement>().SetBriefcaseBallotElement(ballot, _teamID);
    }

    public void SetBriefcaseBallotStatuses()
    {
        List<BallotDef> ballotDefs = _gMgr._teams[_teamID]._teamBriefcaseBallots;

        foreach (GameObject go in _briefcaseBallots)
        {
            BriefcaseBallotElement ballotElement = go.GetComponent<BriefcaseBallotElement>();
            BallotDef ballotDef = ballotDefs.FirstOrDefault(p => p._ballotID == ballotElement._ballotID);
            ballotElement.SetBriefcaseBallotElement(ballotDef,_teamID);
        }
    }

    public void SetBriefcaseBallotStatus(string ballotID)
    {
        GameObject go = _briefcaseBallots.FirstOrDefault(p => p.GetComponent<BriefcaseBallotElement>()._ballotID == ballotID);
        BriefcaseBallotElement element = go.GetComponent<BriefcaseBallotElement>();
        List<BallotDef> upgradeDefs = _gMgr._teams[_teamID]._teamBriefcaseBallots;
        BallotDef upgradeDef = upgradeDefs.FirstOrDefault(p => p._ballotID == element._ballotID);
        element.SetBriefcaseBallotElement(upgradeDef, _teamID);
    }


    public void UnlockFanClubContent()
    {
        _gMgr.SpendActionOnJoiningFanClub(_gMgr._activePlayer._playerID, _teamID);
        _fanClubContentBlocker.SetActive(false);
    }

    public void KickTeamUI()
    {

    }

    public void EndBriefcaseBallot(int index)
    {
        if (index - 2 >= 0) _briefcaseBallots[index - 2].GetComponent<BriefcaseBallotElement>().EndBallot();
    }

    public void UnlockBriefcaseBallot(int index)
    {
        print("88888 " + _briefcaseBallots.Count);
        if(index-1 >= 0) _briefcaseBallots[index - 1].GetComponent<BriefcaseBallotElement>().UnlockBallot();
    }
}
