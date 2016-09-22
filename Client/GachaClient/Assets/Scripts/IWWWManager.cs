using System.Collections;
using UnityEngine;
using Newtonsoft.Json;

public interface IWWWManager
{
    string JsonString { get; }
    WWW Post<T>(string url, T userObject);
    IEnumerator WaitForRequest(WWW www);
}
