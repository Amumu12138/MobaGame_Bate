using System.Collections.Generic;

public class ModifyFileLibrary
{
    Dictionary<string, ModifyFileInfo> modifyFileInfoList = new Dictionary<string, ModifyFileInfo>();

    public void AddModifyFileList(string _fileTxt)
    {
        string[] tempArray = _fileTxt.Split('\n');
        if (tempArray == null)
        {
            return;
        }

        modifyFileInfoList.Clear();
        for (int i = 0; i < tempArray.Length; i++)
        {
            AddModifyFile(tempArray[i]);
        }
    }

    public void AddModifyFile(string _fileData)
    {
        if (string.IsNullOrEmpty(_fileData))
        {
            return;
        }

        ModifyFileInfo tempModifyFileInfo = new ModifyFileInfo(_fileData);
        modifyFileInfoList.Add(tempModifyFileInfo.FilePath, tempModifyFileInfo);
    }

    public ModifyFileInfo GetTargetModifyFile(string _modifyFilePath)
    {
        if (!modifyFileInfoList.ContainsKey(_modifyFilePath))
        {
            return null;
        }

        return modifyFileInfoList[_modifyFilePath].Clone();
    }

    public ModifyFileInfo[] GetModifyFileArray()
    {
        int tempIndex = 0;
        ModifyFileInfo[] tempArray = new ModifyFileInfo[modifyFileInfoList.Count];
        foreach (var item in modifyFileInfoList)
        {
            tempArray[tempIndex] = item.Value.Clone();
            tempIndex = tempIndex + 1;
        }

        return tempArray;
    }
}