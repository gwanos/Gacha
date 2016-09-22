using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using Network;

public class LogInManager : MonoBehaviour, IWWWManager
{
    public RequestLogIn RequestLogIn { get; set; }
    public ResponseLogIn ResponseLogIn { get; set; }
    public string JsonString { get; set; }
    public delegate void HttpRequestDelegate(WWW www);
    public virtual event HttpRequestDelegate OnHttpRequest;

    public LogInManager()
    {
        RequestLogIn = new RequestLogIn();
        ResponseLogIn = new ResponseLogIn();
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

    public void OnClick()
    {
        var name = this.gameObject.name;
        switch (name)
        {
            case "Back":
                SceneManager.LoadScene(0);
                break;
            case "Join Us":
                SceneManager.LoadScene(2);
                break;
            case "Log In":
                LogIn();
                break;
        }
    }

    public void LogIn()
    {
        RequestLogIn = new RequestLogIn();
        var id = GameObject.Find("IDInput").transform.FindChild("UILabel").GetComponent<UILabel>().text;
        var password = GameObject.Find("PasswordInput").transform.FindChild("UILabel").GetComponent<UILabel>().text;

        if (id.Length == 0 || password.Length == 0)
        {
            var error = GameObject.Find("Error").GetComponent<UILabel>();
            error.color = Color.yellow;
            error.text = "항목이 비었습니다.";
            return;
        }

        RequestLogIn.UserId = id;
        RequestLogIn.Password = password;
        Debug.LogFormat("{0} / {1}", RequestLogIn.UserId, RequestLogIn.Password);

        Post(Urls.UrlLogIn, RequestLogIn);
    }


    public WWW Post<T>(string url, T userObject)
    {
        //
        JsonString = JsonConvert.SerializeObject(userObject);
        var postData = System.Text.Encoding.UTF8.GetBytes(JsonString);
        var www = new WWW(url, postData, Urls.headers);
        Debug.LogFormat("{0} / {1}", RequestLogIn.UserId, RequestLogIn.Password);

        // 
        StartCoroutine((IEnumerator)WaitForRequest(www));

        return www;
    }

    public IEnumerator WaitForRequest(WWW www)
    {
        // 응답이 올 때가지 기다림
        yield return www;

        // 응답이 왔다면, 이벤트 리스너에 응답 결과 전달
        bool hasCompleteListener = (OnHttpRequest != null);
        if (hasCompleteListener)
            OnHttpRequest(www);

        // check for errors
        if (www.error == null)
        {
            // 
            var responseLogIn = JsonConvert.DeserializeObject<ResponseLogIn>(www.text);
            var errorLabel = GameObject.Find("Error").GetComponent<UILabel>();
            errorLabel.color = Color.yellow;
            string errorMessage;
            if (responseLogIn.ResultCode != ResultCode.SUCCESS)
            {
                ErrorHandler.Instance.ReSultMap.TryGetValue(responseLogIn.ResultCode, out errorMessage);
                errorLabel.text = errorMessage;
            }
            else
            {
                User.Instance.UserId = responseLogIn.UserId;
                User.Instance.HeroCollection = responseLogIn.HeroCollection;

                //Debug.Log(User.Instance.UserId);
                //foreach (var value in User.Instance.HeroCollection)
                //{
                //    Debug.Log();
                //}

                ErrorHandler.Instance.ReSultMap.TryGetValue(responseLogIn.ResultCode, out errorMessage);
                errorLabel.text = "로그인 " + errorMessage;

                SceneManager.LoadScene(1);
            }
            //Debug.Log("Return from server: " + www.text);
        }
        else
        {
            Debug.Log("Error: " + www.error);
        }

        // 통신 해제
        www.Dispose();
    }
}