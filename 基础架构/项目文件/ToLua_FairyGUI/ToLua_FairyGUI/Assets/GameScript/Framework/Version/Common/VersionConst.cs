using UnityEngine;

public class VersionConst
{
    public const string AssetsUrl = "assets.php";                                   // 资源下载地址
    public const string UpdateAssets = "update.php";                                // 是否更新资源
    public const string VersionFile = "version.php";                                // 版本控制文件
    public const string UnpackFile = "GameAssets.upk";                              // 游戏资源的文件名
    public const string UnpackFolder = "LuaFramework";                              // 解压文件目录
    public const string UnpackMarkFile = "UnpackMark.txt";                          // 解压标示文件，解压完毕才会生成
    public const string BlockwordsFile = "Blockwords.txt";                          // 屏蔽字
    public const string VersionNumberFile = "VersionNumber.txt";                    // 版本号文件
    public const string PackageNumberFile = "PackageNumber.txt";                    // 资源版本号
    public const string PlatformNumberFile = "PlatformNumber.txt";                  // 平台渠道号
    public const string NetworkPath = "http://img.aboilgame.com/ftxgame/FTX_X3/";   // 网络地址

    /// <summary>
    /// 应用程序内容路径
    /// </summary>
    public static string AppContentPath()
    {
        string path = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                path = "jar:file://" + Application.dataPath + "!/assets/";
                break;
            case RuntimePlatform.IPhonePlayer:
                path = Application.dataPath + "/Raw/";
                break;
            default:
                path = Application.streamingAssetsPath + "/";
                break;
        }
        return path;
    }

    /// <summary>
    /// 资源包文件路径
    /// </summary>
    public static string InUnpackFile { get { return AppContentPath() + UnpackFile; } }
    /// <summary>
    /// 屏蔽字文件路径
    /// </summary>
    public static string InBlockwordsFile { get { return AppContentPath() + BlockwordsFile; } }
    /// <summary>
    /// 资源版本号文件路径
    /// </summary>
    public static string InPackageNumberFile { get { return AppContentPath() + PackageNumberFile; } }
    /// <summary>
    /// 版本号文件路径
    /// </summary>
    public static string InVersionNumberFile { get { return AppContentPath() + VersionNumberFile; } }
    /// <summary>
    /// 平台号文件路径
    /// </summary>
    public static string InPlatformNumberFile { get { return AppContentPath() + PlatformNumberFile; } }
    /// <summary>
    /// 资源包加载文件路径
    /// </summary>
    public static string OutUnpackFile { get { return Application.persistentDataPath + "/" + UnpackFile; } }
    /// <summary>
    /// 资源包加载文件夹路径
    /// </summary>
    public static string InUnpackDirectory { get { return Application.persistentDataPath + "/" + UnpackFolder; } }
    /// <summary>
    /// 屏蔽字文件路径
    /// </summary>
    public static string OutBlockwordsFile { get { return Application.persistentDataPath + "/" + BlockwordsFile; } }
    /// <summary>
    /// 资源版本号加载文件路径
    /// </summary>
    public static string OutPackageNumberFile { get { return Application.persistentDataPath + "/" + PackageNumberFile; } }
    /// <summary>
    /// 版本号加载文件地址
    /// </summary>
    public static string OutVersionNumberFile { get { return Application.persistentDataPath + "/" + VersionNumberFile; } }
    /// <summary>
    /// 平台号加载文件路径
    /// </summary>
    public static string OutPlatformNumberFile { get { return Application.persistentDataPath + "/" + PlatformNumberFile; } }
    /// <summary>
    /// 解压标示文件，解压完毕才会生成
    /// </summary>
    public static string UnpackMarkPath { get { return Application.persistentDataPath + "/" + UnpackMarkFile; } }
    /// <summary>
    /// 把PHP文件转换成服务器能识别的网络地址
    /// </summary>
    public static string ToServerPath(string _phpFileName) { return NetworkPath + _phpFileName + "?version=&v=" + Random.Range(0f, 99999999f); }
}