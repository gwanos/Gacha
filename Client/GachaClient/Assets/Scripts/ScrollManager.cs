using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public delegate void ScrollDelegate(int number);

public class ScrollManager : MonoBehaviour
{
    public int _number;
    private ScrollDelegate scrollDelegate;
    
    public void OnClick()
    {
        GachaManager.Instance.RequestGacha.GachaId = _number;
        Debug.Log(GachaManager.Instance.RequestGacha.GachaId);
    }

    public void OnHover(bool isOver)
    {
        var uiLabel = this.transform.FindChild("Label").GetComponent<UILabel>();
        if (isOver)
        {
            uiLabel.color = Color.magenta;
        }
        else
        {
            uiLabel.color = Color.white;
        }
        
    }

    public void Start()
    {
    }
}