using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class UserManager : MonoBehaviour
    {
        public void OnClick()
        {
            var name = this.gameObject.name;

            switch (name)
            {
                case "MyCollection":
                    var heroes = GameObject.Find("Heros").GetComponent<UILabel>();
                    StringBuilder message = new StringBuilder();

                    for (int i = 1; i < User.Instance.HeroCollection.Count + 1; i++)
                    {
                        message.Append(User.Instance.HeroCollection[i - 1] + " ");
                        if (i % 9 == 0)
                            message.Append("\n");
                    }
                    heroes.text = message.ToString();
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
}
