using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DownloadManager : MonoBehaviour
{
    public static DownloadManager mInst;

    int maxDownloadNum = 0;
    int maxDownloadSize = 0;
    int downloadProgress = 0;
    bool downloadEnd = false;
    Queue<VersionInfo> downloadFileList = new Queue<VersionInfo>();
    BreakpointDownload breakpointDownload = new BreakpointDownload();

    public bool DownloadEnd { get { return downloadEnd; } }

    void Awake()
    {
        mInst = this;
    }

    void Start()
    {
        // 注册版本更新事件，接收版本更新事件
        Events.msInst.AddEventListener(VersionEvent.VERSION_UPDATE, new GameEventCallBack(OnVersionFile_Callback));
    }

    void OnDisable()
    {
        if (breakpointDownload == null)
        {
            return;
        }

        breakpointDownload.Close();
    }

    /// <summary>
    /// 启动开始下载资源
    /// </summary>
    public void DownloadAssets()
    {
        if (downloadFileList == null || downloadFileList.Count == 0)
        {
            Events.msInst.DispatchEvent(VersionEvent.DOWNLOAD_COMPLETION, null);
            return;
        }

        VersionInfo tempInfo = downloadFileList.Dequeue();
        string tempLocalPath = Application.persistentDataPath + "/" + tempInfo.VersionName;
        string tempNetworkPath = tempInfo.VersionPath;

        if (breakpointDownload.CheckDownload(tempLocalPath, tempNetworkPath))
        {
            StartCoroutine(YieldDownload(tempLocalPath, tempInfo.VersionNum.ToString()));
        }
    }

    void OnVersionFile_Callback(object _obj)
    {
        VersionLibrary tempVersionLib = (VersionLibrary)_obj;
        if (tempVersionLib == null || tempVersionLib.IsEmptyArray())
        {
            // 标示下载完毕
            downloadEnd = true;
            Events.msInst.DispatchEvent(VersionEvent.DOWNLOAD_COMPLETION, null);
            return;
        }

        VersionInfo[] tempVersionInfoArray = tempVersionLib.GetVersionFileArray();
        for (int i = 0; i < tempVersionInfoArray.Length; i++)
        {
            downloadFileList.Enqueue(tempVersionInfoArray[i]);
        }

        maxDownloadNum = downloadFileList.Count;
        maxDownloadSize = tempVersionLib.GetFileSize();
        Events.msInst.DispatchEvent(VersionEvent.DOWNLOAD_NUMBER, new int[] { downloadFileList.Count, maxDownloadNum });
        Events.msInst.DispatchEvent(VersionEvent.DOWNLOAD_SIZE, maxDownloadSize);
    }

    IEnumerator YieldDownload(string _localPath, string _versionNum)
    {
        Events.msInst.DispatchEvent(VersionEvent.DOWNLOAD_FILE_SIZE, breakpointDownload.GetMaxFileLength());

        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            float tempProgress = breakpointDownload.Progress;
            Events.msInst.DispatchEvent(VersionEvent.DOWNLOAD_UPDATE, tempProgress);

            if (tempProgress >= 1)
            {
                breakpointDownload.Close();
                // 从服务器下载的资源都是打包的文件，需要执行解压操作
                yield return UtilityUnpack.UnpackFolder(_localPath, Application.persistentDataPath + "/" + VersionConst.UnpackFolder + "/", true);

                System.IO.File.WriteAllText(LuaFramework.Util.BasePath + VersionConst.VersionNumberFile, _versionNum);
                Events.msInst.DispatchEvent(VersionEvent.DOWNLOAD_NUMBER, new int[] { downloadFileList.Count, maxDownloadNum });
                break;
            }
        }

        yield return new WaitForSeconds(0.5f);
        System.IO.File.WriteAllText(VersionConst.OutVersionNumberFile, _versionNum);
        DownloadAssets();
    }
}