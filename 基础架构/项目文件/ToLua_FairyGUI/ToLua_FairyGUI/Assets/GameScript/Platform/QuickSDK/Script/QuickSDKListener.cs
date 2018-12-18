﻿using UnityEngine;
using System.Collections;

namespace quicksdk
{
    // QuickSDKListener
    public abstract class QuickSDKListener : MonoBehaviour
    {
        //callback
        public abstract void onInitSuccess();

        public abstract void onInitFailed(ErrorMsg message);

        public abstract void onLoginSuccess(UserInfo userInfo);

        public abstract void onLoginFailed(ErrorMsg errMsg);

        public abstract void onLogoutSuccess();

        public abstract void onLogoutFailed();

        public abstract void onPaySuccess(PayResult payResult);

        public abstract void onPayFailed(PayResult payResult);

        public abstract void onPayCancel(PayResult payResult);

        public abstract void onExitSuccess();

        public abstract void onExitFailed();
        //callback end

        public void onInitSuccess(string msg)
        {
            onInitSuccess();
        }

        public void onInitFailed(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            ErrorMsg errMsg = new ErrorMsg();
            errMsg.errMsg = data["msg"].Value;

            onInitFailed(errMsg);
        }

        public void onLoginSuccess(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            UserInfo userInfo = new UserInfo();
            userInfo.uid = data["userId"].Value;
            userInfo.token = data["userToken"].Value;
            userInfo.userName = data["userName"].Value;
            userInfo.errMsg = data["msg"].Value;

            onLoginSuccess(userInfo);
        }

        public void onLoginFailed(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            ErrorMsg errMsg = new ErrorMsg();
            errMsg.errMsg = data["msg"].Value;

            onLoginFailed(errMsg);
        }

        public void onLogoutSuccess(string msg)
        {
            onLogoutSuccess();
        }

        public void onLogoutFailed(string msg)
        {
            onLogoutFailed();
        }

        public void onPaySuccess(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            PayResult result = new PayResult();
            result.cpOrderId = data["cpOrderId"].Value;
            result.orderId = data["orderId"].Value;
            result.extraParam = data["extraParam"].Value;

            onPaySuccess(result);
        }

        public void onPayFailed(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            PayResult result = new PayResult();
            result.cpOrderId = data["cpOrderId"].Value;
            result.orderId = data["orderId"].Value;
            result.extraParam = data["extraParam"].Value;

            onPayFailed(result);
        }

        public void onPayCancel(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            PayResult result = new PayResult();
            result.cpOrderId = data["cpOrderId"].Value;
            result.orderId = data["orderId"].Value;
            result.extraParam = data["extraParam"].Value;

            onPayCancel(result);
        }

        public void onExitSuccess(string msg)
        {
            onExitSuccess();
        }

        public void onExitFailed(string msg)
        {
            onExitFailed();
        }
    }
}