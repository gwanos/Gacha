using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using Network;

namespace Assets.Scripts
{
    public class SignInManager : MonoBehaviour, IWWWManager
    {
        public RequestSignIn RequestSignIn { get; set; }
        public ResponseSignIn ResponseSignIn { get; set; }

        public string JsonString { get; set; }
        public delegate void HttpRequestDelegate(WWW www);
        public virtual event HttpRequestDelegate OnHttpRequest;

        public void OnClick()
        {
            var name = this.gameObject.name;
            switch (name)
            {
                case "Back":
                    SceneManager.LoadScene(0);
                    break;
                case "Sign In":
                    SignIn();
                    break;
            }
        }

        public SignInManager()
        {
            RequestSignIn = new RequestSignIn();
            ResponseSignIn = new ResponseSignIn();
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

        public void SignIn()
        {
            RequestSignIn = new RequestSignIn();
            var validityChecker = new ValidityChecker();

            var id = GameObject.Find("IDInput").transform.FindChild("UILabel").GetComponent<UILabel>().text;
            var email = GameObject.Find("EmailInput").transform.FindChild("UILabel").GetComponent<UILabel>().text;
            var password = GameObject.Find("PasswordInput").transform.FindChild("UILabel").GetComponent<UILabel>().text;
            var errorMessage = GameObject.Find("Error").GetComponent<UILabel>();
            errorMessage.color = Color.yellow;

            try
            {
                validityChecker.IsValid(id, email, password);
            }
            catch (Exception exception)
            {
                errorMessage.text = exception.Message;
                return;
            }

            RequestSignIn.UserId = id;
            RequestSignIn.Password = password;
            RequestSignIn.Email = email;

            Debug.LogFormat("Sending {0} / {1} / {2} to server.", RequestSignIn.UserId, RequestSignIn.Email, RequestSignIn.Password);

            Post(Urls.UrlSignIn, RequestSignIn);
        }


        public WWW Post<T>(string url, T userObject)
        {
            //
            JsonString = JsonConvert.SerializeObject(userObject);
            var postData = System.Text.Encoding.UTF8.GetBytes(JsonString);
            var www = new WWW(url, postData, Urls.headers);

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
                var signInResponse = JsonConvert.DeserializeObject<ResponseSignIn>(www.text);
                var errorLabel = GameObject.Find("Error").GetComponent<UILabel>();
                errorLabel.color = Color.yellow;
                string errorMessage;
                if (signInResponse.ResultCode != ResultCode.SUCCESS)
                {
                    ErrorHandler.Instance.ReSultMap.TryGetValue(signInResponse.ResultCode, out errorMessage);
                    errorLabel.text = errorMessage;
                }
                else
                {
                    ErrorHandler.Instance.ReSultMap.TryGetValue(signInResponse.ResultCode, out errorMessage);
                    errorLabel.text = "회원가입 " + errorMessage + "\n로그인 후 사용하세요.";
                    //SceneManager.LoadScene(3);
                }
                Debug.Log("Return from server: " + www.text);
            }
            else
            {
                Debug.LogError("Error: " + www.error);
            }

            // 통신 해제
            www.Dispose();
        }
    }
}