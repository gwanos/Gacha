using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Excel;
using NLog;

namespace Gacha.Table
{
    public class TableLoader : ITableLoader
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        // Load
        public Dictionary<TKey, TRow> Load<TKey, TRow>(string filePath, string tableName, bool isFirstRowAsColumnName, 
            Func<DataRow, TKey> getKey, Func<DataRow, TRow> getRow)
        {
            // Load
            DataSet dataSet = GetXLSXFileData(filePath, isFirstRowAsColumnName);
            DataTable dataTable = dataSet.Tables[tableName];

            // Parse
            var ret = TableToDictionary(dataTable, getKey, getRow);
            return ret;
        }

        // Read excel file
        public DataSet GetXLSXFileData(string filePath, bool isFirstRowAsColumnName)
        {
            try
            {
                using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    var excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
                    excelDataReader.IsFirstRowAsColumnNames = isFirstRowAsColumnName;
                    var set = excelDataReader.AsDataSet();

                    return set;
                }
            }
            catch (Exception e)
            {
                _logger.Debug(e);
                return null;
            }
        }

        // DataTable to dictionary
        protected Dictionary<TKey, TRow> TableToDictionary<TKey, TRow>(DataTable table, Func<DataRow, TKey> getKey,
            Func<DataRow, TRow> getRow)
        {
            return table.Rows.OfType<DataRow>().ToDictionary(getKey, getRow);
        }
    }
}