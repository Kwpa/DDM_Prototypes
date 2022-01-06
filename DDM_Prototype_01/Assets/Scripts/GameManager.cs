using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameProfile _gameProfile;
    UIManager _uiMgr;

    public int _days = 0;
    public int _rounds = 0;
    public int _actionPoints = 0;
    public int _sparkPoints = 0;
    public int _timeFactor = 1;
    public System.TimeSpan _timeSpan;


    void Awake()
    {
        _uiMgr = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    void Start()
    {
        LoadGameProfile(_gameProfile);
        _uiMgr.Init();
        _timeSpan = new System.TimeSpan(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGameProfile(GameProfile profile)
    {
        GainDays(profile._days);
        GainRounds(profile._rounds);
        GainActionPoints(profile._startingActionPoints);
        GainSparkPoints(profile._startingSparkPoints);
        SetTimeFactor(profile._timeFactor);
        _uiMgr.SetVariables(profile._rounds, profile._days, profile._startingActionPoints, profile._startingSparkPoints);
    }

    public void GainDays(int value)
    {

    }

    public void GainRounds(int value)
    {

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
}
