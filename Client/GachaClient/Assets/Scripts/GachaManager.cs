using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Scripts;
using Excel;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using Network;

public class GachaManager : MonoBehaviour, IWWWManager
{
    private static GachaManager _instance;
    private List<int> _heroIds;

    public RequestGacha RequestGacha { get; set; }
    public List<int> HeroIds { get { return _heroIds; }}
    public delegate void HttpRequestDelegate(WWW www);
    public virtual event HttpRequestDelegate OnHttpRequest;
    public DisplayManager displayManager = new DisplayManager();
    public string JsonString { get; set; }
    
    public static GachaManager Instance
    {
        get
        {
            if (_instance == null)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(GachaManager)) as GachaManager;
                }
                if (_instance == null)
                {
                    GameObject gameObject = new GameObject("gameObject");
                    _instance = gameObject.AddComponent(typeof(GachaManager)) as GachaManager;
                }
            }
            return _instance;
        }
    }

    private GachaManager()
    {
        RequestGacha = new RequestGacha();
        _heroIds = new List<int>();
    }

    // === === //
    public void Start()
    {
        displayManager.displayEvent += new displayDelegate(displayHeroInfo);
        var userIdLabel = GameObject.Find("UserId").GetComponent<UILabel>();
        userIdLabel.color = Color.yellow;
        userIdLabel.text = "ID: " + User.Instance.UserId;
    }

    public void Choose(int gachaId)
    {
        RequestGacha.GachaId = gachaId;
    }
    
    public void PickHeroes()
    {
        RequestGacha.UserId = User.Instance.UserId;
        Post(Urls.UrlGacha, RequestGacha);
    }

    public WWW Post<T>(string url, T userObject)
    {
        // Object to string
        // Convert json string to byte
        JsonString = JsonConvert.SerializeObject(userObject);
        var postData = System.Text.Encoding.UTF8.GetBytes(JsonString);
        var www = new WWW(url, postData, Urls.headers);

        // Start Coroutine
        StartCoroutine((IEnumerator)WaitForRequest(www));

        return www;
    }

    public IEnumerator WaitForRequest(WWW www)
    {
        // Wait until receivng response
        yield return www;

        // Pass the response to Event listener
        bool hasCompleteListener = (OnHttpRequest != null);
        if (hasCompleteListener)
            OnHttpRequest(www);

        // check for errors
        if (www.error == null)
        {
            Debug.Log("Return from server: " + www.text);
        }
        else
        {
            Debug.Log("Error: " + www.error);
        }

        // Save on the List
        var jsonFromServer = JsonConvert.DeserializeObject<ResponseGacha>(www.text);
        //var responseGacha = new Network.ResponseGacha();
        if (jsonFromServer.ResultCode != ResultCode.SUCCESS)
        {
            // Error handling    
            Debug.LogError(jsonFromServer.ResultCode.ToString());
        }
        
        //responseGacha.ResultCode = jsonFromServer.ResultCode;
        //responseGacha.HeroIds = jsonFromServer.HeroIds;
        _heroIds = jsonFromServer.HeroIds;
        foreach (var value in _heroIds)
        {
            User.Instance.HeroCollection.Add(value);
        }

        // Event Handler
        displayManager.ReceiveIDsFromServer();

        // Dispose
        www.Dispose();
    }
    
    public void displayHeroInfo()
    {
        int index = 0;

        // Destroy
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Card");
        foreach (var obj in objects)
        {
            Destroy(obj);
        }

        HeroInfo heroInfo = new HeroInfo();
        foreach (var heroID in GachaManager.Instance.HeroIds)
        {
            foreach (var hero in HeroData.Instance.Table)
            {
                if (heroID == hero.Key)
                {
                    heroInfo = hero.Value;
                    break;
                }
            }
            Debug.LogFormat("{0}, {1}, {2}, {3}", heroInfo.Id, heroInfo.Name, heroInfo.Grade, heroInfo.Property);

            var prefab = NGUITools.AddChild(GameObject.Find("HeroCardManager"), HeroData.Instance.HeroCard) as GameObject;
            var heroCardImage = prefab.transform.FindChild("HeroCardImage").GetComponent<UISprite>();
            var heroCardName = prefab.transform.Find("HeroCard_Name").GetComponent<UILabel>();
            var heroCardProperty = prefab.transform.Find("HeroCard_Property").GetComponent<UILabel>();
            var heroCardGrade = prefab.transform.Find("HeroCard_Grade").GetComponent<UILabel>();

            prefab.transform.localScale = new Vector3(400, 500, 0);
            prefab.transform.localPosition = HeroData.Instance.spawanPosition[index++];
            heroCardImage.spriteName = heroInfo.Name;
            heroCardName.text = heroInfo.Name.ToString();
            heroCardGrade.text = heroInfo.Grade.ToString();
            switch (heroInfo.Property)
            {
                case 1: // Fire
                    heroCardProperty.text = HeroProperty.Fire.ToString();
                    break;
                case 2: // Water
                    heroCardProperty.text = HeroProperty.Water.ToString();
                    break;
                case 3: // Wind
                    heroCardProperty.text = HeroProperty.Wind.ToString();
                    break;
            }
        }
    }
}