using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIElement : MonoBehaviour
{
    public bool _hidden;
    [HideInInspector] public GameManager _gMgr;
    [HideInInspector] public UIManager _uiMgr;

    public void InitBase()
    {
        GetGameManager();
        GetUIManager();
    }

    public void GetGameManager()
    {
        _gMgr = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void GetUIManager()
    {
        _uiMgr = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    public abstract void Init();

    public void UIVisibility()
    {
        this.gameObject.SetActive(!_hidden);
    }
}
