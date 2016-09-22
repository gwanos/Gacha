using UnityEngine;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts;

public delegate void displayDelegate();
public class DisplayManager
{
    public event displayDelegate displayEvent;

    public void ReceiveIDsFromServer()
    {
        if(GachaManager.Instance.HeroIds.Count == 0)
            Debug.Log("Not received yet."); 
        else
        {
            displayEvent();
        }
    }
}