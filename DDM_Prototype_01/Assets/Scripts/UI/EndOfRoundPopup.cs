using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EndOfRoundPopup : Popup
{
    public TextMeshProUGUI _titleText;
    public TextMeshProUGUI _bodyText;

    public int _round;
    public List<string> _loserTeamNames;

    public override void InitPopup()
    {
        
    }

    public void SetValues(int round, List<string> _losersNames)
    {
        _titleText.text = "Round " + _round + " ends.";
        _bodyText.text = "";
        //_bodyText.text.
    }

}
