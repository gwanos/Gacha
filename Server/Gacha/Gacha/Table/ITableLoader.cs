using System;
using System.Collections.Generic;
using System.Data;

namespace Gacha.Table
{
    public interface ITableLoader
    {
        Dictionary<TKey, TRow> Load<TKey, TRow>(string filePath, string tableName, bool isFirstRowAsColumnName, Func<DataRow, TKey> getKey, Func<DataRow, TRow> getRow);

        DataSet GetXLSXFileData(string filePath, bool isFirstRowAsColumnName);
        //Dictionary<TKey, TRow> TableToDictionary<TKey, TRow>(DataTable table, Func<DataRow, TKey> getKey, Func<DataRow, TRow> getRow);
    }
}