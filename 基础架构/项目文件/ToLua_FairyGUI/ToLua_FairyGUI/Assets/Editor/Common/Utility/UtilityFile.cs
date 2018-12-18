using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public enum FileState
{
    Inexist,     // 不存在
    IsOpen,      // 已被打开
    Available,   // 当前可用
}

public class FileParamInfo
{
    string filePath;
    string fileName;
    string fileFullName;

    public FileParamInfo(string _filePath, string _fileName)
    {
        filePath = _filePath;
        fileName = _fileName.Split('.')[0];
        fileFullName = _fileName;
    }

    public string FilePath { get { return filePath; } }
    public string FileName { get { return fileName; } }
    public string FileFullName { get { return fileFullName; } }
}

public class UtilityFile
{
    [DllImport("kernel32.dll")]
    private static extern IntPtr _lopen(string lpPathName, int iReadWrite);
    [DllImport("kernel32.dll")]
    private static extern bool CloseHandle(IntPtr hObject);
    private const int OF_READWRITE = 2;
    private const int OF_SHARE_DENY_NONE = 0x40;
    private static readonly IntPtr HFILE_ERROR = new IntPtr(-1);

    public static FileState GetFileState(string filePath)
    {
        if (File.Exists(filePath))
        {
            IntPtr vHandle = _lopen(filePath, OF_READWRITE | OF_SHARE_DENY_NONE);
            if (vHandle == HFILE_ERROR)
            {
                return FileState.IsOpen;
            }

            CloseHandle(vHandle);
            return FileState.Available;
        }
        else
        {
            return FileState.Inexist;
        }
    }

    public static void CopyFile(string fromPath, string toPath)
    {
        if (File.Exists(fromPath))
        {
            CreateDirectoryByFilePath(toPath);
            File.Copy(fromPath, toPath, true);
        }
    }

    public static void CreateDirectoryByFilePath(string filePath)
    {
        string dirPath = GetDirectoryName(filePath);
        CreateDirectory(dirPath);
    }

    public static void CreateDirectory(string directoryPath)
    {
        if (Directory.Exists(directoryPath) == false)
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    /// <summary>
    /// 获取文件所在的文件夹路径
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string GetDirectoryName(string filePath)
    {
        return Path.GetDirectoryName(filePath);
    }

    /// <summary>
    /// 获取文件最后的修改时间
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string GetFileLastWriteTime(string filePath)
    {
        return File.GetLastWriteTime(filePath).ToString();
    }

    /// <summary>
    /// 获取文件夹下的全部文件
    /// </summary>
    /// <param name="_directoryPath"></param>
    /// <param name="_searchPattern"></param>
    public static List<FileInfo> GetDirectoryAllFile(string _directoryPath, string _searchPattern = "")
    {
        List<FileInfo> fileList = new List<FileInfo>();
        SearchAllFile(_directoryPath, _searchPattern, fileList);
        return fileList;
    }

    /// <summary>
    /// 获取文件夹下的全部文件
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <param name="searchPattern"></param>
    public static List<FileInfo> GetDirectoryAllFile(string directoryPath, string[] searchPattern)
    {
        List<FileInfo> fileList = new List<FileInfo>();
        int len = searchPattern.Length;
        for (int i = 0; i < len; ++i)
        {
            fileList.AddRange(GetDirectoryAllFile(directoryPath, searchPattern[i]));
        }
        return fileList;
    }

    /// <summary>
    /// 获取文件夹下面的全部文件夹名字
    /// </summary>
    /// <param name="_directoryPath">目标文件夹的路径</param>
    public static List<string> GetAllDirectory(string _directoryPath)
    {
        List<string> tempList = new List<string>();
        DirectoryInfo tempFolder = new DirectoryInfo(_directoryPath);
        DirectoryInfo[] tempDirectoryInfoArray = tempFolder.GetDirectories();
        for (int i = 0; i < tempDirectoryInfoArray.Length; i++)
        {
            tempList.Add(tempDirectoryInfoArray[i].Name);
        }

        return tempList;
    }

    /// <summary>
    /// 获取文件夹下的全部文件，这里是Unity专用的地址，不是完整的文件目录
    /// </summary>
    public static List<FileParamInfo> GetDirectoryAllAssetsFile(string _directoryPath, string _searchPattern)
    {
        List<FileInfo> fileList = new List<FileInfo>();
        SearchAllFile(_directoryPath, _searchPattern, fileList);

        List<FileParamInfo> infoList = new List<FileParamInfo>();
        for (int i = 0; i < fileList.Count; i++)
        {
            string tempName = fileList[i].Name;
            string tempPath = fileList[i].FullName;
            tempPath = tempPath.Replace(@"\", "/");
            tempPath = tempPath.Replace(UnityEngine.Application.dataPath, "Assets");

            infoList.Add(new FileParamInfo(tempPath, tempName));
        }

        return infoList;
    }

    /// <summary>
    /// 获取文件夹下的全部文件，这里是Unity专用的地址，不是完整的文件目录
    /// </summary>
    public static List<FileParamInfo> GetDirectoryAllAssetsFile(string directoryPath, string[] searchPattern)
    {
        List<FileParamInfo> infoList = new List<FileParamInfo>();
        for (int i = 0; i < searchPattern.Length; i++)
        {
            infoList.AddRange(GetDirectoryAllAssetsFile(directoryPath, searchPattern[i]));
        }

        return infoList;
    }

    /// <summary>
    /// 根据路径删除文件
    /// 如果传入的是文件的路径，就删除文件
    /// 如果传入的是文件夹的路径，就删除文件夹
    /// </summary>
    public static void DeleteFileOrDirectory(string path)
    {
        if (string.IsNullOrEmpty(path) == false)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return;
            }
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
    }

    public static void WriteAll(string path, string content, bool isUseDefault = false)
    {
        CreateDirectoryByFilePath(path);
        FileStream fs = new FileStream(path, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs, isUseDefault ? Encoding.Default : Encoding.UTF8);
        sw.Write(content);
        sw.Flush();
        sw.Close();
        fs.Close();
    }

    private static void SearchAllFile(string _directoryPath, string _searchPattern, List<FileInfo> _fileInfoList)
    {
        DirectoryInfo folder = new DirectoryInfo(_directoryPath);
        DirectoryInfo[] directoryInfoList = folder.GetDirectories();
        foreach (DirectoryInfo directory in directoryInfoList)
        {
            SearchAllFile(directory.FullName, _searchPattern, _fileInfoList);
        }

        FileInfo[] fileList = null;
        if (string.IsNullOrEmpty(_searchPattern))
        {
            fileList = folder.GetFiles();
        }
        else
        {
            fileList = folder.GetFiles("*." + _searchPattern);
        }
        
        foreach (FileInfo file in fileList)
        {
            _fileInfoList.Add(file);
        }
    }
}