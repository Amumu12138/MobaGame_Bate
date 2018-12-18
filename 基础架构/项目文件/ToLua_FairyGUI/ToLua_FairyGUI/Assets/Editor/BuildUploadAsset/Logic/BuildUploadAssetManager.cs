using System.IO;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.Collections.Generic;

public class BuildUploadAssetManager
{
    static BuildUploadAssetManager manager;
    public static BuildUploadAssetManager mInst
    {
        get
        {
            if (manager == null)
            {
                manager = new BuildUploadAssetManager();
            }

            return manager;
        }
    }

    string filePath;

    /// <summary>
    /// 压缩文件
    /// </summary>
    public void PackerAssets()
    {
        // 打包资源文件成 UPK 压缩文件
        string tempPath = BuildConst.PackerPath;

        UtilityFile.CreateDirectory(tempPath);
        UtilityPacker.PackFolder(tempPath, BuildConst.PackerFilePath);

        AssetDatabase.Refresh();
    }

    public void UploadAsset(string _serverName)
    {
        string tempPath = BuildConst.BuildPackerPath;
        string tempLuaPath = BuildConst.BuildPackerFilePath;
        UtilityPacker.PackFolder(tempPath, tempLuaPath);

        string tempTargetPath = Application.dataPath + "/" + (EditorTool.Utility.GetVersionNumber(_serverName) + 1) + "_" + GetFileSize(tempLuaPath) + "_" + GetTime() + ".upk";
        File.Move(tempLuaPath, tempTargetPath);

        Upload(tempTargetPath, _serverName);
    }

    long GetFileSize(string _path)
    {
        FileInfo fileInfo = new FileInfo(_path);
        return fileInfo.Length;
    }

    string GetTime() { return System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString(); }

    void Upload(string _filePath, string _serverName)
    {
        filePath = _filePath;
        string tempPscpPath = Application.dataPath.Replace("/Assets", "/Build/PSCP.EXE");
        string tempBuildPath = Application.dataPath.Replace("/Assets", "/Build/Build.bat");

        string tempUploadTemplate = File.ReadAllText(Application.dataPath + "/Editor/BuildUploadAsset/Template/UploadTemplate.txt");
        tempUploadTemplate = tempUploadTemplate.Replace("#PSCP_PATH", tempPscpPath);
        tempUploadTemplate = tempUploadTemplate.Replace("#PASSWORD", BuildConst.ServerPassword);
        tempUploadTemplate = tempUploadTemplate.Replace("#FILE_PATH", _filePath);
        tempUploadTemplate = tempUploadTemplate.Replace("#SERVER_NAME", BuildConst.ServerPath + _serverName);

        File.WriteAllText(tempBuildPath, tempUploadTemplate);

        Process tempProcess = Process.Start(tempBuildPath);
        tempProcess.EnableRaisingEvents = true;
        tempProcess.Exited += EventHandler;
    }

    void EventHandler(object _sender, System.EventArgs e)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}