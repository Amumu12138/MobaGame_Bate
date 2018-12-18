using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;

public class ModifyFileManager
{
    static ModifyFileManager manager;
    public static ModifyFileManager mInst
    {
        get
        {
            if (manager == null)
            {
                manager = new ModifyFileManager();
            }

            return manager;
        }
    }

    string configFilePath;
    ModifyFileLibrary modifyFileLib = new ModifyFileLibrary();

    public ModifyFileManager()
    {
        configFilePath = Application.dataPath + "/Editor/ModifyFile/Config/ModifyFile.txt";
        if (!IsExistModifyFile())
        {
            WriteModifyFileArray();
        }
    }

    public ModifyFileInfo[] ReadModifyFileArray()
    {
        return ReadFileArray(BuildConst.PackerPath, configFilePath);
    }

    public void WriteModifyFileArray()
    {
        WriteFileArray(BuildConst.PackerPath, configFilePath);
    }

    public ModifyFileInfo[] ReadFileArray(string _packerPath, string _filePath)
    {
        List<ModifyFileInfo> tempModifyFileList = new List<ModifyFileInfo>();

        modifyFileLib.AddModifyFileList(File.ReadAllText(_filePath));
        ModifyFileInfo[] tempModifyFileArray = modifyFileLib.GetModifyFileArray();
        List<FileInfo> tempList = UtilityFile.GetDirectoryAllFile(_packerPath);
        for (int i = 0; i < tempList.Count; i++)
        {
            FileInfo tempFileInfo = tempList[i];
            string tempFullName = tempFileInfo.FullName;
            if (tempFullName.Contains(".txt") || tempFullName.Contains(".meta") || tempFullName.Contains(".manifest"))
            {
                continue;
            }

            ModifyFileInfo tempInfo = modifyFileLib.GetTargetModifyFile(tempFullName);
            if (tempInfo != null)
            {
                if (tempFullName == tempInfo.FilePath && CalculateFileMd5(tempFullName) != tempInfo.FileDate)
                {
                    tempModifyFileList.Add(tempInfo);
                }
            }
            else
            {
                tempModifyFileList.Add(new ModifyFileInfo(tempFullName + "#" + CalculateFileMd5(tempFullName)));
            }
        }

        return tempModifyFileList.ToArray();
    }

    public void WriteFileArray(string _packerPath, string _filePath)
    {
        string tempTxt = "";
        List<FileInfo> tempList = UtilityFile.GetDirectoryAllFile(_packerPath);
        for (int i = 0; i < tempList.Count; i++)
        {
            FileInfo tempFileInfo = tempList[i];
            string tempFullName = tempFileInfo.FullName;
            if (tempFullName.Contains(".meta") || tempFullName.Contains(".manifest"))
            {
                continue;
            }

            tempTxt = tempTxt + (tempFullName + "#" + CalculateFileMd5(tempFullName)) + "\n";
        }

        File.WriteAllText(_filePath, tempTxt);
    }

    public bool IsExistModifyFile() { return File.Exists(configFilePath); }

    /// <summary>
    /// 计算字符串的MD5值
    /// </summary>
    string CalculateMd5(string _source)
    {
        MD5CryptoServiceProvider tempMd5 = new MD5CryptoServiceProvider();
        byte[] tempData = Encoding.UTF8.GetBytes(_source);
        byte[] tempMd5Data = tempMd5.ComputeHash(tempData, 0, tempData.Length);
        tempMd5.Clear();

        string tempDestString = "";
        for (int i = 0; i < tempMd5Data.Length; i++)
        {
            tempDestString += Convert.ToString(tempMd5Data[i], 16).PadLeft(2, '0');
        }
        tempDestString = tempDestString.PadLeft(32, '0');
        return tempDestString;
    }

    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    string CalculateFileMd5(string _file)
    {
        try
        {
            FileStream tempFs = new FileStream(_file, FileMode.Open);
            MD5 tempMd5 = new MD5CryptoServiceProvider();
            byte[] tempRetVal = tempMd5.ComputeHash(tempFs);
            tempFs.Close();

            StringBuilder tempSb = new StringBuilder();
            for (int i = 0; i < tempRetVal.Length; i++)
            {
                tempSb.Append(tempRetVal[i].ToString("x2"));
            }
            return tempSb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }
}