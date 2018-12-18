using UnityEngine;

namespace LuaFramework
{
    public class AppConst
    {

        public const string LuaTempDir = "Lua/";                    // 临时目录
        public const string AppName = "LuaFramework";               // 应用程序名称
        public const string ExtName = ".assetbundles";              // 素材扩展名

        public static string FrameworkRoot
        {
            get
            {
                return Application.dataPath + "/" + AppName;
            }
        }
    }
}