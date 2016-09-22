using System;
using UnityEngine;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Excel;
using NUnit.Framework;

public class HeroTableLoader : ITableLoader
{
    //=== Field ===//
    private static readonly string _tableName = "Hero_Table";
    private static readonly string _columnID = "ID";
    private static readonly string _columnName = "NAME";
    private static readonly string _columnGrade = "GRADE";
    private static readonly string _columnProperty = "PROPERTY";

    private static HeroTableLoader _instance;
    public static HeroTableLoader Instance
    {
        get
        {
            if (_instance == null)
            {
                if (_instance == null)
                {
                    _instance = new HeroTableLoader();
                }
            }
            return _instance;
        }
    }

    private HeroTableLoader()
    {
    }

    // Load
    public Dictionary<int, HeroInfo> Load(string filePath, string encodingType)
    {
        var lines = ReadCSVFile(filePath, encodingType);
        var ret = CsvToDictionary(lines);
        return ret;
    }

    // Read .csv file
    public List<string[]> ReadCSVFile(string filePath, string encodingType)
    {
        // Read file
        //var filePath = Application.dataPath + "/" + filePath;
        var lines = new List<string[]>();

        //try
        //{
        var reader = new StreamReader(filePath, Encoding.GetEncoding(encodingType));
        while (!reader.EndOfStream)
        {
            var s = reader.ReadLine().Split(',');
            lines.Add(s);
        }
        return lines;
        //}
        //catch (Exception exception)
        //{
        //    throw exception;
        //}
    }

    // Convert CSV file to dictionary
    public Dictionary<int, HeroInfo> CsvToDictionary(List<string[]> lines)
    {
        // Verify key values
        var keyValueLine = lines[0];
        if (_columnID != keyValueLine[0] || _columnName != keyValueLine[1] || _columnGrade != keyValueLine[2] || _columnProperty != keyValueLine[3])
            throw new Exception("Column name mismatched.");

        // Convert
        var dictionary = new Dictionary<int, HeroInfo>();
        foreach (var line in lines.Skip(1)) // Skip first line.
        {
            dictionary.Add(Convert.ToInt32(line[0]),
                new HeroInfo()
                {
                    Id = Convert.ToInt32(line[0]),
                    Name = line[1],
                    Grade = Convert.ToInt32(line[2]),
                    Property = Convert.ToInt32(line[3]),
                });
        }
        return dictionary;
    }
}
