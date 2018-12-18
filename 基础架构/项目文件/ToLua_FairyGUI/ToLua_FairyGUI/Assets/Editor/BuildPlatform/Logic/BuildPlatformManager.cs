using System;
using UnityEngine;
using UnityEditor;

public class BuildPlatformManager
{
    public static void BuildIOS()
    {

    }

    public static void BuildAndroid(string _generatePath, string _productName, string _bundleVersion, string _bundleIdentifier)
    {
        System.IO.File.WriteAllText(PackageNumberFilePath, (GetFileNumber(PackageNumberFilePath) + 1).ToString());  // 提升版本控制号

        string[] tempLevelArray = { "Assets/GameScene/Main.unity" };
        SetPlayerSettings(_productName, _bundleVersion, _bundleIdentifier);
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
        string tempPath = _generatePath + "/" + _productName + "-" + FileTime + ".apk";
        Debug.LogError("发布包信息 --> " + BuildPipeline.BuildPlayer(tempLevelArray, tempPath, BuildTarget.Android, BuildOptions.None));
    }

    static void SetPlayerSettings(string _productName, string _bundleVersion, string _bundleIdentifier)
    {
        PlayerSettings.productName = _productName;
        PlayerSettings.bundleVersion = _bundleVersion;
        PlayerSettings.bundleIdentifier = _bundleIdentifier;
    }

    static int GetFileNumber(string _filePath)
    {
        int tempVersionNum = 0;
        if (System.IO.File.Exists(_filePath))
        {
            string tempNumber = System.IO.File.ReadAllText(_filePath);
            int.TryParse(tempNumber.Trim(), out tempVersionNum);
        }

        return tempVersionNum;
    }

    static string FileTime
    {
        get
        {
            string tempYear = DateTime.Now.Year > 10 ? DateTime.Now.Year.ToString() : "0" + DateTime.Now.Year.ToString();
            string tempMonth = DateTime.Now.Month > 10 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month.ToString();
            string tempDay = DateTime.Now.Day > 10 ? DateTime.Now.Day.ToString() : "0" + DateTime.Now.Day.ToString();
            string tempHour = DateTime.Now.Hour > 10 ? DateTime.Now.Hour.ToString() : "0" + DateTime.Now.Hour.ToString();
            string tempMinute = DateTime.Now.Minute > 10 ? DateTime.Now.Minute.ToString() : "0" + DateTime.Now.Minute.ToString();
            string tempSecond = DateTime.Now.Second > 10 ? DateTime.Now.Second.ToString() : "0" + DateTime.Now.Second.ToString();

            return tempYear + tempMonth + tempDay + tempHour + tempMinute + tempSecond;
        }
    }

    static string PackageNumberFilePath { get { return Application.streamingAssetsPath + "/" + VersionConst.PackageNumberFile; } }
}