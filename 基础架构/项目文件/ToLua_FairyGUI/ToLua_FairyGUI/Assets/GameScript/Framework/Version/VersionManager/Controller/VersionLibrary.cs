using System.Collections.Generic;

public class VersionLibrary
{
    int versionNum = 0;
    List<VersionInfo> verInfoList = new List<VersionInfo>();

    public void SetVersionNumber(int _versionNum)
    {
        versionNum = _versionNum;
    }

    public void AddVersionInfo(VersionInfo _info)
    {
        if (_info.VersionNum <= versionNum)
        {
            return;
        }

        verInfoList.Add(_info);
    }

    /// <summary>
    /// 根据版本号大小排序
    /// 每个文件都要按照版本号来进行下载，保证顺序正常
    /// </summary>
    public void SortVersionInfo()
    {
        verInfoList.Sort((VersionInfo _info1, VersionInfo _info2) => { return _info1.VersionNum < _info2.VersionNum ? -1 : 1; });
    }

    /// <summary>
    /// 需要下载的文件大小
    /// </summary>
    public int GetFileSize()
    {
        int tempSize = 0;
        for (int i = 0; i < verInfoList.Count; i++)
        {
            tempSize += verInfoList[i].Size;
        }

        return tempSize;
    }

    public bool IsEmptyArray()
    {
        return verInfoList.Count == 0;
    }

    public VersionInfo[] GetVersionFileArray()
    {
        return verInfoList.ToArray();
    }
}