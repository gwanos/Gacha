using System;
using UnityEngine;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using System.IO;

public enum ResultCode
{
    // SYSTEM
    SUCCESS = 0, // 성공

    // NETWORK
    LCN0001 = 101,  // JSon Format error

    // ACCOUNT
    LCA0001 = 1001, // 인증 필요
    LCA0002 = 1002, // 동일한 ID 존재
    LCA0003 = 1003, // DB에 존재하지 않음
    LCA0004 = 1004, // Password 불일치

    // CONTENTS
    LCGACHA0001 = 2001, // 없는 소환서 선택(GachaId 범위 초과)
    LCGACHA0002 = 2002, // 영웅 5명 못 뽓음
}

public class ErrorHandler : MonoBehaviour
{
    private class Result
    {
        public ResultCode ResultCode { get; set; }
        public string ResultText { get; set; }
    }
    private static readonly string filePath = "C:/Dev/Unity/GachaClient/Assets/Resources/ResultCodeMap.dat";
    public Dictionary<ResultCode, string> ReSultMap;
    private static ErrorHandler _instance;
    public static ErrorHandler Instance
    {
        get
        {
            if (null == _instance)
            {
                if (null == _instance)
                {
                    _instance = FindObjectOfType(typeof(ErrorHandler)) as ErrorHandler;
                }
                if (null == _instance)
                {
                    GameObject gameObject = new GameObject("ErrorHandler");
                    _instance = gameObject.AddComponent(typeof(ErrorHandler)) as ErrorHandler;

                }
            }
            return _instance;
        }
    }
    public void LoadData()
    {
        var text = File.ReadAllText(filePath);

        var json = JsonConvert.DeserializeObject<List<Result>>(text);
        foreach (var v in json)
        {
            ReSultMap.Add(v.ResultCode, v.ResultText);
        }
    }
    public ErrorHandler()
    {
        ReSultMap = new Dictionary<ResultCode, string>();
        LoadData();
    }
}