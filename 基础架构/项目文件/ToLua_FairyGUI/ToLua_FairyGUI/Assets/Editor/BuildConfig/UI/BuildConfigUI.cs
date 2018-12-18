using UnityEditor;
using UnityEngine;

public class BuildConfigUI : EditorWindow
{
    [MenuItem("LuaFramework/BuildConfig", false, 902)]
    static void Init()
    {
        if (EditorApplication.isCompiling)
        {
            EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译在执行此功能", "确定");
            return;
        }

        BuildConfigUI window = (BuildConfigUI)GetWindow(typeof(BuildConfigUI));
    }

    const string XlsxPathKey = "XlsxPathKey";

    string luaPath;
    string xlsxPath;

    void OnEnable()
    {
        luaPath = BuildConst.LuaConfigPath;

        if (EditorPrefs.HasKey(XlsxPathKey))
        {
            xlsxPath = EditorPrefs.GetString(XlsxPathKey);
        }
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("配置路径", GUILayout.Width(100)))
        {
            string tempPath = EditorUtility.OpenFolderPanel("Xlsx 文件路径", "", "");
            if (!string.IsNullOrEmpty(tempPath))
            {
                xlsxPath = tempPath;
                EditorPrefs.SetString(XlsxPathKey, tempPath);
            }
        }

        GUILayout.Label(xlsxPath);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("跳转路径", GUILayout.Width(100)))
        {
            System.Diagnostics.Process.Start(luaPath);
        }
        GUILayout.Label(luaPath);
        GUILayout.EndHorizontal();

        if (!string.IsNullOrEmpty(xlsxPath) && !string.IsNullOrEmpty(luaPath) && GUILayout.Button("生成"))
        {
            CreateManager tempManager = new CreateManager(xlsxPath);
            tempManager.GenerateLua_Client(luaPath);
            
            Debug.LogError("配置生成完毕");
        }
    }
}
