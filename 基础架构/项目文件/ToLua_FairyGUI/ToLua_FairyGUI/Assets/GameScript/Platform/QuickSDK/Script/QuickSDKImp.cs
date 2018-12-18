using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

namespace quicksdk
{
    public class OrderInfo
    {
        public string goodsID;
        public string goodsName;
        public string goodsDesc;
        public string quantifier; //商品量词
        public string cpOrderID;
        public string callbackUrl;
        public string extrasParams;
        public double price;
        public double amount;
        public int count;
    };

    public class GameRoleInfo
    {
        public string serverName;
        public string serverID;
        public string gameRoleName;
        public string gameRoleID;
        public string gameRoleBalance;
        public string vipLevel;
        public string gameRoleLevel;
        public string partyName;
    };

    public enum FuncType
    {

        QUICK_SDK_FUNC_TYPE_UNDEFINED,
        QUICK_SDK_FUNC_TYPE_ENTER_BBS,/*进入论坛*/
        QUICK_SDK_FUNC_TYPE_ENTER_USER_CENTER,/*进入用户中心*/
        QUICK_SDK_FUNC_TYPE_SHOW_TOOLBAR,/*显示浮动工具栏*/
        QUICK_SDK_FUNC_TYPE_HIDE_TOOLBAR,/*隐藏浮动工具栏*/
        QUICK_SDK_FUNC_TYPE_SWITCH_ACCOUNT, /*切换账号 （android）*/
        QUICK_SDK_FUNC_TYPE_PAUSED_GAME,/*暂停游戏 （iOS）*/
        QUICK_SDK_FUNC_TYPE_OPEN_URL,/*处理应用跳转openURL:sourceApplication:application:annotation: （iOS）*/
        QUICK_SDK_FUNC_TYPE_REAL_NAME_REGISTER,/*实名认证 （android）*/
        QUICK_SDK_FUNC_TYPE_ANTI_ADDICTION_QUERY, /*防沉迷 （android）*/
        QUICK_SDK_FUNC_TYPE_SHARE, /* 分享 （android）*/
    }

    public enum ToolbarPlace
    {
        QUICK_SDK_TOOLBAR_TOP_LEFT = 1,           /* 左上 */
        QUICK_SDK_TOOLBAR_TOP_RIGHT = 2,           /* 右上 */
        QUICK_SDK_TOOLBAR_MID_LEFT = 3,           /* 左中 */
        QUICK_SDK_TOOLBAR_MID_RIGHT = 4,           /* 右中 */
        QUICK_SDK_TOOLBAR_BOT_LEFT = 5,           /* 左下 */
        QUICK_SDK_TOOLBAR_BOT_RIGHT = 6,           /* 右下 */
    }

    // 错误信息
    public class ErrorMsg
    {
        public string errMsg;
    }

    // 用户信息，登录回调中使用
    public class UserInfo : ErrorMsg
    {
        public string uid;
        public string userName;
        public string token;
    }

    // 支付信息，支付回调中使用
    public class PayResult
    {
        public string orderId;
        public string cpOrderId;
        public string extraParam;
    }

    public class QuickSDKImp
    {
        private static QuickSDKImp _instance;

        public static QuickSDKImp getInstance()
        {
            if (null == _instance)
            {
                _instance = new QuickSDKImp();
            }
            return _instance;
        }

        public void setListener(QuickSDKListener listener)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			QuickUnitySupportAndroid androidSupport = QuickUnitySupportAndroid.getInstance();
			androidSupport.setListener(listener);
#endif
        }

        public void init()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            QuickUnitySupportAndroid androidSupport = QuickUnitySupportAndroid.getInstance();
            androidSupport.init();
#endif
        }

        public void exit()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			QuickUnitySupportAndroid androidSupport = QuickUnitySupportAndroid.getInstance();
			androidSupport.exit();
#endif
        }

        public void login()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			QuickUnitySupportAndroid androidSupport = QuickUnitySupportAndroid.getInstance();
			androidSupport.login();
#endif
        }
        public void logout()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			QuickUnitySupportAndroid androidSupport = QuickUnitySupportAndroid.getInstance();
			androidSupport.logout();
#endif
        }

        public void pay(OrderInfo orderInfo, GameRoleInfo gameRoleInfo)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			QuickUnitySupportAndroid androidSupport = QuickUnitySupportAndroid.getInstance();
			androidSupport.pay(orderInfo, gameRoleInfo);
#endif
        }
        public string userId()//uid
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			QuickUnitySupportAndroid androidSupport = QuickUnitySupportAndroid.getInstance();
			return androidSupport.getUserId();
#else
            return "";
#endif

        }
        public void updateRoleInfoWith(GameRoleInfo gameRoleInfo, bool isCreateRole)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			QuickUnitySupportAndroid androidSupport = QuickUnitySupportAndroid.getInstance();
			androidSupport.updateRoleInfo(gameRoleInfo, isCreateRole);
#endif
        }
        public int enterUserCenter() //用户中心
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			QuickUnitySupportAndroid androidSupport = QuickUnitySupportAndroid.getInstance();
			return androidSupport.callFunc(FuncType.QUICK_SDK_FUNC_TYPE_ENTER_USER_CENTER);
#else
            return 0;
#endif

        }
        public int enterCustomerCenter() ////客服
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			return 0;
#else
            return 0;
#endif

        }
        public int enterBBS()//BBS
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			QuickUnitySupportAndroid androidSupport = QuickUnitySupportAndroid.getInstance();
			return androidSupport.callFunc(FuncType.QUICK_SDK_FUNC_TYPE_ENTER_BBS);
#else
            return 0;
#endif

        }
        public int showToolBar(ToolbarPlace place)//1左上,2右上,3左中,4右中,5左下,6右下
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			QuickUnitySupportAndroid androidSupport = QuickUnitySupportAndroid.getInstance();
			return androidSupport.callFunc(FuncType.QUICK_SDK_FUNC_TYPE_SHOW_TOOLBAR);
#else
            return 0;
#endif

        }
        public int hideToolBar()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			QuickUnitySupportAndroid androidSupport = QuickUnitySupportAndroid.getInstance();
			return androidSupport.callFunc(FuncType.QUICK_SDK_FUNC_TYPE_HIDE_TOOLBAR);
#else
            return 0;
#endif

        }
        public bool isFunctionTypeSupported(FuncType type)//1暂停游戏,2进入用户中心,3进入论坛,4处理应用跳转(旧),5显示浮动工具栏,6隐藏浮动工具栏,7处理应用跳转(新)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			QuickUnitySupportAndroid androidSupport = QuickUnitySupportAndroid.getInstance();
			return androidSupport.isFuncSupport(type);
#else
            return false;
#endif

        }

        public void callFunction(FuncType type)//only for android
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			QuickUnitySupportAndroid androidSupport = QuickUnitySupportAndroid.getInstance();
			androidSupport.callFunc(type);
#endif
        }

        public string channelName()          //获取渠道名称
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			QuickUnitySupportAndroid androidSupport = QuickUnitySupportAndroid.getInstance();
			return androidSupport.getChannelName();
#else
            return "";
#endif

        }
        public string channelVersion()       //获取渠道版本
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			QuickUnitySupportAndroid androidSupport = QuickUnitySupportAndroid.getInstance();
			return androidSupport.getChannelVersion();
#else
            return "";
#endif

        }
        public int channelType()                 //获取渠道类别 渠道唯一标识
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			QuickUnitySupportAndroid androidSupport = QuickUnitySupportAndroid.getInstance();
			return androidSupport.getChannelType();
#else
            return 0;
#endif

        }
        public string SDKVersion()      //QuickSDK版本
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			QuickUnitySupportAndroid androidSupport = QuickUnitySupportAndroid.getInstance();
			return androidSupport.getSDKVersion();
#else
            return "";
#endif

        }

        public string getConfigValue(string key)      //QuickSDK版本
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            QuickUnitySupportAndroid androidSupport = QuickUnitySupportAndroid.getInstance();
            return androidSupport.getConfigValue(key);
#else
            return "";
#endif

        }

        public bool isChannelHasExitDialog()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            QuickUnitySupportAndroid androidSupport = QuickUnitySupportAndroid.getInstance();
            return androidSupport.isChannelHasExitDialog();
#else
            return false;
#endif
        }

        public void exitGame()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            QuickUnitySupportAndroid androidSupport = QuickUnitySupportAndroid.getInstance();
            androidSupport.exitGame();
#endif

        }
    }

#if UNITY_ANDROID && !UNITY_EDITOR

    public class QuickUnitySupportAndroid {

        AndroidJavaObject ao;

        private static QuickUnitySupportAndroid instance;

        private QuickUnitySupportAndroid() {
            
            AndroidJavaClass ac = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            ao = ac.GetStatic<AndroidJavaObject>("currentActivity");
        }

        public static QuickUnitySupportAndroid getInstance()
        {
            if (instance == null)
            {
                instance = new QuickUnitySupportAndroid();
            }

            return instance;
        }

		public void setListener(QuickSDKListener listener)
        {
            Debug.Log("gameObject is " + listener.gameObject.name);
            if (listener == null)
            {
                Debug.LogError("set QuickSDKListener error, listener is null");
                return;
            }
            string gameObjectName = listener.gameObject.name;
            if (ao == null)
            {
                Debug.LogError("setListener error, current activity is null");
            }
            else
            {
                ao.Call("setUnityGameObjectName", gameObjectName);
            }
        }

        public void init()
        {
            ao.Call("requestInit");
        }

        public void exit()
        {
            ao.Call("requestExit");
        }

        public void login()
        {
            ao.Call("requestLogin");
        }

        public void logout()
        {
            ao.Call("requestLogout");
        }

        public void pay(OrderInfo orderInfo, GameRoleInfo gameRoleInfo)
        {
            if (orderInfo == null)
            {
                Debug.LogError("call pay error, orderInfo is null");
                return;
            }
            ao.Call("requestPay",
                orderInfo.goodsID, orderInfo.goodsName, 
                orderInfo.goodsDesc, orderInfo.quantifier, 
                orderInfo.cpOrderID, orderInfo.callbackUrl, 
                orderInfo.extrasParams, orderInfo.price+"", 
                orderInfo.amount + "", orderInfo.count+"",
                
                gameRoleInfo.serverName, gameRoleInfo.serverID,
                gameRoleInfo.gameRoleName, gameRoleInfo.gameRoleID,
                gameRoleInfo.gameRoleBalance, gameRoleInfo.vipLevel,
                gameRoleInfo.gameRoleLevel, gameRoleInfo.partyName);
        }

        public string getUserId()
        {
            return ao.Call<string>("getUserId");
        }

        public void updateRoleInfo(GameRoleInfo gameRoleInfo, bool isCreate)
        {
            if (gameRoleInfo.Equals(null))
            {
                Debug.LogError("updateRoleInfo is error, gameRoleInfo is null");
                return;
            }

            string serverName = String.IsNullOrEmpty(gameRoleInfo.serverName) ? "" : gameRoleInfo.serverName;
            string serverId = String.IsNullOrEmpty(gameRoleInfo.serverID) ? "" : gameRoleInfo.serverID;
            string roleName = String.IsNullOrEmpty(gameRoleInfo.gameRoleName) ? "" : gameRoleInfo.gameRoleName;
            string roleId = String.IsNullOrEmpty(gameRoleInfo.gameRoleID) ? "" : gameRoleInfo.gameRoleID;
            string roleBalance = String.IsNullOrEmpty(gameRoleInfo.gameRoleBalance) ? "" : gameRoleInfo.gameRoleBalance;
            string vipLevel = String.IsNullOrEmpty(gameRoleInfo.vipLevel) ? "" : gameRoleInfo.vipLevel;
            string roleLevel = String.IsNullOrEmpty(gameRoleInfo.gameRoleLevel) ? "" : gameRoleInfo.gameRoleLevel;
            string partyName = String.IsNullOrEmpty(gameRoleInfo.partyName) ? "" : gameRoleInfo.partyName;

            ao.Call("requestUpdateRole",
                serverId,
                serverName,
                roleName,
                roleId,
                roleBalance,
                vipLevel,
                roleLevel,
                partyName,
                isCreate + "");
            Debug.LogWarning("updateRoleInfo executed");
        }

        /**
         * return 0 success, -100 false or not support such function
         */
        public int callFunc(FuncType funcType)
        {
            int androidFuncType = 0;
            switch (funcType)
            {
                case FuncType.QUICK_SDK_FUNC_TYPE_UNDEFINED:
                    // Do nothing
                    break;
                case FuncType.QUICK_SDK_FUNC_TYPE_ENTER_BBS:
                    androidFuncType = 101;
                    break;

                case FuncType.QUICK_SDK_FUNC_TYPE_ENTER_USER_CENTER:
                    androidFuncType = 102;
                    break;

                case FuncType.QUICK_SDK_FUNC_TYPE_SHOW_TOOLBAR:
                    androidFuncType = 103;
                    break;

                case FuncType.QUICK_SDK_FUNC_TYPE_HIDE_TOOLBAR:
                    androidFuncType = 104;
                    break;
                case FuncType.QUICK_SDK_FUNC_TYPE_SWITCH_ACCOUNT:
                    androidFuncType = 107;
                    break;
                case FuncType.QUICK_SDK_FUNC_TYPE_REAL_NAME_REGISTER:
                    androidFuncType = 105;
                    break;
                case FuncType.QUICK_SDK_FUNC_TYPE_ANTI_ADDICTION_QUERY:
                    androidFuncType = 106;
                    break;
                case FuncType.QUICK_SDK_FUNC_TYPE_SHARE:
                    androidFuncType = 108;
                    break;
            }

            // TODO
            return ao.Call<int>("callFunc", androidFuncType);
        }


        public bool isFuncSupport(FuncType funcType)
        {
            int androidFuncType = 0;
            switch (funcType)
            {
                case FuncType.QUICK_SDK_FUNC_TYPE_UNDEFINED:
                    // Do nothing
                    break;
                case FuncType.QUICK_SDK_FUNC_TYPE_ENTER_BBS:
                    androidFuncType = 101;
                    break;

                case FuncType.QUICK_SDK_FUNC_TYPE_ENTER_USER_CENTER:
                    androidFuncType = 102;
                    break;

                case FuncType.QUICK_SDK_FUNC_TYPE_SHOW_TOOLBAR:
                    androidFuncType = 103;
                    break;

                case FuncType.QUICK_SDK_FUNC_TYPE_HIDE_TOOLBAR:
                    androidFuncType = 104;
                    break;
                case FuncType.QUICK_SDK_FUNC_TYPE_SWITCH_ACCOUNT:
                    androidFuncType = 107;
                    break;
                case FuncType.QUICK_SDK_FUNC_TYPE_REAL_NAME_REGISTER:
                    androidFuncType = 105;
                    break;
                case FuncType.QUICK_SDK_FUNC_TYPE_ANTI_ADDICTION_QUERY:
                    androidFuncType = 106;
                    break;
                case FuncType.QUICK_SDK_FUNC_TYPE_SHARE:
                    androidFuncType = 108;
                    break;
            }
            return ao.Call<bool>("isFuncSupport", androidFuncType);
        }

        public string getChannelName()
        {
            return ao.Call<string>("getChannelName");
        }

        public string getChannelVersion()
        {
            return ao.Call<string>("getChannelVersion");
        }

        public int getChannelType()
        {
            return ao.Call<int>("getChannelType");
        }

        public string getSDKVersion()
        {
            return ao.Call<string>("getSDKVersion");
        }

        public string getConfigValue(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                return null;
            }
            return ao.Call<string>("getConfigValue", key);
        }

        public bool isChannelHasExitDialog()
        {
            return ao.Call<bool>("isChannelHasExitDialog");
        }

        public void exitGame()
        {
            ao.Call("finish");
        }
    
}
#endif
}