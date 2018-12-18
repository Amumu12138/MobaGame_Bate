using UnityEditor;
using UnityEngine;

public class BuildPlatformUI : EditorWindow
{
    [MenuItem("LuaFramework/BuildPlatform", false, 904)]
    static void Init()
    {
        if (EditorApplication.isCompiling)
        {
            EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译在执行此功能", "确定");
            return;
        }

        BuildPlatformUI window = (BuildPlatformUI)GetWindow(typeof(BuildPlatformUI));
    }

    string generatePath;
    string bundleVersion = "1.0";
    string productName = "王者NBA";
    string bundleIdentifier = "com.aboilgame.wznba";

    const string GeneratePathKey = "GeneratePathKey";

    void OnEnable()
    {
        if (EditorPrefs.HasKey(GeneratePathKey))
        {
            generatePath = EditorPrefs.GetString(GeneratePathKey);
        }
    }

    void OnGUI()
    {
        GUILayout.Label("项目版本：" + bundleVersion);
        GUILayout.Label("项目名称：" + productName);
        GUILayout.Label("项目签名：" + bundleIdentifier);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("配置路径", GUILayout.Width(100)))
        {
            string tempPath = EditorUtility.OpenFolderPanel("生成文件路径", "", "");
            if (!string.IsNullOrEmpty(tempPath))
            {
                generatePath = tempPath;
                EditorPrefs.SetString(GeneratePathKey, tempPath);
            }
        }

        GUILayout.Label(generatePath);
        GUILayout.EndHorizontal();
#if UNITY_IPHONE
        if (GUILayout.Button("生成IOS资源"))
        {
            BuildIosApp();
        }
#else
        if (GUILayout.Button("发布SDK包"))
        {
            if (string.IsNullOrEmpty(generatePath))
            {
                EditorUtility.DisplayDialog("警告", "生成文件路径不能为空", "确定");
                return;
            }

            BuildAndroidApk();
        }
#endif
    }

    void BuildIosApp(string _versionType = "")
    {
        BuildBundleManager.mInst.BuildLuaIOS();             // 生成 Lua 资源
        BuildBundleManager.mInst.BuildFolderIOS();          // 生成 Fairy 资源
        BuildBundleManager.mInst.BuildIOS(_versionType);    // 生成 IOS 资源
    }

    void BuildAndroidApk(string _versionType = "")
    {
        BuildBundleManager.mInst.BuildLuaAndroid();                 // 生成 Lua 资源
        BuildBundleManager.mInst.BuildFolderAndroid();              // 生成 Fairy 资源
        BuildBundleManager.mInst.BuildAndroid(_versionType);        // 生成 Android 资源
        BuildUploadAssetManager.mInst.PackerAssets();               // 压缩全部资源
        BuildPlatformManager.BuildAndroid(generatePath, productName, bundleVersion, bundleIdentifier);
    }
}