using Excel;
using System;
using System.IO;
using System.Collections.Generic;

public class ReadXlsxFile
{
    string fileName;
    Dictionary<string, List<List<string>>> tablesDataList = new Dictionary<string, List<List<string>>>();

    /// <summary>
    /// 横向读取
    /// </summary>
    public Dictionary<string, List<List<string>>> RowTablesDataList
    {
        get
        {
            Dictionary<string, List<List<string>>> tempTablesDataList = new Dictionary<string, List<List<string>>>();
            foreach (var item in tablesDataList)
            {
                List<List<string>> tempDataList = new List<List<string>>();
                for (int i = 0; i < tablesDataList[item.Key].Count; i++)
                {
                    List<string> tempData = new List<string>();
                    for (int j = 0; j < tablesDataList[item.Key][i].Count; j++)
                    {
                        tempData.Add(tablesDataList[item.Key][i][j]);
                    }
                    tempDataList.Add(tempData);
                }

                tempTablesDataList.Add(item.Key, tempDataList);
            }

            return tempTablesDataList;
        }
    }

    /// <summary>
    /// 纵向读取
    /// </summary>
    public Dictionary<string, List<List<string>>> ColTablesDataList
    {
        get
        {
            Dictionary<string, List<List<string>>> tempTablesDataList = new Dictionary<string, List<List<string>>>();
            foreach (var item in tablesDataList)
            {
                if (tablesDataList[item.Key].Count == 0)
                {
                    continue;
                }

                List<List<string>> tempDataList = new List<List<string>>();
                for (int i = 0; i < tablesDataList[item.Key][0].Count; i++)
                {
                    List<string> tempData = new List<string>();
                    for (int j = 0; j < tablesDataList[item.Key].Count; j++)
                    {
                        tempData.Add(tablesDataList[item.Key][j][i]);
                    }
                    tempDataList.Add(tempData);
                }

                tempTablesDataList.Add(item.Key, tempDataList);
            }

            return tempTablesDataList;
        }
    }

    public bool IsFileOpened(string _filePath)
    {
        bool result = false;
        try
        {
            FileStream tempStream = File.Open(_filePath, FileMode.Open, FileAccess.Read);
            tempStream.Close();
        }
        catch (Exception e)
        {
            result = true;
        }
        return result;
    }

    public bool Load(string _path)
    {
        if (!File.Exists(_path))
        {
            return false;
        }

        fileName = Path.GetFileNameWithoutExtension(_path);
        tablesDataList.Add(fileName, new List<List<string>>());

        FileStream tempStream = File.Open(_path, FileMode.Open, FileAccess.Read);
        IExcelDataReader tempExcelReader = ExcelReaderFactory.CreateOpenXmlReader(tempStream);

        ReadTables(tempExcelReader);
        return true;
    }

    void ReadTables(IExcelDataReader _excelReader)
    {
        if (_excelReader == null)
        {
            return;
        }

        do
        {
            int tempReadLine = 0;
            int tempFieldCount = 0;

            string tempReaderName = _excelReader.Name;
            if (tempReaderName.Contains("#"))
            {
                continue;
            }

            if (!tablesDataList.ContainsKey(tempReaderName))
            {
                tablesDataList.Add(tempReaderName, new List<List<string>>());
            }

            List<List<string>> tempDataValue = new List<List<string>>();
            while (_excelReader.Read())
            {
                if (_excelReader.IsDBNull(0))
                {
                    break;
                }

                // 第一行的是参数名，这个是必须存在的值，长度以这个为标准
                if (tempReadLine == 0)
                {
                    for (int i = 0; i < _excelReader.FieldCount; i++)
                    {
                        if (_excelReader.IsDBNull(i))
                        {
                            continue;
                        }

                        string temp = _excelReader.GetString(i);

                        tempFieldCount++;
                    }

                    tempReadLine++;
                }

                List<string> tempValue = new List<string>();
                for (int i = 0; i < tempFieldCount; i++)
                {
                    string value = _excelReader.IsDBNull(i) ? "" : _excelReader.GetString(i);
                    tempValue.Add(value);
                }

                tempDataValue.Add(tempValue);
            }

            tablesDataList[tempReaderName] = tempDataValue;
        }
        while (_excelReader.NextResult());
    }
}