using UnityEngine;
using UnityEditor;

public class BuildUploadAssetUI : EditorWindow
{
    [MenuItem("LuaFramework/BuildUploadAsset", false, 906)]
    static void Init()
    {
        if (EditorApplication.isCompiling)
        {
            EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译在执行此功能", "确定");
            return;
        }

        BuildUploadAssetUI window = (BuildUploadAssetUI)GetWindow(typeof(BuildUploadAssetUI));
    }

    string search;
    Vector2 scrollPosition;
    ModifyFileInfo[] modifyFileArray;

    void OnEnable()
    {
        UpdateModifyFileArray();
    }

    void OnGUI()
    {
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("保存全部文件"))
        {
            ModifyFileManager.mInst.WriteModifyFileArray();
            UpdateModifyFileArray();
        }

        if (GUILayout.Button("重新生成资源"))
        {
#if UNITY_IPHONE
            BuildBundleManager.mInst.BuildLuaIOS();
            BuildBundleManager.mInst.BuildIOS();
            BuildBundleManager.mInst.BuildFolderIOS();
#else
            BuildBundleManager.mInst.BuildLuaAndroid();
            BuildBundleManager.mInst.BuildAndroid();
            BuildBundleManager.mInst.BuildFolderAndroid();
#endif
            UpdateModifyFileArray();
        }

        GUI.backgroundColor = Color.white;
        search = EditorGUILayout.TextField(search, GUILayout.Height(20));

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        for (int i = 0; i < modifyFileArray.Length; i++)
        {
            string tempPath = modifyFileArray[i].AssetsFilePath;
            if (tempPath.Contains(".manifest"))
            {
                continue;
            }

            if (!string.IsNullOrEmpty(search) && !tempPath.Contains(search))
            {
                continue;
            }
            
            GUILayout.Label(tempPath);
        }
        GUILayout.EndScrollView();
#if UNITY_IPHONE
        if (GUILayout.Button("IOS测试服"))
        {
            CopyModifyFileArray();
            BuildUploadAssetManager.mInst.UploadAsset(BuildConst.TestIosServer);
        }

        if (GUILayout.Button("IOS越狱服"))
        {
            if (EditorUtility.DisplayDialog("警告", "本服务器是外网正式服，对所有线上玩家有作用", "确定"))
            {
                CopyModifyFileArray();
                BuildUploadAssetManager.mInst.UploadAsset(BuildConst.IosJailbreakingServer);
            }
        }

        if (GUILayout.Button("IOS正式服"))
        {
            if (EditorUtility.DisplayDialog("警告", "本服务器是外网正式服，对所有线上玩家有作用", "确定"))
            {
                CopyModifyFileArray();
                BuildUploadAssetManager.mInst.UploadAsset(BuildConst.IosServer);
            }
        }
#else
        if (GUILayout.Button("Android测试服"))
        {
            CopyModifyFileArray();
            BuildUploadAssetManager.mInst.UploadAsset(BuildConst.TestAndroidServer);
        }

        if (GUILayout.Button("Androil正式服"))
        {
            if (EditorUtility.DisplayDialog("警告", "本服务器是外网正式服，对所有线上玩家有作用", "确定"))
            {
                CopyModifyFileArray();
                BuildUploadAssetManager.mInst.UploadAsset(BuildConst.AndroidServer);
            }
        }
#endif
    }

    /// <summary>
    /// 复制一份资源到指定目录下进行压缩
    /// </summary>
    void CopyModifyFileArray()
    {
        for (int i = 0; i < modifyFileArray.Length; i++)
        {
            string tempPath = modifyFileArray[i].AssetsFilePath;
            string tempOldPath = tempPath;
            string tempNewPath = tempPath.Replace(BuildConst.PackerPath, BuildConst.BuildPackerPath);
            UtilityFile.CopyFile(tempOldPath, tempNewPath);
        }

        AssetDatabase.Refresh();
    }

    void UpdateModifyFileArray()
    {
        modifyFileArray = ModifyFileManager.mInst.ReadModifyFileArray();
    }
}