using System.IO;
using System.Collections.Generic;

public class BaseScript
{
    string className;

    public void Generate(string _writePath, string _fileName, Dictionary<string, List<CreateFileGroupLibrary>> _paramList)
    {
        foreach (var item in _paramList)
        {
            className = _fileName + item.Key + "Config";
            string tempDir = _writePath + "/" + _fileName + item.Key;

            HorizontalTableList(tempDir, item.Value);
        }
    }

    public Dictionary<string, string> LoadParam(string _type)
    {
        string tempPath = Directory.GetCurrentDirectory() + "/Config/" + _type + "Param.txt";
        if (!File.Exists(tempPath))
        {
            UnityEngine.Debug.LogError(tempPath + " 文件不存在，无法加载");
            return null;
        }

        return GetValue(File.ReadAllLines(tempPath));
    }

    public Dictionary<string, string> LoadParamValue(string _type)
    {
        string tempPath = Directory.GetCurrentDirectory() + "/Config/" + _type + "ParamValue.txt";
        if (!File.Exists(tempPath))
        {
            UnityEngine.Debug.LogError(tempPath + " 文件不存在，无法加载");
            return null;
        }

        return GetValue(File.ReadAllLines(tempPath));
    }

    public string LoadReadText(string _type)
    {
        string tempPath = Directory.GetCurrentDirectory() + "/Config/" + _type + "StreamingAssets.txt";
        if (!File.Exists(tempPath))
        {
            UnityEngine.Debug.LogError(tempPath + " 文件不存在，无法加载");
            return "";
        }

        return File.ReadAllText(tempPath);
    }

    Dictionary<string, string> GetValue(string[] _lines)
    {
        Dictionary<string, string> tempValueList = new Dictionary<string, string>();

        if (_lines == null || _lines.Length == 0)
        {
            UnityEngine.Debug.LogError("tempList value or null");
            return tempValueList;
        }

        for (int i = 0; i < _lines.Length; i++)
        {
            string[] tempSplit = _lines[i].Split('#');
            if (tempSplit == null || tempSplit.Length < 2)
            {
                continue;
            }

            tempValueList.Add(tempSplit[0], tempSplit[1]);
        }

        return tempValueList;
    }

    protected string ClassName { get { return className; } }
    protected virtual void HorizontalTableList(string _dir, List<CreateFileGroupLibrary> _dataList) { }
}