using System.Collections.Generic;

// 单个参数列表
class LuaParamInfo
{
    const string NewLine = "\r\t\t";
    List<string> paramList = new List<string>();

    public LuaParamInfo()
    {
        // 直接使用 Proto 作为配置
        paramList.Add("{" + NewLine);
    }

    public LuaParamInfo(string _paramId)
    {
        // 直接使用 Lua 作为配置
        paramList.Add("[" + _paramId + "] = {" + NewLine);
    }

    public void AddParam(string _paramName, string _paramType, string _paramValue)
    {
        if (_paramType == "string")
        {
            paramList.Add(GetStringParam(_paramName, _paramValue));
        }
        else if (_paramType == "strings")
        {
            paramList.Add(GetStringArrayParam(_paramName, _paramValue));
        }
        else if (_paramType == "ints" || _paramType == "floats")
        {
            paramList.Add(GetNumberArrayParam(_paramName, _paramValue));
        }
        else
        {
            paramList.Add(GetNumberParam(_paramName, _paramValue));
        }
    }

    string GetStringParam(string _paramName, string _paramValue) { return _paramName + " = " + ('"') + _paramValue + ('"') + ";" + NewLine; }
    string GetNumberArrayParam(string _paramName, string _paramValue) { return _paramName + " = " + ("{ ") + _paramValue + " };" + NewLine; }

    string GetNumberParam(string _paramName, string _paramValue)
    {
        string tempParamValue = _paramValue;
        if (string.IsNullOrEmpty(_paramValue))
        {
            tempParamValue = "0";
        }

        return _paramName + " = " + tempParamValue + ";" + NewLine;
    }

    string GetStringArrayParam(string _paramName, string _paramValue)
    {
        string[] tempParam = _paramValue.Split(',');
        if (tempParam == null || tempParam.Length == 0)
        {
            return "";
        }

        string tempValue = ('"') + tempParam[0] + ('"');
        for (int i = 1; i < tempParam.Length; i++)
        {
            tempValue += ("," + ('"') + tempParam[i] + ('"'));
        }

        return _paramName + " = { " + tempValue + " }" + NewLine;
    }

    public string ParamValue
    {
        get
        {
            string tempParamValue = "";
            for (int i = 0; i < paramList.Count; i++)
            {
                tempParamValue += (paramList[i]);
            }

            return tempParamValue + "};";
        }
    }
}

// 全部参数列表
class LuaParamLibrary
{
    string configName = "";
    const string NewLine = "\r\t";
    List<LuaParamInfo> paramInfo = new List<LuaParamInfo>();

    public LuaParamLibrary(string _configName)
    {
        configName = _configName + " = {";
    }

    public void AddParamInfo(LuaParamInfo _paramInfo)
    {
        paramInfo.Add(_paramInfo);
    }

    public string ParamValue
    {
        get
        {
            string tempConfig = configName + NewLine;
            for (int i = 0; i < paramInfo.Count; i++)
            {
                tempConfig += paramInfo[i].ParamValue + NewLine;
            }

            return tempConfig + "};";
        }
    }
}

public class LuaScript : BaseScript
{
    bool isShowIndex = true;

    public LuaScript(bool _isShowIndex)
    {
        isShowIndex = _isShowIndex;
    }

    protected override void HorizontalTableList(string _dir, List<CreateFileGroupLibrary> _dataList)
    {
        LuaParamLibrary tempParamLib = new LuaParamLibrary(ClassName);

        for (int j = 0; j < _dataList.Count; j++)
        {
            // 获取单行数据
            List<CreateFileInfo> tempInfo = _dataList[j].FileInfoList;
            if (tempInfo.Count == 0)
            {
                continue;
            }

            LuaParamInfo tempParamInfo = null;
            if (isShowIndex)
            {
                tempParamInfo = new LuaParamInfo(tempInfo[0].ParamValue);
            }
            else
            {
                tempParamInfo = new LuaParamInfo();
            }

            for (int i = 0; i < tempInfo.Count; i++)
            {
                tempParamInfo.AddParam(tempInfo[i].ParamName, tempInfo[i].ParamType, tempInfo[i].ParamValue);
            }

            tempParamLib.AddParamInfo(tempParamInfo);
        }

        System.IO.File.WriteAllText(_dir + "Config.lua", tempParamLib.ParamValue);
    }
}