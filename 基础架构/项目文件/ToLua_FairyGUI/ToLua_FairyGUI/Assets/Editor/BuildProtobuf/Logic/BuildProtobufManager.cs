using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

public class BuildProtobufManager
{
    static BuildProtobufManager manager;
    public static BuildProtobufManager mInst
    {
        get
        {
            if (manager == null)
            {
                manager = new BuildProtobufManager();
            }

            return manager;
        }
    }

    public void GeneratePbFiles(string _protoPath)
    {
        List<FileInfo> tempProtoFileList = UtilityFile.GetDirectoryAllFile(_protoPath, "proto");
        UtilityFile.DeleteFileOrDirectory(BuildConst.BuildPbPath);
        UtilityFile.CreateDirectory(BuildConst.BuildPbPath);
        for (int i = 0; i < tempProtoFileList.Count; ++i)
        {
            FileInfo tempFile = tempProtoFileList[i];
            GeneratePbFile(tempFile.FullName, _protoPath);
        }
    }

    void GeneratePbFile(string _filePath, string _protoPath)
    {
        string tempFileName = Path.GetFileNameWithoutExtension(_filePath);
        ProcessStartInfo tempProcessInfo = new ProcessStartInfo();
        tempProcessInfo.ErrorDialog = true;
        tempProcessInfo.UseShellExecute = true;
        tempProcessInfo.FileName = BuildConst.ProtocPath;
        tempProcessInfo.WindowStyle = ProcessWindowStyle.Hidden;
        tempProcessInfo.Arguments = " -I " + _protoPath + " -o " + BuildConst.BuildPbPath + "/" + tempFileName + ".pb " + _filePath;

        Process tempProcess = Process.Start(tempProcessInfo);
        tempProcess.WaitForExit();
    }
}