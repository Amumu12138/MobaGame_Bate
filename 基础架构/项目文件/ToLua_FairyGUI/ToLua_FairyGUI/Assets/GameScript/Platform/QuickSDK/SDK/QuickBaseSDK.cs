using quicksdk;
using UnityEngine;

public class QuickBaseSDK : QuickSDKListener
{
    float exitGameTime = 0;
    bool initSuccess = false;
    bool loginSuccess = false;

    public UserInfo userInfo = null;
    public OrderInfo orderInfo = new OrderInfo();
    GameRoleInfo gameRoleInfo = new GameRoleInfo();

    void Start()
    {
        QuickSDK.getInstance().setListener(this);
    }

    public void SDKServiceInit()
    {
        QuickSDK.getInstance().init();
    }

    public void SDKServiceLogin()
    {
        if (!initSuccess)
        {
            return;
        }

        if (loginSuccess)
        {
            Events.msInst.DispatchEvent(SDKEvent.Login, "");
            return;
        }

        QuickSDK.getInstance().login();
    }

    public void SDKServiceLogout()
    {
        QuickSDK.getInstance().hideToolBar();
        QuickSDK.getInstance().logout();
    }

    public void SDKServiceExitGame()
    {
        if (Time.time - exitGameTime < 0.5f)
        {
            return;
        }
        exitGameTime = Time.time;

        if (QuickSDK.getInstance().isChannelHasExitDialog())
        {
            QuickSDK.getInstance().exit();
            return;
        }

        Events.msInst.DispatchEvent(SDKEvent.ExitGame, "");
    }

    public void SDKServiceRoleInfo(string _roleID, string _roleLevel, string _roleName, string _teamName, string _serverID, string _serverName, string _vipLevel)
    {
        gameRoleInfo.gameRoleID = _roleID;
        gameRoleInfo.gameRoleLevel = _roleLevel;
        gameRoleInfo.gameRoleName = _roleName;
        gameRoleInfo.partyName = _teamName;
        gameRoleInfo.serverID = _serverID;
        gameRoleInfo.serverName = _serverName;
        gameRoleInfo.vipLevel = _vipLevel;
        gameRoleInfo.gameRoleBalance = "100";

        QuickSDK.getInstance().updateRoleInfoWith(gameRoleInfo, (_roleLevel == "0" || _roleLevel == "1"));
    }

    public void SDKServicePurchase(string _requestId, string _price, string _productName, string _productDesc)
    {
        SDKServicePurchase(_requestId, float.Parse(_price), _productName, _productDesc);
    }

    public void SDKServicePurchase(string _requestId, float _price, string _productName, string _productDesc)
    {
        string tempOrderID = GetOrderID(_requestId);
        orderInfo.cpOrderID = tempOrderID;
        orderInfo.goodsName = _productName;
        orderInfo.goodsDesc = _productDesc;
        orderInfo.amount = _price;
        orderInfo.price = _price;
        orderInfo.extrasParams = tempOrderID;
        orderInfo.callbackUrl = GetCallbackUrl();
        orderInfo.goodsID = GetGoodsID(_price);
        orderInfo.count = GetPurchaseCount(_price);

        QuickSDK.getInstance().pay(orderInfo, gameRoleInfo);
    }

    public WWWForm GetForm()
    {
        WWWForm temp_form = new WWWForm();
        temp_form.AddField("openid", userInfo.uid);
        temp_form.AddField("token", userInfo.token);
        temp_form.AddField("channel", GetChannel());
        temp_form.AddField("source", GetSource());
        temp_form.AddField("system", "Android");
        temp_form.AddField("Product_Key", GetProductKey());

        return temp_form;
    }

    public bool IsLoginSuccess() { return loginSuccess; }
    public virtual string GetLoginUrl() { return "http://zgameweb.ftxgame.com/quick/login"; }

    protected virtual string GetSource() { return ""; }
    protected virtual string GetCallbackUrl() { return ""; }
    protected virtual string GetSystem() { return "Android"; }
    protected virtual string GetProductKey() { return "81940917"; }
    protected virtual string GetGoodsID(float _price) { return "1"; }
    protected virtual int GetPurchaseCount(float _price) { return 1; }
    protected virtual string GetOrderID(string _orderID) { return _orderID; }
    protected virtual string GetChannel() { return QuickSDK.getInstance().channelType().ToString(); }

    public override void onInitSuccess()
    {
        initSuccess = true;
        Events.msInst.DispatchEvent(SDKEvent.Init, "");
    }

    public override void onLoginSuccess(UserInfo _userInfo)
    {
        userInfo = _userInfo;
        if (!loginSuccess)
        {
            loginSuccess = true;
        }
        else
        {
            Events.msInst.DispatchEvent(SDKEvent.Logout, "");
            return;
        }

        Events.msInst.DispatchEvent(SDKEvent.Login, "");
        QuickSDK.getInstance().showToolBar(ToolbarPlace.QUICK_SDK_TOOLBAR_BOT_LEFT);
    }

    public override void onLogoutSuccess()
    {
        loginSuccess = false;
        Events.msInst.DispatchEvent(SDKEvent.Logout, "");
    }

    public override void onExitSuccess()
    {
        Application.Quit();
    }

    public override void onExitFailed() { }
    public override void onLogoutFailed() { }
    public override void onInitFailed(ErrorMsg message) { }
    public override void onLoginFailed(ErrorMsg errMsg) { }
    public override void onPayFailed(PayResult payResult) { }
    public override void onPayCancel(PayResult payResult) { }
    public override void onPaySuccess(PayResult payResult) { }

    public static QuickChannelType GetChannelType() { return (QuickChannelType)QuickSDK.getInstance().channelType(); }
}