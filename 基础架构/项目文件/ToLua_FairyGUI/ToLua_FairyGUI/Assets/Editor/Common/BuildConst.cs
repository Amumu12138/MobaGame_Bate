using UnityEngine;

public class BuildConst
{
    public const string IosServer = "Server_2000";
    public const string AndroidServer = "Server_3000";
    public const string TestIosServer = "Server_0000";
    public const string TestAndroidServer = "Server_1000";
    public const string IosJailbreakingServer = "Server_4000";

    public const string BytesName = ".bytes";
    public const string BuildLuaName = ".lua";
    public const string AppName = "LuaFramework";
    public const string BuildName = "UpdateAsset";
    public const string ExtName = ".assetbundles";
    public const string VersionFile = "version.php";
    public const string UnpackFile = "GameAssets.upk";
    public const string ServerPassword = "nrNzzTq$xdj5no0z";
    public const string ServerPath = "103.249.193.169:/data/www/ftxgame/FTX_X3/";

    // -------------------------------------------------- 打包资源 --------------------------------------------------
    public static string AssetPath { get { return Application.dataPath + "/GameAssets/"; } }
    public static string BuildFolderAssetPath { get { return BuildAssetPath + "/fairy_assets"; } }
    public static string FairyAssetPath { get { return Application.dataPath + "/StreamingFairyAssets/"; } }
    public static string BuildLuaTempPath { get { return Application.dataPath + "/StreamingLuaAssets/"; } }
    public static string BuildLuaPath
    {
        get
        {
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                return Application.streamingAssetsPath + "/lua/";
            }
            return Application.dataPath + "/StreamingGameAssets/lua/";
        }
    }

    public static string BuildAssetPath
    {
        get
        {
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                return Application.streamingAssetsPath + "/assetbundles";
            }

            return Application.dataPath + "/StreamingGameAssets/assetbundles";
        }
    }

    // -------------------------------------------------- 打包文件 --------------------------------------------------
    public static string PackerPath { get { return Application.dataPath + "/StreamingGameAssets/"; } }                      // 全局打包的目录
    public static string BuildPackerPath { get { return Application.dataPath + "/UpdateAsset/"; } }                         // 发布 UPK 文件夹路径
    public static string BuildPackerFilePath { get { return BuildPackerPath + "/UpdateAssets.upk"; } }                      // 发布 UPK 文件的路径
    public static string UploadServerPath { get { return "http://img.aboilgame.com/ftxgame/FTX_X3/"; } }                    // 上传文件的服务器地址
    public static string PackerFilePath { get { return Application.streamingAssetsPath + "/" + BuildConst.UnpackFile; } }   // 打包文件路径

    // -------------------------------------------------- 发布 Lua --------------------------------------------------
    public static string LuaPath { get { return Application.dataPath + "/" + AppName + "/lua/"; } }             // Lua 资源的路径
    public static string LuaConfigPath { get { return Application.dataPath + "/LuaFramework/Lua/Game/Configs/ConfigLua"; } }

    // ------------------------------------------------- 发布 Proto -------------------------------------------------
    public static string BuildPbPath { get { return Application.dataPath + "/LuaFramework/Lua/Game/Proto"; } }
    public static string ProtocPath { get { return (Application.dataPath + "/Build/protoc.exe").Replace("/Assets/", "/"); } }
}