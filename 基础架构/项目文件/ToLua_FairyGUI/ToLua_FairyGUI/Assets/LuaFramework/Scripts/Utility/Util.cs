using System;
using System.IO;
using UnityEngine;
using System.Text.RegularExpressions;

namespace LuaFramework
{
    public class Util
    {
        // -------------------- 通用函数 --------------------
        /// <summary>
        /// 随机整数
        /// </summary>
        public static int RandomInt(int _min, int _max) { return UnityEngine.Random.Range(_min, _max); }
        /// <summary>
        /// 随机浮点数
        /// </summary>
        public static float RandomFloat(float _min, float _max) { return UnityEngine.Random.Range(_min, _max); }

        /// <summary>
        /// 网络可用
        /// </summary>
        public static bool NetAvailable { get { return Application.internetReachability != NetworkReachability.NotReachable; } }
        /// <summary>
        /// 是否是无线
        /// </summary>
        public static bool IsWifi { get { return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork; } }

        /// <summary>
        /// 获取运行平台
        /// </summary>
        public static string GetGamePlatform()
        {
            if (RuntimePlatform.Android == Application.platform)
            {
                return "Android";
            }
            else if (RuntimePlatform.IPhonePlayer == Application.platform)
            {
                return "iPhone";
            }

            return "Windows";
        }

        /// <summary>
        /// 是否合法字符，排除非法字符
        /// </summary>
        public static bool IsLegalChar(string _str)
        {
            for (int i = 0; i < _str.Length; i++)
            {
                if (!Regex.IsMatch(_str[i].ToString(), "[a-zA-Z0-9\u4e00-\u9fa5]"))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 执行Lua方法
        /// </summary>
        public static void CallMethod(string _module, string _func, params object[] _args)
        {
            if (LuaManager.mInst == null)
            {
                return;
            }

            LuaManager.mInst.CallMethod(_module, _func, _args);
        }

        /// <summary>
        /// 清理内存
        /// </summary>
        public static void ClearMemory()
        {
            if (LuaManager.mInst == null)
            {
                return;
            }

            GC.Collect();
            Resources.UnloadUnusedAssets();
            LuaManager.mInst.LuaGC();
        }

        public static void Log(string str)
        {
            Debug.Log(str);
        }

        public static void LogWarning(string str)
        {
            Debug.LogWarning(str);
        }

        public static void LogError(string str)
        {
            Debug.LogError(str);
        }

        // -------------------- 文件操作 --------------------
        /// <summary>
        /// 是否存在目标文件
        /// </summary>
        public static bool IsExistAssetBundleFile(string _fileName)
        {
            return File.Exists(AssetBundlesPath + _fileName + AppConst.ExtName);
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        public static void WriteFile(string _fileName, string _contents)
        {
            File.WriteAllText(Application.persistentDataPath + "/" + _fileName, _contents);
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        public static string ReadFile(string _fileName)
        {
            string tempPath = Application.persistentDataPath + "/" + _fileName;
            if (!File.Exists(tempPath))
            {
                return "";
            }

            return File.ReadAllText(tempPath);
        }

        // -------------------- 文件路径 --------------------
        public static string BasePath
        {
            get
            {
                if (RuntimePlatform.IPhonePlayer == Application.platform)
                {
                    if (!VersionManager.mInst.IsUpdate)
                    {
                        // IOS审核中使用本路径
                        return Application.streamingAssetsPath + "/";
                    }
                }

                // Android平台或者IOS审核通过使用本路径
                return Application.persistentDataPath + "/LuaFramework/";
            }
        }

        public static string GetTargetAssetBundlesPath(string _fileName)
        {
            return AssetBundlesPath + _fileName.ToLower() + AppConst.ExtName;
        }

        public static string LuaPath { get { return BaseAssetPath + "lua/"; } }
        public static string ProtoPath { get { return LuaPath + "game/proto/"; } }
        public static string AssetBundlesPath { get { return BaseAssetPath + "assetbundles/"; } }
        public static string BaseAssetPath { get { return Application.isMobilePlatform ? BasePath : Application.dataPath + "/StreamingGameAssets/"; } }
    }
}