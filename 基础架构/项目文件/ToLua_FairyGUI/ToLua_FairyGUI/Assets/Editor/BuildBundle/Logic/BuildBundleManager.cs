using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BuildBundleManager
{
    static BuildBundleLibrary buildBundleLib = new BuildBundleLibrary();
    static BuildBundleLibrary buildBundleFolderLib = new BuildBundleLibrary();

    static BuildBundleManager buildBundleManager;
    public static BuildBundleManager mInst
    {
        get
        {
            if (buildBundleManager == null)
            {
                // ------------------------------ 资源打包 ------------------------------
                // buildBundleLib.AddBundleInfo("Textures/Prop", "prop_", "png");
                // buildBundleLib.AddBundleInfo("Common/Audio", "Audio_", "mp3");
                // buildBundleLib.AddBundleInfo("Effects/Prefabs", "effect_", "prefab");

                // ------------------------------ Fairy打包 -----------------------------
                List<string> tempFairyPathList = UtilityFile.GetAllDirectory(BuildConst.FairyAssetPath);
                for (int i = 0; i < tempFairyPathList.Count; i++)
                {
                    buildBundleFolderLib.AddBundleInfo(tempFairyPathList[i], "Fairy_", "bytes", "png");
                }

                buildBundleManager = new BuildBundleManager();
            }

            return buildBundleManager;
        }
    }

    public void BuildLuaIOS()
    {
        BuildAssetBundleLua(BuildTarget.iOS);
    }

    public void BuildLuaAndroid()
    {
        BuildAssetBundleLua(BuildTarget.Android);
    }

    public void BuildIOS(string _versionType = "")
    {
        BuildAssetBundle(BuildTarget.iOS, _versionType);
    }

    public void BuildAndroid(string _versionType = "")
    {
        BuildAssetBundle(BuildTarget.Android, _versionType);
    }

    public void BuildFolderIOS()
    {
        BuildAssetBundleFolder(BuildConst.FairyAssetPath, BuildTarget.iOS);
    }

    public void BuildFolderAndroid()
    {
        BuildAssetBundleFolder(BuildConst.FairyAssetPath, BuildTarget.Android);
    }

    public void GenerateAssetBundleNames()
    {
        List<BuildBundleInfo> tempList = buildBundleLib.GetBundleInfoList();
        for (int i = 0; i < tempList.Count; i++)
        {
            BuildBundleInfo tempInfo = tempList[i];
            if (tempInfo != null)
            {
                DoSetFolderAssetBundleName(tempInfo.Prefix, tempInfo.Path, tempInfo.SuffixArray);
            }
        }
    }

    public void RemoveAssetBundleNames()
    {
        List<BuildBundleInfo> tempList = buildBundleLib.GetBundleInfoList();
        for (int i = 0; i < tempList.Count; i++)
        {
            BuildBundleInfo tempInfo = tempList[i];
            if (tempInfo != null)
            {
                RemoveUnusedAssetBundleNames(tempInfo.Prefix, tempInfo.Path, tempInfo.SuffixArray);
            }
        }
    }

    void BuildAssetBundle(BuildTarget _target, string _versionType = "")
    {
        if (_target == BuildTarget.iOS)
        {
            File.WriteAllText(PlatformNumberFilePath, "2000");
            File.WriteAllText(VersionNumberFilePath, EditorTool.Utility.GetVersionNumber(BuildConst.IosServer).ToString());
        }
        else if (_target == BuildTarget.Android)
        {
            File.WriteAllText(PlatformNumberFilePath, "3000");
            File.WriteAllText(VersionNumberFilePath, EditorTool.Utility.GetVersionNumber(BuildConst.AndroidServer).ToString());
        }

        UtilityFile.CreateDirectory(BuildConst.BuildAssetPath);
        BuildPipeline.BuildAssetBundles(BuildConst.BuildAssetPath, BuildAssetBundleOptions.ChunkBasedCompression, _target);
    }

    void BuildAssetBundleFolder(string _folderPath, BuildTarget _target)
    {
        List<BuildBundleInfo> tempList = buildBundleFolderLib.GetBundleInfoList();
        AssetBundleBuild[] tempAssetBundleArray = new AssetBundleBuild[tempList.Count];
        for (int i = 0; i < tempAssetBundleArray.Length; i++)
        {
            BuildBundleInfo tempInfo = tempList[i];
            tempAssetBundleArray[i].assetBundleName = tempInfo.Prefix + tempInfo.Path + BuildConst.ExtName;
            List<FileParamInfo> tempInfoList = UtilityFile.GetDirectoryAllAssetsFile(_folderPath + tempInfo.Path, tempInfo.SuffixArray);
            string[] tempAssetNames = new string[tempInfoList.Count];
            for (int j = 0; j < tempAssetNames.Length; j++)
            {
                tempAssetNames[j] = tempInfoList[j].FilePath;
            }

            tempAssetBundleArray[i].assetNames = tempAssetNames;
        }

        UtilityFile.CreateDirectory(BuildConst.BuildFolderAssetPath);
        BuildPipeline.BuildAssetBundles(BuildConst.BuildFolderAssetPath, tempAssetBundleArray, BuildAssetBundleOptions.ChunkBasedCompression, _target);
    }

    void BuildAssetBundleLua(BuildTarget _target)
    {
        if (Directory.Exists(BuildConst.BuildLuaPath))
        {
            Directory.Delete(BuildConst.BuildLuaPath, true);
        }

        CopyLuaFile(Application.dataPath + "/LuaFramework/lua/", true);
        CopyLuaFile(Application.dataPath + "/LuaFramework/Tolua/Lua/", false);
        AssetDatabase.Refresh();

        List<FileParamInfo> tempInfoList = UtilityFile.GetDirectoryAllAssetsFile(BuildConst.BuildLuaTempPath, "bytes");
        for (int i = 0; i < tempInfoList.Count; i++)
        {
            BuildSingleAssetBundle(_target, tempInfoList[i].FilePath);
        }

        Directory.Delete(BuildConst.BuildLuaTempPath, true);
        AssetDatabase.Refresh();
    }

    void DoSetFolderAssetBundleName(string _name, string _folderName, string[] _searchPatternArray)
    {
        string tempPath = BuildConst.AssetPath + _folderName;
        UtilityFile.CreateDirectory(tempPath);

        List<FileParamInfo> tempInfoList = UtilityFile.GetDirectoryAllAssetsFile(tempPath, _searchPatternArray);
        if (tempInfoList == null)
        {
            return;
        }

        for (int i = 0; i < tempInfoList.Count; i++)
        {
            DoSetAssetBundleName(_name + tempInfoList[i].FileName, tempInfoList[i].FilePath);
        }
    }

    void RemoveUnusedAssetBundleNames(string _name, string _folderName, string[] _searchPatternArray)
    {
        string tempPath = BuildConst.AssetPath + _folderName;
        UtilityFile.CreateDirectory(tempPath);

        List<FileParamInfo> tempInfoList = UtilityFile.GetDirectoryAllAssetsFile(tempPath, _searchPatternArray);
        if (tempInfoList == null)
        {
            return;
        }

        for (int i = 0; i < tempInfoList.Count; i++)
        {
            DoSetAssetBundleName(null, tempInfoList[i].FilePath);
        }

        AssetDatabase.RemoveUnusedAssetBundleNames();
    }

    /// <summary>
    /// 修改bundle名字
    /// </summary>
    void DoSetAssetBundleName(string _name, string _path)
    {
        AssetImporter importer = AssetImporter.GetAtPath(_path);
        if (importer == null)
        {
            UnityEngine.Debug.LogError("资源不存在 --> " + _path);
            return;
        }

        if (_name == null)
        {
            importer.assetBundleName = null;
            importer.assetBundleVariant = "";
        }
        else
        {
            importer.assetBundleName = _name;
            importer.assetBundleVariant = "assetbundles";
        }
    }

    /// <summary>
    /// 打包单个文件为AB包
    /// </summary>
    void BuildSingleAssetBundle(BuildTarget _target, string _filePath)
    {
        string tempFileName = _filePath.Replace("Assets/StreamingLuaAssets/", "");
        AssetBundleBuild[] tempBuildArray = new AssetBundleBuild[1];
        tempBuildArray[0].assetNames = new string[] { _filePath };
        tempBuildArray[0].assetBundleName = tempFileName.Replace("/", "_").Replace(".lua.bytes", BuildConst.BuildLuaName);

        string tempOutPath = BuildConst.BuildLuaPath;
        if (!Directory.Exists(tempOutPath))
        {
            Directory.CreateDirectory(tempOutPath);
        }

        BuildPipeline.BuildAssetBundles(tempOutPath, tempBuildArray, BuildAssetBundleOptions.ChunkBasedCompression, _target);
    }

    void CopyLuaFile(string _filePath, bool _isToLower)
    {
        List<FileInfo> tempFileList = UtilityFile.GetDirectoryAllFile(_filePath);
        for (int i = 0; i < tempFileList.Count; i++)
        {
            if (tempFileList[i].Name.Contains(".meta"))
            {
                continue;
            }

            string tempFullFilePath = tempFileList[i].FullName.Replace(@"\", "/");
            string tempLuaFilePath = tempFullFilePath.Replace(_filePath, BuildConst.BuildLuaTempPath);
            string tempLuaFolderPath = Path.GetDirectoryName(tempLuaFilePath);
            string tempFilePath = tempFullFilePath.Replace(_filePath, BuildConst.BuildLuaPath);
            string tempFolderPath = Path.GetDirectoryName(tempFilePath);

            if (_isToLower)
            {
                tempFilePath = tempFilePath.ToLower();
                tempFolderPath = tempFolderPath.ToLower();
                tempLuaFilePath = tempLuaFilePath.ToLower();
                tempLuaFolderPath = tempLuaFolderPath.ToLower();
            }

            if (!Directory.Exists(tempLuaFolderPath))
            {
                Directory.CreateDirectory(tempLuaFolderPath);
            }

            if (!tempFullFilePath.Contains(".lua"))
            {
                if (!Directory.Exists(tempFolderPath))
                {
                    Directory.CreateDirectory(tempFolderPath);
                }

                File.Copy(tempFullFilePath, tempFilePath);
                continue;
            }

            string tempNewFile = tempLuaFilePath + BuildConst.BytesName;
            File.Copy(tempFullFilePath, tempNewFile);
        }
    }

    static string VersionNumberFilePath { get { return Application.streamingAssetsPath + "/" + VersionConst.VersionNumberFile; } }
    static string PlatformNumberFilePath { get { return Application.streamingAssetsPath + "/" + VersionConst.PlatformNumberFile; } }
}