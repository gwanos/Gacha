using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Excel;

public enum HeroProperty
{
    Fire = 1, Water = 2, Wind = 3
}

public class HeroData : MonoBehaviour
{
    
    //=== Field ===//
    private static readonly string _fileName = "C:/Dev/Unity/GachaClient/Assets/HeroTable.csv";
    //private static string _fileName = Application.dataPath + "/" + "HeroTable.csv";
    private static readonly string _encodingType = "euc-kr";

    private static HeroData _instance;
    private Dictionary<int, HeroInfo> _table = new Dictionary<int, HeroInfo>();

    public GameObject HeroCard;
    public GameObject[] CardBackImages;
    public Vector3[] spawanPosition = new Vector3[]
    {
        new Vector3(-400, 100, 0),
        new Vector3(-200, 100, 0),
        new Vector3(0, 100, 0),
        new Vector3(200, 100, 0),
        new Vector3(400, 100, 0),
    };

    public static HeroData Instance
    {
        get
        {
            if (null == _instance)
            {
                if (null == _instance)
                {
                    _instance = FindObjectOfType(typeof(HeroData)) as HeroData;
                }
                if (null == _instance)
                {
                    GameObject gameObject = new GameObject("HeroData");
                    _instance = gameObject.AddComponent(typeof(HeroData)) as HeroData;
                    
                }
            }
            return _instance;
        }
    }
    public Dictionary<int, HeroInfo> Table { get { return _table; } }


    //=== Method ===//  
    // Constructor
    private HeroData()
    {
        // Load
        _table = HeroTableLoader.Instance.Load(_fileName, _encodingType);
        //if( !IsValid() )
          //  throw new Exception("Invalid values in .csv");
    }

    // Main
    void Start()
    {
        var heroTable = HeroData.Instance.Table;
    }

    // Validate datas
    public bool IsValid()
    {
        foreach (var row in Table)
        {
            if (row.Key != row.Value.Id) return false;
            if (Table.Count < row.Value.Id || row.Value.Id < 0 ) return false;
            if (!(1 <= row.Value.Grade || row.Value.Grade <= 6)) return false;
            if (!(1 <= row.Value.Property || row.Value.Property <= 3)) return false;
        }
        return true;
    }
}
