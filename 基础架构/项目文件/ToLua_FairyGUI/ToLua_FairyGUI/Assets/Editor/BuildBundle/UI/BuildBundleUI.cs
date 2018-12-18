using UnityEditor;
using UnityEngine;

public class BuildBundleUI : EditorWindow
{
    [MenuItem("LuaFramework/BuildBundle", false, 903)]
    static void Init()
    {
        if (EditorApplication.isCompiling)
        {
            EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译在执行此功能", "确定");
            return;
        }

        BuildBundleUI window = (BuildBundleUI)GetWindow(typeof(BuildBundleUI));
    }

    void OnGUI()
    {
#if UNITY_IPHONE
        if (GUILayout.Button("BuildLua"))
        {
            BuildBundleManager.mInst.BuildLuaIOS();
        }

        if (GUILayout.Button("BuildFairyGUI"))
        {
            BuildBundleManager.mInst.BuildFolderIOS();
        }

        if (GUILayout.Button("BuildAssetBundle"))
        {
            BuildBundleManager.mInst.BuildIOS();
        }
#else
        if (GUILayout.Button("BuildLua"))
        {
            BuildBundleManager.mInst.BuildLuaAndroid();
        }

        if (GUILayout.Button("BuildFairyGUI"))
        {
            BuildBundleManager.mInst.BuildFolderAndroid();
        }

        if (GUILayout.Button("BuildAssetBundle"))
        {
            BuildBundleManager.mInst.BuildAndroid();
        }
#endif

        if (GUILayout.Button("RemoveAssetBundleNames"))
        {
            BuildBundleManager.mInst.RemoveAssetBundleNames();
        }

        if (GUILayout.Button("GenerateAssetBundleNames"))
        {
            BuildBundleManager.mInst.GenerateAssetBundleNames();
        }
    }
}