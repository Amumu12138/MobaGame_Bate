using System.IO;
using UnityEngine;
using System.Text;
using System.Collections.Generic;

class OneFileInfo
{
    public int id = 0;
    public int size = 0;
    public int startPos = 0;
    public int pathLength = 0;
    public string filePath = "";
    public byte[] fileData = null;
};

public class UtilityPacker
{
    private static int id = 0;
    private static int totalSize = 0;
    private static Dictionary<int, OneFileInfo> allFileInfoDic = new Dictionary<int, OneFileInfo>();

    /** 遍历文件夹获取所有文件信息 **/
    static void TraverseFolder(string _folderPath)
    {
        Debug.Log("遍历文件夹 " + _folderPath);

        string tempSourceDirpath = _folderPath.Substring(0, _folderPath.LastIndexOf('/'));

        /** 读取文件夹下面所有文件的信息 **/
        DirectoryInfo tempDirInfo = new DirectoryInfo(_folderPath);

        foreach (FileInfo tempFileInfo in tempDirInfo.GetFiles("*.*", SearchOption.AllDirectories))
        {
            if (tempFileInfo.Extension == ".meta" || tempFileInfo.Extension == ".manifest")
            {
                continue;
            }

            string tempFileName = tempFileInfo.FullName.Replace("\\", "/");
            tempFileName = tempFileName.Replace(tempSourceDirpath + "/", "");

            int tempFileSize = (int)tempFileInfo.Length;

            Debug.Log(id + " : " + tempFileName + " 文件大小: " + tempFileSize);

            OneFileInfo tempInfo = new OneFileInfo();
            tempInfo.id = id;
            tempInfo.size = tempFileSize;
            tempInfo.filePath = tempFileName;
            tempInfo.pathLength = new UTF8Encoding().GetBytes(tempFileName).Length;

            /**  读取这个文件  **/
            FileStream tempFileStreamRead = new FileStream(tempFileInfo.FullName, FileMode.Open, FileAccess.Read);
            if (tempFileStreamRead == null)
            {
                Debug.Log("读取文件失败 ： " + tempFileInfo.FullName);
                return;
            }
            else
            {
                byte[] tempFileData = new byte[tempFileSize];
                tempFileStreamRead.Read(tempFileData, 0, tempFileSize);
                tempInfo.fileData = tempFileData;
            }
            tempFileStreamRead.Close();

            allFileInfoDic.Add(id, tempInfo);

            id++;
            totalSize += tempFileSize;
        }
    }

    /**  打包一个文件夹  **/
    public static void PackFolder(string _folderPath, string _upkFilePath)
    {
        TraverseFolder(_folderPath);

        Debug.Log("文件数量 : " + id);
        Debug.Log("文件总大小 : " + totalSize);

        /**  更新文件在UPK中的起始点  **/
        int tempFirstFileStartPos = 0 + 4;
        for (int tempIndex = 0; tempIndex < allFileInfoDic.Count; tempIndex++)
        {
            tempFirstFileStartPos += 4 + 4 + 4 + 4 + allFileInfoDic[tempIndex].pathLength;
        }

        int tempStartPos = 0;
        for (int tempIndex = 0; tempIndex < allFileInfoDic.Count; tempIndex++)
        {
            if (tempIndex == 0)
            {
                tempStartPos = tempFirstFileStartPos;
            }
            else
            {
                tempStartPos = allFileInfoDic[tempIndex - 1].startPos + allFileInfoDic[tempIndex - 1].size;//上一个文件的开始+文件大小;
            }

            allFileInfoDic[tempIndex].startPos = tempStartPos;
        }

        /**  写文件  **/
        FileStream tempFileStream = new FileStream(_upkFilePath, FileMode.Create);

        /**  文件总数量  **/
        byte[] tempTotalIdData = System.BitConverter.GetBytes(id);
        tempFileStream.Write(tempTotalIdData, 0, tempTotalIdData.Length);

        for (int tempIndex = 0; tempIndex < allFileInfoDic.Count; tempIndex++)
        {
            /** 写入ID **/
            byte[] tempIdData = System.BitConverter.GetBytes(allFileInfoDic[tempIndex].id);
            tempFileStream.Write(tempIdData, 0, tempIdData.Length);

            /**  写入StartPos  **/
            byte[] tempStartPosData = System.BitConverter.GetBytes(allFileInfoDic[tempIndex].startPos);
            tempFileStream.Write(tempStartPosData, 0, tempStartPosData.Length);

            /**  写入size  **/
            byte[] tempSizeData = System.BitConverter.GetBytes(allFileInfoDic[tempIndex].size);
            tempFileStream.Write(tempSizeData, 0, tempSizeData.Length);

            /**  写入pathLength  **/
            byte[] tempPathLengthData = System.BitConverter.GetBytes(allFileInfoDic[tempIndex].pathLength);
            tempFileStream.Write(tempPathLengthData, 0, tempPathLengthData.Length);

            /**  写入path  **/
            byte[] tempMyPathData = new UTF8Encoding().GetBytes(allFileInfoDic[tempIndex].filePath);

            tempFileStream.Write(tempMyPathData, 0, tempMyPathData.Length);
        }

        /**  写入文件数据  **/
        int tempTotalProcessSize = 0;
        foreach (var tempInfopair in allFileInfoDic)
        {
            OneFileInfo info = tempInfopair.Value;
            int tempSize = info.size;
            byte[] tempData = null;
            int tempProcessSize = 0;
            while (tempProcessSize < tempSize)
            {
                if (tempSize - tempProcessSize < 1024)
                {
                    tempData = new byte[tempSize - tempProcessSize];
                }
                else
                {
                    tempData = new byte[1024];
                }
                tempFileStream.Write(info.fileData, tempProcessSize, tempData.Length);

                tempProcessSize += tempData.Length;
                tempTotalProcessSize += tempData.Length;
            }
        }

        tempFileStream.Flush();
        tempFileStream.Close();

        /** 重置数据 **/
        id = 0;
        totalSize = 0;
        allFileInfoDic.Clear();
    }
}