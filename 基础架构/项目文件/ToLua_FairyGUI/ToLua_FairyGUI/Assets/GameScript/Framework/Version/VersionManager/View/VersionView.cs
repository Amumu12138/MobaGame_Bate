using UnityEngine;
using System.Collections;

public class VersionView : MonoBehaviour
{
    void Start()
    {
        Events.msInst.AddEventListener(VersionEvent.UNPACC_SIZE, new GameEventCallBack(OnUnpackFileSize_Callback), this);
        Events.msInst.AddEventListener(VersionEvent.UNPACC_PROGRESS, new GameEventCallBack(OnUnpackProgress_Callback), this);
        Events.msInst.AddEventListener(VersionEvent.UNPACC_COMPLETION, new GameEventCallBack(OnUnpackCompletion_Callback), this);

        Events.msInst.AddEventListener(VersionEvent.DOWNLOAD_SIZE, new GameEventCallBack(OnDownloadMaxSize_Callback), this);
        Events.msInst.AddEventListener(VersionEvent.DOWNLOAD_UPDATE, new GameEventCallBack(OnDownloadUpdate_Callback), this);
        Events.msInst.AddEventListener(VersionEvent.DOWNLOAD_NUMBER, new GameEventCallBack(OnDownloadNumber_Callback), this);
        Events.msInst.AddEventListener(VersionEvent.DOWNLOAD_COMPLETION, new GameEventCallBack(OnDownloadCompletion_Callback), this);

        //EnterView("Loading");
        //SetFairyChildText(mainView, "n3", "");
        //SetFairyChildClick(GetFairyChild(mainView, "n4"), "n8", new FairyGUI.EventCallback0(OnClick_SureUpdate));
        //SetFairyChildClick(GetFairyChild(mainView, "n4"), "n7", new FairyGUI.EventCallback0(OnClick_CancelUpdate));

        VersionManager.mInst.CheckUpdateResource();
    }

    void OnDisable()
    {
        Events.msInst.RemoveEventListener(this);
    }

    IEnumerator EnterGame()
    {
        yield return new WaitForSeconds(0.2f);

        //ExitView();                               // 退出加载页
        LuaFramework.LuaManager.mInst.InitStart();  // 启动游戏
    }

    void OnClick_SureUpdate()
    {
        DownloadManager.mInst.DownloadAssets();
        //SetFairyChildVisible(mainView, "n4", false);
    }

    void OnClick_CancelUpdate()
    {
        Application.Quit();
    }

    // 版本为最新的版本
    void OnDownloadCompletion_Callback(object _obj)
    {
        StartCoroutine(EnterGame());
        //SetFairyChildProgressValue(mainView, "n2", 100);
    }

    // 解压资源文件总大小
    void OnUnpackFileSize_Callback(object _obj)
    {
        //SetFairyChildProgressMax(mainView, "n2", 100);
        //SetFairyChildText(mainView, "n3", "资源解压中...");
    }

    // 解压资源文件进度
    void OnUnpackProgress_Callback(object _obj)
    {
        //SetFairyChildProgressValue(mainView, "n2", ((float)_obj) * 100);
    }

    // 解压资源文件完成
    void OnUnpackCompletion_Callback(object _obj)
    {
        // 文件解压完毕
        //SetFairyChildProgressValue(mainView, "n2", 100);
        //SetFairyChildText(mainView, "n3", "资源解压完毕...");
    }

    // 总共需要下载的文件大小
    void OnDownloadMaxSize_Callback(object _obj)
    {
        DownloadManager.mInst.DownloadAssets();
        //SetFairyChildVisible(mainView, "n4", true);
        //SetFairyChildText(GetFairyChild(mainView, "n4"), "n9", "需要下载" + GetKbStr((int)_obj) + "更新资源，是否确定更新？");
    }

    // 下载文件更新过程
    void OnDownloadUpdate_Callback(object _obj)
    {
        //SetFairyChildProgressValue(mainView, "n2", ((float)_obj) * 100);
    }

    void OnDownloadNumber_Callback(object _obj)
    {
        //int[] tempNum = (int[])_obj;
        //string tempNumValue = ((tempNum[1] - tempNum[0]) + "/" + tempNum[1]);
        //SetFairyChildText(mainView, "n3", "资源正在下载中(" + tempNumValue + ")");
    }

    float GetMbSize(float _bSize)
    {
        if (_bSize == 0)
        {
            return 0;
        }

        float tempSize = GetKbSize(_bSize) / 1024;
        int tempTargetSize = (int)(tempSize * 100);

        return ((float)tempTargetSize) / 100;
    }

    string GetKbStr(float _bSize) { return GetKbSize(_bSize).ToString("f2") + " KB"; }
    float GetKbSize(float _bSize) { return _bSize == 0 ? 0 : _bSize / 1024; }
}