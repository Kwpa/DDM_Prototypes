using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PersonalityStatElement : UIElement
{
    public enum StatComparison
    {
        More,
        Less,
        Equal
    }

    public override void Init()
    {
        //
    }

    public string _teamID;
    public string _statID; 
    public TextMeshProUGUI _text;
    public Image _icon;

    public void SetStatText(string compOne, string compTwo, StatComparison statComparison)
    {
        switch (statComparison)
        {
            case StatComparison.More:
                _text.text = "More " + compOne.ToUpper() + " than " + compTwo.ToUpper();
                break;
            case StatComparison.Less:
                _text.text = "More " + compTwo.ToUpper() + " than " + compOne.ToUpper();
                break;
            case StatComparison.Equal:
                _text.text = "Equal in "  + compOne.ToUpper() + " and " + compTwo.ToUpper();
                break;
        }
    }
}
