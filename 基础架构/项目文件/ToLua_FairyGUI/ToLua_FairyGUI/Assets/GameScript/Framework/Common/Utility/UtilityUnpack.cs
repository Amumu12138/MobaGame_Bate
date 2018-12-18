using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections;
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

public class UtilityUnpack
{
    static UTF8Encoding encoding = new UTF8Encoding();
    static Dictionary<int, OneFileInfo> allFileInfoDic = new Dictionary<int, OneFileInfo>();

    public static IEnumerator UnpackFolder(string _upkFilePath, string _outFilePath, bool _isDownload = false)
    {
        int tempOffset = 0;
        int tempTotalSize = 0;
        int tempFileCount = 0;
        allFileInfoDic.Clear();
        FileStream tempUpkFilestream = new FileStream(_upkFilePath, FileMode.Open);
        long tempIdss = tempUpkFilestream.Length;
        tempUpkFilestream.Seek(0, SeekOrigin.Begin);

        //读取文件数量;
        byte[] tempTotaliddata = new byte[4];
        tempUpkFilestream.Read(tempTotaliddata, 0, 4);
        int tempFilecount = BitConverter.ToInt32(tempTotaliddata, 0);
        tempOffset += 4;

        //读取所有文件信息;
        for (int tempIndex = 0; tempIndex < tempFilecount; tempIndex++)
        {
            byte[] tempIdData = new byte[4];
            tempUpkFilestream.Seek(tempOffset, SeekOrigin.Begin);
            tempUpkFilestream.Read(tempIdData, 0, 4);
            int tempId = BitConverter.ToInt32(tempIdData, 0);
            tempOffset += 4;

            //读取StartPos;
            byte[] tempStartPosData = new byte[4];
            tempUpkFilestream.Seek(tempOffset, SeekOrigin.Begin);
            tempUpkFilestream.Read(tempStartPosData, 0, 4);
            int tempStartPos = BitConverter.ToInt32(tempStartPosData, 0);
            tempOffset += 4;

            //读取size;
            byte[] tempSizeData = new byte[4];
            tempUpkFilestream.Seek(tempOffset, SeekOrigin.Begin);
            tempUpkFilestream.Read(tempSizeData, 0, 4);
            int tempSize = BitConverter.ToInt32(tempSizeData, 0);
            tempOffset += 4;

            //读取pathLength;
            byte[] tempPathLengthData = new byte[4];
            tempUpkFilestream.Seek(tempOffset, SeekOrigin.Begin);
            tempUpkFilestream.Read(tempPathLengthData, 0, 4);
            int tempPathLength = BitConverter.ToInt32(tempPathLengthData, 0);
            tempOffset += 4;

            //读取path;
            byte[] tempPathData = new byte[tempPathLength];
            tempUpkFilestream.Seek(tempOffset, SeekOrigin.Begin);
            tempUpkFilestream.Read(tempPathData, 0, tempPathLength);
            string tempPath = encoding.GetString(tempPathData);
            tempOffset += tempPathLength;

            //添加到Dic;
            OneFileInfo tempInfo = new OneFileInfo();
            tempInfo.id = tempId;
            tempInfo.size = tempSize;
            tempInfo.filePath = tempPath;
            tempInfo.startPos = tempStartPos;
            tempInfo.pathLength = tempPathLength;
            allFileInfoDic.Add(tempId, tempInfo);

            tempTotalSize += tempSize;
        }
        Events.msInst.DispatchEvent(VersionEvent.UNPACC_SIZE, (float)tempTotalSize);

        //释放文件;
        int tempTotalProcessSize = 0;
        foreach (var tempInfoPair in allFileInfoDic)
        {
            OneFileInfo tempInfo = tempInfoPair.Value;

            int tempSize = tempInfo.size;
            string tempPath = tempInfo.filePath;
            int tempStartPos = tempInfo.startPos;

            //创建文件;
            string tempDirPath = "";
            if (tempPath.IndexOf('/') > -1)
            {
                tempDirPath = _outFilePath + tempPath.Substring(0, tempPath.LastIndexOf('/'));
            }
            else
            {
                tempDirPath = _outFilePath;
            }

            string tempFilePath = _outFilePath + tempPath;
            if (Directory.Exists(tempDirPath) == false)
            {
                Directory.CreateDirectory(tempDirPath);
            }
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }

            byte[] tmepFileData;
            int tempProcessSize = 0;
            FileStream tempFileStream = new FileStream(tempFilePath, FileMode.Create);
            while (tempProcessSize < tempSize)
            {
                if (tempSize - tempProcessSize < 1024)
                {
                    tmepFileData = new byte[tempSize - tempProcessSize];
                }
                else
                {
                    tmepFileData = new byte[1024];
                }

                //读取;
                tempUpkFilestream.Seek(tempStartPos + tempProcessSize, SeekOrigin.Begin);
                tempUpkFilestream.Read(tmepFileData, 0, tmepFileData.Length);

                tempFileStream.Write(tmepFileData, 0, tmepFileData.Length);
                tempProcessSize += tmepFileData.Length;
                tempTotalProcessSize += tmepFileData.Length;
            }

            if (_isDownload)
            {
                yield return null;
            }
            else
            {
                if (tempFileCount >= 10)
                {
                    tempFileCount = 0;
                    yield return null;
                }
            }

            tempFileCount++;
            Events.msInst.DispatchEvent(VersionEvent.UNPACC_PROGRESS, ((float)tempTotalProcessSize / (float)tempTotalSize));

            tempFileStream.Flush();
            tempFileStream.Close();
        }
        tempUpkFilestream.Close();

        // 解压完毕，删除文件
        File.Delete(_upkFilePath);
        Events.msInst.DispatchEvent(VersionEvent.UNPACC_COMPLETION, null);
    }
}