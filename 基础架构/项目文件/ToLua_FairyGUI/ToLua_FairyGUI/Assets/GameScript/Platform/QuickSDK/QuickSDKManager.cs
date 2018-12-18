using UnityEngine;

public class QuickSDKManager : MonoBehaviour
{
    public static QuickSDKManager mInst;

    public QuickBaseSDK baseSDK = null;

    void Awake()
    {
        if (mInst != null)
        {
            return;
        }

        mInst = this;
    }

    void Start()
    {
        if (QuickBaseSDK.GetChannelType() == QuickChannelType.Nont)
        {
            return;
        }

        GameObject tempGo = new GameObject("QuickPlatformSDK");

        if (QuickBaseSDK.GetChannelType() == QuickChannelType.Sogou)
        {
            baseSDK = tempGo.AddComponent<QuickSogouSDK>();
        }
		else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Aiyouxi)
		{
			baseSDK = tempGo.AddComponent<QuickAiyouxiSDK>();
		}
		else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Ant)
		{
			baseSDK = tempGo.AddComponent<QuickAntSDK>();
		}
		else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Ccplay)
		{
			baseSDK = tempGo.AddComponent<QuickCcplaySDK>();
		}
		else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Droi)
		{
			baseSDK = tempGo.AddComponent<QuickDroiSDK>();
		}
		else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Ewan)
		{
			baseSDK = tempGo.AddComponent<QuickEwanSDK>();
		}
		else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Gfan)
		{
			baseSDK = tempGo.AddComponent<QuickGfanSDK>();
		}
		else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Haima)
		{
			baseSDK = tempGo.AddComponent<QuickHaimaSDK>();
		}
		else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Itools)
		{
			baseSDK = tempGo.AddComponent<QuickItoolsSDK>();
		}
		else if (QuickBaseSDK.GetChannelType() == QuickChannelType.KuGou)
		{
			baseSDK = tempGo.AddComponent<QuickKuGouSDK>();
		}
		else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Letv)
		{
			baseSDK = tempGo.AddComponent<QuickLetvSDK>();
		}
		else if (QuickBaseSDK.GetChannelType() == QuickChannelType.M2324)
		{
			baseSDK = tempGo.AddComponent<QuickM2324SDK>();
		}
		else if (QuickBaseSDK.GetChannelType() == QuickChannelType.PPS)
		{
			baseSDK = tempGo.AddComponent<QuickPpsSDK>();
		}
		else if (QuickBaseSDK.GetChannelType() == QuickChannelType.PPTV)
		{
			baseSDK = tempGo.AddComponent<QuickPptvSDK>();
		}
		else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Shoumeng)
		{
			baseSDK = tempGo.AddComponent<QuickShoumengSDK>();
		}
		else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Sy37)
		{
			baseSDK = tempGo.AddComponent<QuickSy37SDK>();
		}
		else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Yiyu)
		{
			baseSDK = tempGo.AddComponent<QuickYiyuSDK>();
		}
		else if (QuickBaseSDK.GetChannelType() == QuickChannelType.YouKu)
		{
			baseSDK = tempGo.AddComponent<QuickYouKuSDK>();
		}
		else if (QuickBaseSDK.GetChannelType() == QuickChannelType._7K)
		{
			baseSDK = tempGo.AddComponent<Quick7KSDK>();
		}
		else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Wandoujia)
		{
			baseSDK = tempGo.AddComponent<QuickWandoujiaSDK>();
		}
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.YouLong)
        {
            baseSDK = tempGo.AddComponent<QuickYouLongSDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Vivo)
        {
            baseSDK = tempGo.AddComponent<QuickVivoSDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Anzhi)
        {
            baseSDK = tempGo.AddComponent<QuickAnzhiSDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Baidu)
        {
            baseSDK = tempGo.AddComponent<QuickBaiduSDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Coolpad)
        {
            baseSDK = tempGo.AddComponent<QuickCoolpadSDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Downjoy)
        {
            baseSDK = tempGo.AddComponent<QuickDownjoySDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Guopan)
        {
            baseSDK = tempGo.AddComponent<QuickGuopanSDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Huawei)
        {
            baseSDK = tempGo.AddComponent<QuickHuaweiSDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Iapppay)
        {
            baseSDK = tempGo.AddComponent<QuickIapppaySDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Jinli)
        {
            baseSDK = tempGo.AddComponent<QuickJinliSDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Keno)
        {
            baseSDK = tempGo.AddComponent<QuickKenoSDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Lenovo)
        {
            baseSDK = tempGo.AddComponent<QuickLenovoSDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Letvs)
        {
            baseSDK = tempGo.AddComponent<QuickLetvsSDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.M4399)
        {
            baseSDK = tempGo.AddComponent<QuickM4399SDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Meizu)
        {
            baseSDK = tempGo.AddComponent<QuickMeizuSDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Muzhiwan)
        {
            baseSDK = tempGo.AddComponent<QuickMuzhiwanSDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Oppo)
        {
            baseSDK = tempGo.AddComponent<QuickOppoSDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Qihoo360)
        {
            baseSDK = tempGo.AddComponent<QuickQihoo360SDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Tencent)
        {
            baseSDK = tempGo.AddComponent<QuickTencentSDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.UC)
        {
            baseSDK = tempGo.AddComponent<QuickUcSDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Xiaomi)
        {
            baseSDK = tempGo.AddComponent<QuickXiaomiSDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Haixin)
        {
            baseSDK = tempGo.AddComponent<QuickHaixinSDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.Yike)
        {
            baseSDK = tempGo.AddComponent<QuickYikeSDK>();
        }
        else if (QuickBaseSDK.GetChannelType() == QuickChannelType.LieBao)
        {
            baseSDK = tempGo.AddComponent<QuickLieBaoSDK>();
        }

        DontDestroyOnLoad(tempGo);
    }

    public void SDKServiceInit()
    {
        if (baseSDK == null)
        {
            return;
        }

        baseSDK.SDKServiceInit();
    }

    public void SDKServiceLogin()
    {
        if (baseSDK == null)
        {
            return;
        }

        baseSDK.SDKServiceLogin();
    }

    public void SDKServiceLogout()
    {
        if (baseSDK == null)
        {
            return;
        }

        baseSDK.SDKServiceLogout();
    }

    public void SDKServiceExitGame()
    {
        if (baseSDK == null)
        {
            return;
        }

        baseSDK.SDKServiceExitGame();
    }

    public void SDKServiceSubmitData(string _roleID, string _roleLevel, string _roleName, string _teamName, string _serverID, string _serverName, string _vipLevel)
    {
        if (baseSDK == null)
        {
            return;
        }

        baseSDK.SDKServiceRoleInfo(_roleID, _roleLevel, _roleName, _teamName, _serverID, _serverName, _vipLevel);
    }

    public void SDKServicePurchase(string _requestId, string _price, string _productName, string _productDesc)
    {
        if (baseSDK == null)
        {
            return;
        }

        baseSDK.SDKServicePurchase(_requestId, _price, _productName, _productDesc);
    }

    public string GetLoginUrl()
    {
        if (baseSDK == null)
        {
            return "";
        }

        return baseSDK.GetLoginUrl();
    }

    public WWWForm GetForm()
    {
        if (baseSDK == null)
        {
            return null;
        }

        return baseSDK.GetForm();
    }

    public bool IsLoginSuccess()
    {
        if (baseSDK == null)
        {
            return false;
        }

        return baseSDK.IsLoginSuccess();
    }

    public int GetChannelId()
    {
        return quicksdk.QuickSDK.getInstance().channelType();
    }
}