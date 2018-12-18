using UnityEngine;
using System.Collections;

public class QuickSDKCallback : MonoBehaviour
{
    float time = 0;

    void Start()
    {
        Events.msInst.AddEventListener(SDKEvent.Init, new GameEventCallBack(OnServiceInit_Callback));
        Events.msInst.AddEventListener(SDKEvent.Login, new GameEventCallBack(OnServiceLogin_Callback));
		Events.msInst.AddEventListener(SDKEvent.Logout, new GameEventCallBack(OnServiceLogout_Callback));
        Events.msInst.AddEventListener(SDKEvent.ExitGame, new GameEventCallBack(OnServiceExitGame_Callback));
    }

    void OnServiceInit_Callback(object _obj)
    {
        Invoke("OnSDKServiceLogin_Invoke", 2f);
    }

    void OnSDKServiceLogin_Invoke()
    {
        if (Time.time - time < 3)
        {
            //return;
        }
        time = Time.time;

        QuickSDKManager.mInst.SDKServiceLogin();
    }

    void OnServiceLogin_Callback(object _obj)
    {
        string temp_login_url = QuickSDKManager.mInst.GetLoginUrl();
        WWWForm temp_form = QuickSDKManager.mInst.GetForm();
        StartCoroutine_Auto(wwwRequst(temp_login_url, temp_form, data =>
        {
            LuaFramework.Util.CallMethod("Game", "SDKLogin", data.ToString());
        }));
    }

	void OnServiceLogout_Callback(object _obj)
	{
        LuaFramework.Util.CallMethod("Game", "LoginOut", _obj);
    }

    void OnServiceExitGame_Callback(object _obj)
    {
        LuaFramework.Util.CallMethod("Game", "SDKServiceExitGame");     // 是否退出
    }

    IEnumerator wwwRequst(string url, WWWForm _form, System.Action<string> cb)
    {
        WWW w = new WWW(url, _form);

        Debug.LogWarning("LoginServer.wwwRequst,url=" + w.url);
        yield return w;
        if (!string.IsNullOrEmpty(w.error))
        {
            Debug.LogWarning("LoginServer.wwwRequst,error=" + w.error + ",url=" + w.url);
            yield break;
        }
        string data = w.text;
        Debug.LogWarning("LoginServer.wwwRequst,data= " + data);
        w.Dispose();
        if (cb != null)
        {
            cb(data);
        }
    }
}