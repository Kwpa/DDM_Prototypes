using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Popup : UIElement
{
    public override void Init()
    {
        //...
        InitPopup();
    }

    public abstract void InitPopup(); 

    public virtual void Open()
    {
        _hidden = false;
    }

    public virtual void Close()
    {
        _hidden = true;
    }
}
