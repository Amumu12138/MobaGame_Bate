using UnityEditor;
using UnityEngine;

public class BuildProtobufUI : EditorWindow
{
    [MenuItem("LuaFramework/BuildProtobuf", false, 905)]
    static void Init()
    {
        if (EditorApplication.isCompiling)
        {
            EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译在执行此功能", "确定");
            return;
        }

        BuildProtobufUI window = (BuildProtobufUI)GetWindow(typeof(BuildProtobufUI));
    }

    const string ProtoPathKey = "ProtoPathKey";

    string protoPath;
    string protocPath;

    void OnEnable()
    {
        protocPath = BuildConst.ProtocPath;
        if (EditorPrefs.HasKey(ProtoPathKey))
        {
            protoPath = EditorPrefs.GetString(ProtoPathKey);
        }
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("配置路径", GUILayout.Width(100)))
        {
            string tempPath = EditorUtility.OpenFolderPanel("Proto 文件路径", "", "");
            if (!string.IsNullOrEmpty(tempPath))
            {
                protoPath = tempPath;
                EditorPrefs.SetString(ProtoPathKey, tempPath);
            }
        }

        GUILayout.Label(protoPath);
        GUILayout.EndHorizontal();

        GUILayout.Label(protocPath);

        if (!string.IsNullOrEmpty(protoPath) && !string.IsNullOrEmpty(protocPath) && GUILayout.Button("生成"))
        {
            BuildProtobufManager.mInst.GeneratePbFiles(protoPath);

            AssetDatabase.Refresh();
            Debug.LogError("-----> Proto 生成完毕 <-----");
        }
    }
}