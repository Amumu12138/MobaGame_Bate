using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 管理版本信息
/// 解析需要加载的文件
/// </summary>
public class VersionManager : MonoBehaviour
{
    public static VersionManager mInst;

    bool isUpdate;
    string networkPath;
    VersionLibrary versionLib = new VersionLibrary();

    /// <summary>
    /// 是否使用热更文件
    /// </summary>
    public bool IsUpdate { get { return isUpdate; } }

    void Awake()
    {
        mInst = this;
    }

    /// <summary>
    /// 检查有没有资源需要更新
    /// </summary>
    public void CheckUpdateResource()
    {
        StartCoroutine(LoadAllFile());
    }

    /// <summary>
    /// 检查资源是否可以更新
    /// </summary>
    /// <param name="_isUpdate">0为不更新，1为更新</param>
    void CheckUpdateAssets(string _isUpdate)
    {
        isUpdate = _isUpdate != "0";
        if (isUpdate)
        {
            // 版本需要做热更，请求资源服务器地址
            string tempNetworkPath = VersionConst.ToServerPath(VersionConst.AssetsUrl);
            string tempPlatformNumber = GetFileNumber_String(VersionConst.OutPlatformNumberFile);
            StartCoroutine(LoadWWW(tempNetworkPath, "platformAssets", tempPlatformNumber, OnServerPath_Callback));
            return;
        }

        // 版本不需要做热更，直接返回完成事件
        Events.msInst.DispatchEvent(VersionEvent.DOWNLOAD_COMPLETION, null);
    }

    /// <summary>
    /// 请求资源服务器地址
    /// </summary>
    /// <param name="_networkPath">资源服务器地址</param>
    void OnServerPath_Callback(string _networkPath)
    {
        networkPath = _networkPath;
        StartCoroutine(CheckVersionAssets());
    }

    /// <summary>
    /// 调用PHP返回全部的服务器目录文件夹
    /// </summary>
    void OnVersionFileText_Callback(string _versionFileText)
    {
        List<int> tempVersionNumberList = new List<int>();
        string[] temp_fileTexts = _versionFileText.Split('#');
        for (int i = 0; i < temp_fileTexts.Length; i++)
        {
            string tempAssetsName = temp_fileTexts[i].Trim();

            // 排除空文件
            if (string.IsNullOrEmpty(tempAssetsName))
            {
                continue;
            }

            // 排除 PHP 文件
            if (tempAssetsName.Contains("php"))
            {
                continue;
            }

            // 文件命名格式为：版本号_文件大小_创建文件日期
            string[] tempFile = tempAssetsName.Split('_');
            if (tempFile.Length < 2)
            {
                continue;
            }

            // 目前只用到了版本号和文件大小，这里的版本号会和本地的版本号做比对，小于等于本地版本号是不会记录的
            versionLib.AddVersionInfo(new VersionInfo(tempFile[1], tempFile[0], NetworkPath + tempAssetsName + "?version=" + "&v=" + Random.Range(0f, 99999999f), tempAssetsName));
        }

        // 这里控制了所有的版本都只能按照顺序下载，这里把版本号从小到大排序一下
        versionLib.SortVersionInfo();

        // 当前版本不是最新的，发送版本号列表
        Events.msInst.DispatchEvent(VersionEvent.VERSION_UPDATE, versionLib);
    }

    /// <summary>
    /// 加载必要文件到本地目录
    /// </summary>
    IEnumerator LoadAllFile()
    {
        if (!File.Exists(VersionConst.OutPlatformNumberFile))
        {
            yield return LoadFile(VersionConst.InPlatformNumberFile, VersionConst.OutPlatformNumberFile);
        }

        if (!File.Exists(VersionConst.OutVersionNumberFile))
        {
            yield return LoadFile(VersionConst.InVersionNumberFile, VersionConst.OutVersionNumberFile);
        }

        string tempNetworkPath = VersionConst.ToServerPath(VersionConst.UpdateAssets);
        string tempPlatformNumber = GetFileNumber_String(VersionConst.OutPlatformNumberFile);
        StartCoroutine(LoadWWW(tempNetworkPath, "platformNum", tempPlatformNumber, CheckUpdateAssets));
    }

    /// <summary>
    /// 检查版本资源，用于替换原资源
    /// </summary>
    IEnumerator CheckVersionAssets()
    {
        // 获取资源版本号，资源版本号不相同的话，那么就会清除缓存，重新解压资源
        int tempOldPackageNum = GetFileNumber(VersionConst.OutPackageNumberFile);
        WWW www = new WWW(VersionConst.InPackageNumberFile);
        yield return www;

        if (www.isDone)
        {
            int tempPackageNum = 0;
            int.TryParse(www.text.Trim(), out tempPackageNum);
            if (tempPackageNum != tempOldPackageNum)
            {
                // 版本号不同，则标示安装包是覆盖安装，需要清除数据
                if (Directory.Exists(VersionConst.InUnpackDirectory))
                {
                    // 资源版本号不同，清除缓存
                    Directory.Delete(VersionConst.InUnpackDirectory, true);
                }

                if (File.Exists(VersionConst.UnpackMarkPath))
                {
                    // 资源版本号不同，清除标示文件，标示文件用来判断文件是否需要解压文件
                    File.Delete(VersionConst.UnpackMarkPath);
                }

                // 重新写入新的资源版本号
                File.WriteAllBytes(VersionConst.OutPackageNumberFile, www.bytes);
            }
        }

        StartCoroutine(LoadVersionAssets());
        yield return 0;
    }

    /// <summary>
    /// 解压初始文件
    /// </summary>
    IEnumerator LoadVersionAssets()
    {
        if (!File.Exists(VersionConst.UnpackMarkPath))
        {
            Directory.CreateDirectory(VersionConst.InUnpackDirectory);
#if UNITY_IPHONE
            yield return LoadDirectory(VersionConst.AppContentPath(), VersionConst.InUnpackDirectory + "/");
#else
            // UPK文件拷贝到相对路径
            yield return LoadFile(VersionConst.InUnpackFile, VersionConst.OutUnpackFile);
            // 解压相对路径的UPK文件
            yield return UtilityUnpack.UnpackFolder(VersionConst.OutUnpackFile, VersionConst.InUnpackDirectory + "/");
#endif
        }

        // 解压文件成功，写入标示文件
        // 防止UPK文件解压到一半，用户就退出游戏，导致文件解压不完整出现异常
        File.WriteAllText(VersionConst.UnpackMarkPath, "2000");
        
        // 资源解压完毕，开始加载本地的版本号
        versionLib.SetVersionNumber(GetVersionNumber());
        StartCoroutine(LoadWWW(NetworkPath + VersionConst.VersionFile + "?version=" + "&v=" + Random.Range(0f, 99999999f), OnVersionFileText_Callback));
    }

    IEnumerator LoadWWW(string _url, System.Action<string> _callbackFun = null)
    {
        WWW www = new WWW(_url);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError("路径无法加载 = " + _url);
            _callbackFun("");
            yield break;
        }

        _callbackFun(www.text.Trim());
    }

    IEnumerator LoadWWW(string _url, string _fieldName, string _fieldValue, System.Action<string> _callbackFun = null)
    {
        WWWForm tempForm = new WWWForm();
        tempForm.AddField(_fieldName, _fieldValue);

        WWW www = new WWW(_url, tempForm);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError("路径无法加载 = " + _url);
            _callbackFun("");
            yield break;
        }

        _callbackFun(www.text.Trim());
    }

    IEnumerator LoadFile(string _inFile, string _outFile)
    {
        if (Application.platform != RuntimePlatform.WindowsEditor)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                WWW www = new WWW(_inFile);
                yield return www;

                if (www.isDone)
                {
                    File.WriteAllBytes(_outFile, www.bytes);
                }
                yield return 0;
            }
            else
            {
                if (File.Exists(_outFile))
                {
                    File.Delete(_outFile);
                }
                File.Copy(_inFile, _outFile, true);
            }
        }
    }

    // 把 StreamingAssets 路径文件拷贝到相对路径，IOS专用
    IEnumerator LoadDirectory(string _inFile, string _outFile)
    {
        int tempFileCount = 0;
        List<FileInfo> temp = GetDirectoryAllFile(_inFile);
        for (int i = 0; i < temp.Count; i++)
        {
            string tempInFile = temp[i].FullName.Replace(@"\", "/");
            string tempOutFile = tempInFile.Replace(_inFile, _outFile);
            string tempOutDirectory = tempOutFile.Replace(temp[i].Name, "");
            if (!Directory.Exists(tempOutDirectory))
            {
                Directory.CreateDirectory(tempOutDirectory);
            }

            if (!File.Exists(tempOutFile))
            {
                tempFileCount++;
                if (tempFileCount >= 30)
                {
                    tempFileCount = 0;
                    yield return null;
                }

                File.Copy(tempInFile, tempOutFile);
                Events.msInst.DispatchEvent(VersionEvent.UNPACC_PROGRESS, ((float)i / (float)temp.Count));
            }
        }
        Events.msInst.DispatchEvent(VersionEvent.UNPACC_COMPLETION, null);
    }

    /// <summary>
    /// 获取文件夹下的全部文件
    /// </summary>
    /// <param name="_directoryPath"></param>
    /// <param name="_searchPattern"></param>
    List<FileInfo> GetDirectoryAllFile(string _directoryPath, string _searchPattern = "")
    {
        List<FileInfo> fileList = new List<FileInfo>();
        SearchAllFile(_directoryPath, _searchPattern, fileList);
        return fileList;
    }

    void SearchAllFile(string _directoryPath, string _searchPattern, List<FileInfo> _fileInfoList)
    {
        DirectoryInfo folder = new DirectoryInfo(_directoryPath);
        DirectoryInfo[] directoryInfoList = folder.GetDirectories();
        foreach (DirectoryInfo directory in directoryInfoList)
        {
            SearchAllFile(directory.FullName, _searchPattern, _fileInfoList);
        }

        FileInfo[] fileList = null;
        if (string.IsNullOrEmpty(_searchPattern))
        {
            fileList = folder.GetFiles();
        }
        else
        {
            fileList = folder.GetFiles("*." + _searchPattern);
        }

        foreach (FileInfo file in fileList)
        {
            if (!file.Name.Contains(".meta") && !file.Name.Contains(".manifest"))
            {
                _fileInfoList.Add(file);
            }
        }
    }

    /// <summary>
    /// 根据目标路径文件，获取对应的资源更新平台
    /// </summary>
    string GetFileNumber_String(string _filePath)
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            // Windows编辑器下，默认平台为：9999
            return "9999";
        }

        string tempNumber = "";
        if (File.Exists(_filePath))
        {
            return File.ReadAllText(_filePath).Trim();
        }

        return tempNumber;
    }

    int GetFileNumber(string _filePath)
    {
        int tempVersionNum = 0;
        if (File.Exists(_filePath))
        {
            string tempNumber = File.ReadAllText(_filePath);
            int.TryParse(tempNumber.Trim(), out tempVersionNum);
        }

        return tempVersionNum;
    }

    string NetworkPath { get { return networkPath + "/"; } }
    int GetVersionNumber() { return GetFileNumber(VersionConst.OutVersionNumberFile); }
}