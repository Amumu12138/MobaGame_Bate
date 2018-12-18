using System.IO;
using System.Collections.Generic;

/**
 * 读取全部Xlsx配置文件并解析保存
 */

// 单个参数值
public class CreateFileInfo
{
    string paramName = "";              // 参数名字，必须存在，不能为空
    string paramExplain = "";           // 参数说明，参数在代码做注释时使用
    string paramType = "";              // 参数类型，必须存在，不能为空
    string paramState = "";             // 参数状态，0 ： 单纯显示，不会对其操作  1 ：后端使用  2 ：前端使用  3 ： 前后端通用 
    string paramValue = "";             // 参数最终的值
    string originalParamType = "";      // 原始的参数类型，当参数是数组的时候会使用到

    public CreateFileInfo(string _paramName, string _paramExplain, string _paramType, string _paramState, string _paramValue)
    {
        originalParamType = _paramType;

        paramName = _paramName;
        paramExplain = _paramExplain;
        paramType = _paramType;
        paramState = _paramState;
        paramValue = _paramValue;
    }

    public void ParamTypeToArray()
    {
        if (paramType == (originalParamType + "s"))
        {
            return;
        }

        paramType = originalParamType + "s";
    }

    public void AddGroupValue(string _value)
    {
        paramValue += ("," + _value);
    }

    public string ParamName { get { return paramName; } }
    public string ParamType { get { return paramType; } }
    public string ParamState { get { return paramState; } }
    public string ParamValue { get { return paramValue; } }
    public string ParamExplain { get { return paramExplain; } }
}

// 整列参数列表
public class CreateFileGroupLibrary
{
    List<CreateFileInfo> groupList = new List<CreateFileInfo>();

    public void AddFileInfo(CreateFileInfo _info)
    {
        for (int i = 0; i < groupList.Count; i++)
        {
            if (groupList[i].ParamName == _info.ParamName)
            {
                // 当参数的名字相同时，设置为数组
                groupList[i].ParamTypeToArray();
                // 添加值，会自动添加分隔符
                groupList[i].AddGroupValue(_info.ParamValue);
                return;
            }
        }

        groupList.Add(_info);
    }

    public List<CreateFileInfo> FileInfoList { get { return groupList; } }
}

// 整个Xlsx参数列表
public class CreateLibrary
{
    string fileName = "";
    Dictionary<string, List<CreateFileGroupLibrary>> clientList = new Dictionary<string, List<CreateFileGroupLibrary>>();
    Dictionary<string, List<CreateFileGroupLibrary>> serverList = new Dictionary<string, List<CreateFileGroupLibrary>>();

    public CreateLibrary(string _xlsxFilePath)
    {
        ReadXlsxFile readXlsx = new ReadXlsxFile();
        if (string.IsNullOrEmpty(_xlsxFilePath))
        {
            UnityEngine.Debug.LogError("Xlsx 文件路径为空，无法加载");
            return;
        }

        if (readXlsx.IsFileOpened(_xlsxFilePath))
        {
            UnityEngine.Debug.LogError("Xlsx 文件处在打开状态，请先关闭该文件");
            return;
        }

        if (!readXlsx.Load(_xlsxFilePath))
        {
            UnityEngine.Debug.LogError("Xlsx 文件读取失败，请检查文件是否存在");
            return;
        }

        fileName = Path.GetFileNameWithoutExtension(_xlsxFilePath);

        foreach (var item in readXlsx.ColTablesDataList)
        {
            int tempRow = readXlsx.ColTablesDataList[item.Key].Count;       // 横向
            int tempCol = readXlsx.ColTablesDataList[item.Key][0].Count;    // 纵向

            clientList.Add(item.Key, new List<CreateFileGroupLibrary>());
            serverList.Add(item.Key, new List<CreateFileGroupLibrary>());

            List<List<string>> tempRowDataList = readXlsx.ColTablesDataList[item.Key];
            for (int i = 4; i < tempCol; i++)
            {
                CreateFileGroupLibrary tempClientGroupLib = new CreateFileGroupLibrary();
                CreateFileGroupLibrary tempServerGroupLib = new CreateFileGroupLibrary();

                for (int j = 0; j < tempRow; j++)
                {
                    string tempParamName = tempRowDataList[j][0];
                    string tempParamExplain = tempRowDataList[j][1];
                    string tempParamType = tempRowDataList[j][2];
                    string tempParamState = tempRowDataList[j][3];
                    string tempParamValue = tempRowDataList[j][i];

                    if (tempParamState == "0")
                    {
                        continue;
                    }

                    CreateFileInfo tempInfo = new CreateFileInfo(tempParamName, tempParamExplain, tempParamType, tempParamState, tempParamValue);

                    if (tempParamState == "1")         // 服务器专用
                    {
                        tempServerGroupLib.AddFileInfo(tempInfo);
                    }
                    else if (tempParamState == "2")    // 客户端专用
                    {
                        tempClientGroupLib.AddFileInfo(tempInfo);
                    }
                    else if (tempParamState == "3")    // 服务器和客户端通用
                    {
                        tempServerGroupLib.AddFileInfo(tempInfo);
                        tempClientGroupLib.AddFileInfo(tempInfo);
                    }
                }

                clientList[item.Key].Add(tempClientGroupLib);
                serverList[item.Key].Add(tempServerGroupLib);
            }
        }
    }

    /// <summary>
    /// Xlsx 文件名字
    /// </summary>
    public string FileName { get { return fileName; } }
    public Dictionary<string, List<CreateFileGroupLibrary>> ClientList { get { return clientList; } }
    public Dictionary<string, List<CreateFileGroupLibrary>> ServerList { get { return serverList; } }
}