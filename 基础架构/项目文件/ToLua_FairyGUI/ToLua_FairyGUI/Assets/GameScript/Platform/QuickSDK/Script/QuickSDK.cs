using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

namespace quicksdk
{
    public class QuickSDK
    {
        private static QuickSDK _instance;

        public static QuickSDK getInstance()
        {
            if (null == _instance)
            {
                _instance = new QuickSDK();
            }
            return _instance;
        }

        public void setListener(QuickSDKListener listener)
        {
            QuickSDKImp.getInstance().setListener(listener);
        }

        public void init() //only for android
        {
            QuickSDKImp.getInstance().init();
        }

        public void exit()// 退出游戏 仅android
        {
            QuickSDKImp.getInstance().exit();
        }

        public void login()
        {
            QuickSDKImp.getInstance().login();
        }
        public void logout()
        {
            QuickSDKImp.getInstance().logout();
        }

        public void pay(OrderInfo orderInfo, GameRoleInfo gameRoleInfo)
        {
            QuickSDKImp.getInstance().pay(orderInfo, gameRoleInfo);
        }
        public string userId()//渠道uid
        {
            return QuickSDKImp.getInstance().userId();
        }
        public void updateRoleInfoWith(GameRoleInfo gameRoleInfo, bool isCreateRole)
        {
            QuickSDKImp.getInstance().updateRoleInfoWith(gameRoleInfo, isCreateRole);
        }
        public int enterUserCenter() //用户中心
        {
            return QuickSDKImp.getInstance().enterUserCenter();
        }
        public int enterCustomerCenter() ////客服
        {
            return QuickSDKImp.getInstance().enterCustomerCenter();
        }
        public int enterBBS()//BBS
        {
            return QuickSDKImp.getInstance().enterBBS();
        }
        public int showToolBar(ToolbarPlace place)//1左上,2右上,3左中,4右中,5左下,6右下
        {
            return QuickSDKImp.getInstance().showToolBar(place);
        }
        public int hideToolBar()
        {
            return QuickSDKImp.getInstance().hideToolBar();
        }
        public bool isFunctionTypeSupported(FuncType type)
        {
            return QuickSDKImp.getInstance().isFunctionTypeSupported(type);
        }
        public string channelName()          //获取渠道名称
        {
            return QuickSDKImp.getInstance().channelName();
        }
        public string channelVersion()       //获取渠道版本
        {
            return QuickSDKImp.getInstance().channelVersion();
        }
        public int channelType()                 //获取渠道类别 渠道唯一标识
        {
            return QuickSDKImp.getInstance().channelType();
        }
        public string SDKVersion()      //QuickSDK版本
        {
            return QuickSDKImp.getInstance().SDKVersion();
        }

        public string getConfigValue(string key)
        {
            return QuickSDKImp.getInstance().getConfigValue(key);
        }

        // 关闭游戏退出游戏进程 仅android
        public void exitGame()
        {
            QuickSDKImp.getInstance().exitGame();
        }

        // 判断渠道是否有自带退出框 仅android
        public bool isChannelHasExitDialog()
        {
            return QuickSDKImp.getInstance().isChannelHasExitDialog();
        }
    }
}