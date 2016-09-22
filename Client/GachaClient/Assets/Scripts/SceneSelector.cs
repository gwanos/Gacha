using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using Assets.Scripts;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
    public void OnClick()
    {
        var name = this.gameObject.name;

        switch (name)
        {
            case "Play":
                if (User.Instance.UserId == null)
                    SceneManager.LoadScene(3);
                else
                    SceneManager.LoadScene(1);
                break;
            case "Log In":
                SceneManager.LoadScene(3);
                break;
            case "My Page":
                if (User.Instance.UserId == null)
                    SceneManager.LoadScene(3);
                else
                    SceneManager.LoadScene(4);
                break;
        }
    }

    public void OnHover(bool isOver)
    {
        var uiLabel = this.GetComponent<UILabel>();
        if (isOver)
        {
            uiLabel.color = Color.yellow;
        }
        else
        {
            uiLabel.color = Color.white;
        }
    }
}
