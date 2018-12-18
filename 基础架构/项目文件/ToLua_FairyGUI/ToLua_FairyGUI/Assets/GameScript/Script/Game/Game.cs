using FairyGUI;
using UnityEngine;
using LuaFramework;
using GameFramework;

public class Game : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 30;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        SetContentScaleFactor(Screen.width, Screen.height);

        // -------------- 架构相关 --------------
        gameObject.AddComponent<FPS>();
        gameObject.AddComponent<Events>();
        gameObject.AddComponent<LuaManager>();
        gameObject.AddComponent<NetManager>();
        gameObject.AddComponent<TimeManager>();
        gameObject.AddComponent<AudioManager>();
        gameObject.AddComponent<FairyManager>();
        gameObject.AddComponent<VersionManager>();
        gameObject.AddComponent<DownloadManager>();
        gameObject.AddComponent<GlobalController>();

        // --------------- UI相关 ---------------
        gameObject.AddComponent<IconManager>();
        gameObject.AddComponent<VersionView>();
        gameObject.AddComponent<MyLoaderExtension>();
    }

    void OnApplicationPause(bool _pauseStatus)
    {
        Util.CallMethod("Game", "OnApplicationPause", _pauseStatus);     // 是否暂停
    }

    void OnApplicationFocus(bool _focusStatus)
    {
        Util.CallMethod("Game", "OnApplicationFocus", _focusStatus);     // 是否失去焦点
    }

    void SetContentScaleFactor(int _designResolutionX, int _designResolutionY)
    {
        GRoot.inst.SetContentScaleFactor(750, 1334);
        Vector3 tempLocalScale = GRoot.inst.displayObject.cachedTransform.localScale;
        float tempScaleFactor = (float)_designResolutionX / (float)_designResolutionY;
        if (tempScaleFactor < 0.75)
        {
            return;
        }

        // 针对IPAD适配
        float tempScaleX = (tempLocalScale.x / (750f / 1334f)) * tempScaleFactor;
        GRoot.inst.displayObject.cachedTransform.localScale = new Vector3(tempScaleX, tempLocalScale.y, tempLocalScale.z);
    }
}